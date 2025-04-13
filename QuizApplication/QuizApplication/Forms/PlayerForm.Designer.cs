namespace QuizApplication
{
    partial class PlayerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblPrompt;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblPrompt = new System.Windows.Forms.Label();

            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.Text = "Welcome";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblPrompt.Text = "Enter your name:";
            lblPrompt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblPrompt.Location = new System.Drawing.Point(30, 20);
            lblPrompt.Size = new System.Drawing.Size(340, 25);

            txtName.Font = new System.Drawing.Font("Segoe UI", 12F);
            txtName.Location = new System.Drawing.Point(30, 60);
            txtName.Size = new System.Drawing.Size(340, 34);
            txtName.MaxLength = 20;
            txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            btnStart.Text = "Start Quiz";
            btnStart.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnStart.Location = new System.Drawing.Point(130, 100);
            btnStart.Size = new System.Drawing.Size(140, 40);
            btnStart.Click += new System.EventHandler(this.btnStart_Click);

            this.Controls.Add(lblPrompt);
            this.Controls.Add(txtName);
            this.Controls.Add(btnStart);
            this.ResumeLayout(false);
        }
    }
}
