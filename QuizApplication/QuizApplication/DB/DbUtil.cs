using QuizApplication.Helpers;
using QuizApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApplication.DB
{
    internal class DbUtil
    {
        private const string dbPath = "Quiz.db";
        // This method initializes the database and creates the necessary tables if they do not exist.
        public static void init()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);


                var connectionString = $"Data Source={dbPath};Version=3;";
                using var connection = new SQLiteConnection(connectionString);
                connection.Open();

                // create tables
                var createQuestionTable = @"
                CREATE TABLE IF NOT EXISTS Question (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    questionText TEXT NOT NULL,
                    imagePath TEXT,
                    questionType TEXT NOT NULL
                );";

                var createAnswerTable = @"
                CREATE TABLE IF NOT EXISTS Answer (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    questionId INTEGER NOT NULL,
                    optionIndex INTEGER NOT NULL,
                    optionText TEXT NOT NULL,
                    isCorrect INTEGER NOT NULL,
                    FOREIGN KEY(questionId) REFERENCES Question(id)
                );";

                string createScoreTable = @"
                CREATE TABLE IF NOT EXISTS Score (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    score REAL NOT NULL,
                    answerTime TEXT NOT NULL
                );";

                new SQLiteCommand(createQuestionTable, connection).ExecuteNonQuery();
                new SQLiteCommand(createAnswerTable, connection).ExecuteNonQuery();
                new SQLiteCommand(createScoreTable, connection).ExecuteNonQuery();

                // create question bank
                var questions = QuestionBank.GetRandomizedQuestions();

                foreach (var question in questions)
                {
                    // insert into question table
                    var insertQuestion = new SQLiteCommand("INSERT INTO Question (questionText, imagePath, questionType) VALUES (@text, @img, @type)", connection);
                    insertQuestion.Parameters.AddWithValue("@text", question.QuestionText);
                    insertQuestion.Parameters.AddWithValue("@img", question.ImagePath);
                    insertQuestion.Parameters.AddWithValue("@type", question.QuestionType);
                    insertQuestion.ExecuteNonQuery();

                    // get Question ID
                    long questionId = connection.LastInsertRowId;

                    for (int i = 0; i < question.Options.Length; i++)
                    {
                        bool isCorrect = false;
                        if (question is MCQQuestion mcq)
                            isCorrect = (i == mcq.CorrectAnswerIndex);
                        else if (question is MultiAnswerQuestion maq)
                            isCorrect = maq.CorrectAnswerIndexes.Contains(i);

                        var insertAnswer = new SQLiteCommand("INSERT INTO Answer (questionId, optionIndex, optionText, isCorrect) VALUES (@qid, @idx, @opt, @correct)", connection);
                        insertAnswer.Parameters.AddWithValue("@qid", questionId);
                        insertAnswer.Parameters.AddWithValue("@idx", i);
                        insertAnswer.Parameters.AddWithValue("@opt", question.Options[i]);
                        insertAnswer.Parameters.AddWithValue("@correct", isCorrect ? 1 : 0);
                        insertAnswer.ExecuteNonQuery();
                    }
                }
            }

        }


        // This method retrieves all questions from the database.
        public static List<Question> LoadRandomQuestions()
        {
            var questions = new List<Question>();

            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();

            var questionCmd = new SQLiteCommand("SELECT id, questionText, imagePath, questionType FROM Question", connection);
            using var questionReader = questionCmd.ExecuteReader();

            while (questionReader.Read())
            {
                long questionId = questionReader.GetInt64(0);
                string questionText = questionReader.GetString(1);
                string imagePath = questionReader.IsDBNull(2) ? "" : questionReader.GetString(2);
                string questionType = questionReader.GetString(3);

                var answerCmd = new SQLiteCommand("SELECT optionIndex, optionText, isCorrect FROM Answer WHERE questionId = @qid", connection);
                answerCmd.Parameters.AddWithValue("@qid", questionId);
                using var answerReader = answerCmd.ExecuteReader();

                var options = new List<(int index, string text, bool isCorrect)>();
                while (answerReader.Read())
                {
                    int index = answerReader.GetInt32(0);
                    string text = answerReader.GetString(1);
                    bool isCorrect = answerReader.GetInt32(2) == 1;
                    options.Add((index, text, isCorrect));
                }

                // ✅ randomize the order of options
                var random = new Random();
                var shuffled = options.OrderBy(x => random.Next()).ToList();
                string[] shuffledTexts = shuffled.Select(x => x.text).ToArray();

                Question q;
                if (questionType == "MCQ")
                {
                    int newCorrectIndex = shuffled.FindIndex(x => x.isCorrect);
                    q = new MCQQuestion
                    {
                        QuestionText = questionText,
                        Options = shuffledTexts,
                        CorrectAnswerIndex = newCorrectIndex,
                        ImagePath = imagePath
                    };
                }
                else if (questionType == "MultiAnswer")
                {
                    List<int> newCorrectIndexes = shuffled
                        .Select((x, i) => (x, i))
                        .Where(pair => pair.x.isCorrect)
                        .Select(pair => pair.i)
                        .ToList();

                    q = new MultiAnswerQuestion
                    {
                        QuestionText = questionText,
                        Options = shuffledTexts,
                        CorrectAnswerIndexes = newCorrectIndexes,
                        ImagePath = imagePath
                    };
                }
                else
                {
                    continue;
                }

                questions.Add(q);
            }

            // ✅ randomize the order of questions
            var finalRandom = new Random();
            return questions.OrderBy(q => finalRandom.Next()).ToList();
        }

        // This method retrieves all questions from the database without randomization.
        public static List<Question> GetQuestions()
        {
            var questions = new List<Question>();

            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();

            var questionCmd = new SQLiteCommand("SELECT id, questionText, imagePath, questionType FROM Question", connection);
            using var questionReader = questionCmd.ExecuteReader();

            while (questionReader.Read())
            {
                int questionId = questionReader.GetInt32(0);
                string questionText = questionReader.GetString(1);
                string imagePath = questionReader.IsDBNull(2) ? "" : questionReader.GetString(2);
                string questionType = questionReader.GetString(3);

                var answerCmd = new SQLiteCommand("SELECT optionIndex, optionText, isCorrect FROM Answer WHERE questionId = @qid ORDER BY optionIndex", connection);
                answerCmd.Parameters.AddWithValue("@qid", questionId);
                using var answerReader = answerCmd.ExecuteReader();

                var options = new List<string>();
                var correctIndexes = new List<int>();

                while (answerReader.Read())
                {
                    int index = answerReader.GetInt32(0);
                    string text = answerReader.GetString(1);
                    bool isCorrect = answerReader.GetInt32(2) == 1;

                    options.Add(text);
                    if (isCorrect) correctIndexes.Add(index);
                }

                Question q;
                if (questionType == "MCQ")
                {
                    q = new MCQQuestion
                    {
                        Id = questionId,
                        QuestionText = questionText,
                        Options = options.ToArray(),
                        CorrectAnswerIndex = correctIndexes.FirstOrDefault(),
                        ImagePath = imagePath
                    };
                }
                else if (questionType == "MultiAnswer")
                {
                    q = new MultiAnswerQuestion
                    {
                        Id = questionId,
                        QuestionText = questionText,
                        Options = options.ToArray(),
                        CorrectAnswerIndexes = correctIndexes,
                        ImagePath = imagePath
                    };
                }
                else
                {
                    continue;
                }

                questions.Add(q);
            }

            return questions;
        }
        public static void AddQuestion(Question question)
        {
            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();

            // insert into Question table
            var insertQ = new SQLiteCommand("INSERT INTO Question (questionText, imagePath, questionType) VALUES (@text, @img, @type)", connection);
            insertQ.Parameters.AddWithValue("@text", question.QuestionText);
            insertQ.Parameters.AddWithValue("@img", question.ImagePath ?? "");
            insertQ.Parameters.AddWithValue("@type", question.QuestionType);
            insertQ.ExecuteNonQuery();

            long questionId = connection.LastInsertRowId;

            // insert into Answer table
            for (int i = 0; i < question.Options.Length; i++)
            {
                bool isCorrect = false;

                if (question is MCQQuestion mcq)
                    isCorrect = (i == mcq.CorrectAnswerIndex);
                else if (question is MultiAnswerQuestion maq)
                    isCorrect = maq.CorrectAnswerIndexes.Contains(i);

                var insertA = new SQLiteCommand("INSERT INTO Answer (questionId, optionIndex, optionText, isCorrect) VALUES (@qid, @idx, @text, @correct)", connection);
                insertA.Parameters.AddWithValue("@qid", questionId);
                insertA.Parameters.AddWithValue("@idx", i);
                insertA.Parameters.AddWithValue("@text", question.Options[i]);
                insertA.Parameters.AddWithValue("@correct", isCorrect ? 1 : 0);
                insertA.ExecuteNonQuery();
            }
        }

        // update question
        public static void UpdateQuestion(Question q)
        {
            using var conn = new SQLiteConnection("Data Source=Quiz.db;Version=3;");
            conn.Open();

            var updateQ = new SQLiteCommand("UPDATE Question SET questionText = @text, imagePath = @img WHERE id = @id", conn);
            updateQ.Parameters.AddWithValue("@text", q.QuestionText);
            updateQ.Parameters.AddWithValue("@img", q.ImagePath ?? "");
            updateQ.Parameters.AddWithValue("@id", q.Id);
            updateQ.ExecuteNonQuery();

            var deleteOld = new SQLiteCommand("DELETE FROM Answer WHERE questionId = @qid", conn);
            deleteOld.Parameters.AddWithValue("@qid", q.Id);
            deleteOld.ExecuteNonQuery();

            for (int i = 0; i < q.Options.Length; i++)
            {
                bool isCorrect = false;
                if (q is MCQQuestion mcq) isCorrect = mcq.CorrectAnswerIndex == i;
                else if (q is MultiAnswerQuestion maq) isCorrect = maq.CorrectAnswerIndexes.Contains(i);

                var insertA = new SQLiteCommand("INSERT INTO Answer (questionId, optionIndex, optionText, isCorrect) VALUES (@qid, @idx, @text, @correct)", conn);
                insertA.Parameters.AddWithValue("@qid", q.Id);
                insertA.Parameters.AddWithValue("@idx", i);
                insertA.Parameters.AddWithValue("@text", q.Options[i]);
                insertA.Parameters.AddWithValue("@correct", isCorrect ? 1 : 0);
                insertA.ExecuteNonQuery();
            }
        }

        // delete question
        public static void DeleteQuestion(Question question)
        {
            using var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            conn.Open();

            var cmd1 = new SQLiteCommand("DELETE FROM Answer WHERE questionId = @qid", conn);
            cmd1.Parameters.AddWithValue("@qid", question.Id);
            cmd1.ExecuteNonQuery();

            var cmd2 = new SQLiteCommand("DELETE FROM Question WHERE id = @qid", conn);
            cmd2.Parameters.AddWithValue("@qid", question.Id);
            cmd2.ExecuteNonQuery();
        }

        public static void InsertUserScore(UserScore score)
        {
            using var conn = new SQLiteConnection("Data Source=Quiz.db;Version=3;");
            conn.Open();

            var cmd = new SQLiteCommand("INSERT INTO Score (username, score, answerTime) VALUES (@name, @score, @time)", conn);
            cmd.Parameters.AddWithValue("@name", score.Username);
            cmd.Parameters.AddWithValue("@score", score.Score);
            cmd.Parameters.AddWithValue("@time", score.AnswerTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }

        public static List<UserScore> GetAllUserScores()
        {
            var list = new List<UserScore>();
            using var conn = new SQLiteConnection("Data Source=Quiz.db;Version=3;");
            conn.Open();

            var cmd = new SQLiteCommand("SELECT username, score, answerTime FROM Score", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var score = new UserScore
                {
                    Username = reader.GetString(0),
                    Score = reader.GetDouble(1),
                    AnswerTime = DateTime.Parse(reader.GetString(2))
                };
                list.Add(score);
            }

            return list;
        }
    }
}

