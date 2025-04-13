using QuizApplication.DB;
using QuizApplication.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuizApplication.Forms
{
    public partial class AddQuestionForm : Form
    {
        private TextBox txtQuestion;
        private ComboBox cmbType;
        private TextBox txtImagePath;
        private List<TextBox> optionBoxes = new();
        private List<CheckBox> checkBoxes = new();
        private List<RadioButton> radioButtons = new();

        public AddQuestionForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Add New Question";
            this.Size = new Size(650, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            // Question label and textbox
            var lblQuestion = new Label { Text = "Question:", Location = new Point(30, 30), AutoSize = true };
            txtQuestion = new TextBox { Location = new Point(120, 30), Width = 450 };

            // Question type dropdown
            var lblType = new Label { Text = "Type:", Location = new Point(30, 70), AutoSize = true };
            cmbType = new ComboBox
            {
                Location = new Point(120, 70),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(new string[] { "MCQ", "MultiAnswer" });
            cmbType.SelectedIndex = 0;

            // Image path label and textbox
            var lblImage = new Label { Text = "Image Path:", Location = new Point(30, 110), AutoSize = true };
            txtImagePath = new TextBox { Location = new Point(120, 110), Width = 450 };

            this.Controls.Add(lblQuestion);
            this.Controls.Add(txtQuestion);
            this.Controls.Add(lblType);
            this.Controls.Add(cmbType);
            this.Controls.Add(lblImage);
            this.Controls.Add(txtImagePath);

            // Four options with either checkbox or radiobutton beside each
            for (int i = 0; i < 4; i++)
            {
                var txtOption = new TextBox { Location = new Point(120, 150 + i * 40), Width = 300 };
                var chk = new CheckBox { Location = new Point(440, 150 + i * 40), Visible = false };
                var rb = new RadioButton { Location = new Point(440, 150 + i * 40), Visible = true };

                optionBoxes.Add(txtOption);
                checkBoxes.Add(chk);
                radioButtons.Add(rb);

                this.Controls.Add(txtOption);
                this.Controls.Add(chk);
                this.Controls.Add(rb);
            }

            // Toggle input style when question type changes
            cmbType.SelectedIndexChanged += (s, e) =>
            {
                bool isMulti = cmbType.SelectedItem.ToString() == "MultiAnswer";
                for (int i = 0; i < 4; i++)
                {
                    checkBoxes[i].Visible = isMulti;
                    radioButtons[i].Visible = !isMulti;
                    checkBoxes[i].Checked = false;
                    radioButtons[i].Checked = false;
                }
            };

            // Save and cancel buttons
            var btnSave = new Button { Text = "Save", Location = new Point(120, 320), Size = new Size(100, 30) };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(250, 320), Size = new Size(100, 30) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string questionText = txtQuestion.Text.Trim();
            string type = cmbType.SelectedItem.ToString();
            string imagePath = txtImagePath.Text.Trim();
            var options = optionBoxes.Select(x => x.Text.Trim()).ToArray();

            // Validate question and options
            if (string.IsNullOrWhiteSpace(questionText) || options.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Please fill in the question and all four options.");
                return;
            }

            Question question;

            if (type == "MCQ")
            {
                // Find the selected radio button index
                int correctIndex = radioButtons.FindIndex(rb => rb.Checked);
                if (correctIndex == -1)
                {
                    MessageBox.Show("Please select one correct answer.");
                    return;
                }

                question = new MCQQuestion
                {
                    QuestionText = questionText,
                    Options = options,
                    CorrectAnswerIndex = correctIndex,
                    ImagePath = imagePath
                };
            }
            else
            {
                // Find all selected checkboxes
                var correctIndexes = checkBoxes
                    .Select((chk, i) => new { chk.Checked, Index = i })
                    .Where(x => x.Checked)
                    .Select(x => x.Index)
                    .ToList();

                if (correctIndexes.Count == 0)
                {
                    MessageBox.Show("Please select at least one correct answer.");
                    return;
                }

                question = new MultiAnswerQuestion
                {
                    QuestionText = questionText,
                    Options = options,
                    CorrectAnswerIndexes = correctIndexes,
                    ImagePath = imagePath
                };
            }

            // Save the question to the database
            DbUtil.AddQuestion(question);
            MessageBox.Show("Question saved successfully.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
