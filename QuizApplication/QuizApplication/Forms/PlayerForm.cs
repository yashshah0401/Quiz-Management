using System;
using System.Windows.Forms;

namespace QuizApplication
{
    public partial class PlayerForm : Form
    {
        public string PlayerName { get; private set; }

        public PlayerForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                PlayerName = txtName.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter your name.");
            }
        }
    }
}
