using QuizApplication.DB;
using QuizApplication.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuizApplication.Forms
{
    public partial class EditQuestionForm : Form
    {
        private TextBox txtQuestion;
        private ComboBox cmbType;
        private TextBox txtImagePath;
        private List<TextBox> optionBoxes = new();
        private List<CheckBox> checkBoxes = new();
        private List<RadioButton> radioButtons = new();

        private Question original;

        public EditQuestionForm(Question question)
        {
            InitializeComponent();
            this.original = question;
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            this.Text = "Edit Question";
            this.Size = new Size(650, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            // Question text
            var lblQuestion = new Label { Text = "Question:", Location = new Point(30, 30), AutoSize = true };
            txtQuestion = new TextBox { Location = new Point(120, 30), Width = 450 };

            // Type (disabled since we don't allow changing question type)
            var lblType = new Label { Text = "Type:", Location = new Point(30, 70), AutoSize = true };
            cmbType = new ComboBox
            {
                Location = new Point(120, 70),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            cmbType.Items.AddRange(new string[] { "MCQ", "MultiAnswer" });

            // Image path field
            var lblImage = new Label { Text = "Image Path:", Location = new Point(30, 110), AutoSize = true };
            txtImagePath = new TextBox { Location = new Point(120, 110), Width = 450 };

            this.Controls.Add(lblQuestion);
            this.Controls.Add(txtQuestion);
            this.Controls.Add(lblType);
            this.Controls.Add(cmbType);
            this.Controls.Add(lblImage);
            this.Controls.Add(txtImagePath);

            // Option inputs with corresponding radio/checkbox
            for (int i = 0; i < 4; i++)
            {
                var txtOption = new TextBox { Location = new Point(120, 150 + i * 40), Width = 300 };
                var chk = new CheckBox { Location = new Point(440, 150 + i * 40), Visible = false };
                var rb = new RadioButton { Location = new Point(440, 150 + i * 40), Visible = false };

                optionBoxes.Add(txtOption);
                checkBoxes.Add(chk);
                radioButtons.Add(rb);

                this.Controls.Add(txtOption);
                this.Controls.Add(chk);
                this.Controls.Add(rb);
            }

            // Save / Cancel buttons
            var btnSave = new Button { Text = "Update", Location = new Point(120, 320), Size = new Size(100, 30) };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(250, 320), Size = new Size(100, 30) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            // Load question details
            txtQuestion.Text = original.QuestionText;
            txtImagePath.Text = original.ImagePath;
            cmbType.SelectedItem = original is MCQQuestion ? "MCQ" : "MultiAnswer";

            bool isMulti = original is MultiAnswerQuestion;

            for (int i = 0; i < original.Options.Length; i++)
            {
                optionBoxes[i].Text = original.Options[i];

                if (isMulti)
                {
                    checkBoxes[i].Visible = true;
                    checkBoxes[i].Checked = ((MultiAnswerQuestion)original).CorrectAnswerIndexes.Contains(i);
                }
                else
                {
                    radioButtons[i].Visible = true;
                    radioButtons[i].Checked = ((MCQQuestion)original).CorrectAnswerIndex == i;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string text = txtQuestion.Text.Trim();
            string imagePath = txtImagePath.Text.Trim();
            string[] options = optionBoxes.Select(x => x.Text.Trim()).ToArray();

            // Validation
            if (string.IsNullOrWhiteSpace(text) || options.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Please complete the question and all options.");
                return;
            }

            // Update base fields
            original.QuestionText = text;
            original.ImagePath = imagePath;
            original.Options = options;

            if (original is MCQQuestion mcq)
            {
                int correct = radioButtons.FindIndex(rb => rb.Checked);
                if (correct == -1)
                {
                    MessageBox.Show("Please select a correct answer.");
                    return;
                }
                mcq.CorrectAnswerIndex = correct;
            }
            else if (original is MultiAnswerQuestion multi)
            {
                var correctIndexes = checkBoxes
                    .Select((chk, i) => new { chk.Checked, i })
                    .Where(x => x.Checked)
                    .Select(x => x.i)
                    .ToList();

                if (!correctIndexes.Any())
                {
                    MessageBox.Show("Please select at least one correct answer.");
                    return;
                }

                multi.CorrectAnswerIndexes = correctIndexes;
            }

            // Save to DB
            DbUtil.UpdateQuestion(original);
            MessageBox.Show("Question updated successfully.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
