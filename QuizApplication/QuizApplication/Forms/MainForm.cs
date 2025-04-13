using QuizApplication.Models;
using QuizApplication.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuizApplication.DB;

namespace QuizApplication
{
    public partial class MainForm : Form
    {
        private List<Question> quizQuestions;
        private string playerName;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private System.Windows.Forms.Timer questionTimer;  // Specify full path for Timer
        private int timeLeft = 60;
        private List<UserAnswer> userAnswers = new List<UserAnswer>();

        private const string defaultImagePath = "Assets/default.png"; // Default image path if no image is provided

        public bool RestartRequested { get; private set; } = false;

        public MainForm(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
            InitializeTimer();
            quizQuestions = DbUtil.LoadRandomQuestions();
            LoadQuestion();
        }

        private void InitializeTimer()
        {
            questionTimer = new System.Windows.Forms.Timer();  // Specify full path for Timer
            questionTimer.Interval = 1000;
            questionTimer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Time: {timeLeft}";
            if (timeLeft <= 0)
            {
                questionTimer.Stop();
                MessageBox.Show("Time's up!");
                btnSubmit.Enabled = false;
            }
        }

        private void LoadQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                ShowResults();
                return;
            }

            pnlOptions.Controls.Clear();
            var question = quizQuestions[currentQuestionIndex];
            lblQuestion.Text = question.QuestionText;

            // dispose old image resource
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, question.ImagePath);
            if (!string.IsNullOrEmpty(question.ImagePath) && File.Exists(fullPath))
            {
                pictureBox.Image = Image.FromFile(fullPath);
                pictureBox.Visible = true;
            }
            else
            {
                string defaultFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultImagePath);
                try
                {
                    pictureBox.Image = Image.FromFile(defaultFullPath);
                    pictureBox.Visible = true;
                }
                catch (Exception ex)
                {
                    pictureBox.Visible = false;
                }
            }
            pictureBox.BorderStyle = BorderStyle.FixedSingle;

            LoadOptions(question.Options, question is MultiAnswerQuestion);

            lblFeedback.Text = "";
            timeLeft = 60;
            lblTimer.Text = $"Time: {timeLeft}";
            btnSubmit.Enabled = true;
            questionTimer.Start();
        }

        private void LoadOptions(string[] options, bool isMultiAnswer)
        {
            int top = 10;
            foreach (var option in options.Select((text, index) => new { text, index }))
            {
                Control ctrl = isMultiAnswer
                    ? new CheckBox { Text = option.text, Tag = option.index, Font = new Font("Segoe UI", 12F), AutoSize = true }
                    : new RadioButton { Text = option.text, Tag = option.index, Font = new Font("Segoe UI", 12F), AutoSize = true };

                // Set location with padding and consistent spacing
                ctrl.Location = new Point(10, top);
                pnlOptions.Controls.Add(ctrl);
                top += 40;
            }

            // Make sure to adjust panel height dynamically based on number of options
            pnlOptions.Height = top + 10; // Padding at the bottom
            pictureBox.Height = pnlOptions.Height; // Match picture box height to options panel
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            questionTimer.Stop();
            var question = quizQuestions[currentQuestionIndex];
            bool isCorrect = false;
            List<string> selectedAnswers = new List<string>();

            if (question is MCQQuestion mcq)
            {
                var selected = pnlOptions.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                if (selected == null) { MessageBox.Show("Select an option."); return; }
                int index = (int)selected.Tag;
                isCorrect = mcq.CheckAnswer(index);
                selectedAnswers.Add(mcq.Options[index]);
            }
            else if (question is MultiAnswerQuestion maq)
            {
                var selected = pnlOptions.Controls.OfType<CheckBox>().Where(c => c.Checked).Select(c => (int)c.Tag).ToList();
                if (!selected.Any()) { MessageBox.Show("Select at least one option."); return; }
                isCorrect = maq.CheckAnswer(selected);
                selectedAnswers = selected.Select(i => maq.Options[i]).ToList();
            }

            if (isCorrect) score++;
            userAnswers.Add(new UserAnswer
            {
                Question = question,
                SelectedAnswers = selectedAnswers,
                IsCorrect = isCorrect
            });

            lblFeedback.Text = isCorrect ? "✔ Correct!" : "✘ Incorrect!";
            lblFeedback.ForeColor = isCorrect ? Color.Green : Color.Red;
            btnSubmit.Enabled = false;

            await Task.Delay(1000);
            currentQuestionIndex++;
            LoadQuestion();
        }

        private void ShowResults()
        {
            questionTimer.Stop();
            ResultForm resultForm = new ResultForm(playerName, score, quizQuestions.Count, userAnswers);

            DialogResult result = resultForm.ShowDialog();

            if (result == DialogResult.Retry)
            {
                RestartRequested = true;
                this.Close();
            }
            else
            {
                this.Close(); // Exit application
            }
        }
    }
}