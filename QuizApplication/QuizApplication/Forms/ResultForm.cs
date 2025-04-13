using QuizApplication.DB;
using QuizApplication.Forms;
using QuizApplication.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuizApplication
{
    public partial class ResultForm : Form
    {
        private string playerName;
        private int score;
        private int total;
        private List<UserAnswer> _userAnswers;

        public ResultForm(string playerName, int score, int total, List<UserAnswer> userAnswers)
        {
            InitializeComponent();
            this.playerName = playerName;
            this.score = score;
            this.total = total;
            this._userAnswers = userAnswers;
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            double percentage = (double)score / total * 100;
            string grade = "";

            if (percentage >= 90)
                grade = "🏆 Excellent!";
            else if (percentage >= 70)
                grade = "🎯 Great Job!";
            else if (percentage >= 50)
                grade = "👍 Good Effort!";
            else
                grade = "💡 Keep Practicing!";

            lblSummary.Text = $"Player: {playerName}\n\n" +
                              $"You scored {score} out of {total}!\n\n" +
                              $"Percentage: {percentage:0.00}%\n\n" +
                              $"Result: {grade}";

            var userScore = new UserScore
            {
                Username = playerName,
                Score = percentage
            };
            DbUtil.InsertUserScore(userScore);
            if (percentage == 100)
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void btnBoard_Click(object sender, EventArgs e)
        {
            new BoardForm().ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.MediumSlateBlue;
                btn.ForeColor = Color.White;
            }
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Name == "btnReviewIncorrect")
                {
                    btn.BackColor = Color.LightSkyBlue;
                }
                else
                {
                    btn.BackColor = SystemColors.Control;
                }

                btn.ForeColor = Color.Black;
            }
        }

        private void lblSummary_Click(object sender, EventArgs e)
        {
            // Optional: handle click on lblSummary if needed
        }

        private void btnReviewIncorrect_Click(object sender, EventArgs e)
        {
            var incorrectAnswers = _userAnswers
                .Where(a => !a.IsCorrect)
                .ToList();

            if (incorrectAnswers.Count == 0)
            {
                MessageBox.Show("🎉 Congratulations! You got all answers correct!", "No Incorrect Answers");
                return;
            }

            string review = "❌ Incorrect Answers:\n\n";

            foreach (var answer in incorrectAnswers)
            {
                review += $"Q: {answer.Question.QuestionText}\n";
                review += $"Your Answer: {string.Join(", ", answer.SelectedAnswers)}\n";

                if (answer.Question is MCQQuestion mcq)
                {
                    review += $"Correct Answer: {mcq.Options[mcq.CorrectAnswerIndex]}\n\n";
                }
                else if (answer.Question is MultiAnswerQuestion maq)
                {
                    var correctAnswers = maq.CorrectAnswerIndexes.Select(i => maq.Options[i]);
                    review += $"Correct Answers: {string.Join(", ", correctAnswers)}\n\n";
                }
            }

            MessageBox.Show(review, "Review Incorrect Answers", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}