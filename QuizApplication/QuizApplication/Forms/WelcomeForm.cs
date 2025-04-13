using System;
using System.Windows.Forms;

namespace QuizApplication
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.ClientSize = new Size(500, 350);
        }

        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            string playerName = txtPlayerName.Text.Trim();

            if (string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Please enter your name!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MainForm mainForm = new MainForm(playerName);
            mainForm.Show();
            this.Hide();  // Hide the Welcome form
        }
    }
}
