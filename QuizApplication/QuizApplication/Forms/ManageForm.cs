using QuizApplication.DB;
using QuizApplication.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuizApplication.Forms
{
    public partial class ManageForm : Form
    {
        public ManageForm()
        {
            InitializeComponent();
            RebuildUI();
        }

        private void RebuildUI()
        {
            Controls.Clear();

            var questionTable = BuildQuestionTable();
            Controls.Add(questionTable);

            var btnAdd = new Button
            {
                Text = "Add Question",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(160, 40),
                Location = new Point(150, questionTable.Bottom + 20)
            };
            btnAdd.Click += BtnAdd_Click;
            Controls.Add(btnAdd);

            var btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(160, 40),
                Location = new Point(150, btnAdd.Bottom + 20)
            };
            btnClose.Click += (s, e) => this.Close();
            Controls.Add(btnClose);

            this.ClientSize = new Size(1000, btnClose.Bottom + 40);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Manage Question";
        }

        private TableLayoutPanel BuildQuestionTable()
        {
            List<Question> questions = DbUtil.GetQuestions();  // Get questions from DB
            var table = new TableLayoutPanel
            {
                ColumnCount = 4,
                Location = new Point(30, 20),
                Size = new Size(900, 40 * (questions.Count + 1) + 10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                AutoScroll = false
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40)); // Question
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // Options
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10)); // Correct Answer
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20)); // Button

            // Table headers
            table.Controls.Add(new Label { Text = "Question Text", Font = new Font("Segoe UI", 10, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
            table.Controls.Add(new Label { Text = "Options", Font = new Font("Segoe UI", 10, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            table.Controls.Add(new Label { Text = "Correct Answer", Font = new Font("Segoe UI", 10, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter }, 2, 0);
            table.Controls.Add(new Label { Text = "Actions", Font = new Font("Segoe UI", 10, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter }, 3, 0);

            // Data rows
            for (int i = 0; i < questions.Count; i++)
            {
                var question = questions[i];

                // Question text
                table.Controls.Add(new Label { Text = question.QuestionText, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, i + 1);

                // Options
                var options = string.Join(", ", question.Options);
                table.Controls.Add(new Label { Text = options, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, i + 1);

                // Correct Answer
                string correctAnswer = "";
                if (question is MultiAnswerQuestion multi)
                {
                    correctAnswer = string.Join(", ", multi.CorrectAnswerIndexes.Select(idx => question.Options[idx]));
                }
                else if (question is MCQQuestion mcq)
                {
                    correctAnswer = question.Options[mcq.CorrectAnswerIndex];
                }
                table.Controls.Add(new Label { Text = correctAnswer, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, i + 1);

                // Actions panel
                var btnEdit = new Button
                {
                    Text = "Edit",
                    Font = new Font("Segoe UI", 8, FontStyle.Regular),
                    Size = new Size(60, 30),
                    Tag = question
                };
                btnEdit.Click += BtnEdit_Click;

                var btnDelete = new Button
                {
                    Text = "Delete",
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    Size = new Size(80, 30),
                    Tag = question
                };
                btnDelete.Click += BtnDelete_Click; // ✅ Wire up the delete click event

                var panel = new FlowLayoutPanel { AutoSize = true };
                panel.Controls.Add(btnEdit);
                panel.Controls.Add(btnDelete);
                table.Controls.Add(panel, 3, i + 1);
            }

            return table;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new AddQuestionForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshTable();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag is Question question)
            {
                var form = new EditQuestionForm(question);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshTable();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var btnDelete = sender as Button;
            var question = btnDelete?.Tag as Question;

            if (question != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the question: \"{question.QuestionText}\"?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DbUtil.DeleteQuestion(question); // Delete from database
                    RefreshTable(); // Refresh the UI
                }
            }
        }

        private void RefreshTable()
        {
            RebuildUI();
        }
    }
}
