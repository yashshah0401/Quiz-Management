namespace QuizApplication
{
    partial class WelcomeForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblNamePrompt = new System.Windows.Forms.Label();
            this.btnStartQuiz = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // txtPlayerName
            this.txtPlayerName.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtPlayerName.Location = new System.Drawing.Point(100, 120);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(300, 32);
            this.txtPlayerName.TabIndex = 0;
            this.txtPlayerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPlayerName.BorderStyle = BorderStyle.FixedSingle;
            this.txtPlayerName.BackColor = Color.LightGray;
            this.txtPlayerName.ForeColor = Color.Black;

            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = Color.DarkSlateBlue;
            this.lblWelcome.Location = new System.Drawing.Point(150, 30);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(200, 45);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome!";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblNamePrompt
            this.lblNamePrompt.AutoSize = true;
            this.lblNamePrompt.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblNamePrompt.ForeColor = Color.Black;
            this.lblNamePrompt.Location = new System.Drawing.Point(100, 100);
            this.lblNamePrompt.Name = "lblNamePrompt";
            this.lblNamePrompt.Size = new System.Drawing.Size(180, 21);
            this.lblNamePrompt.TabIndex = 2;
            this.lblNamePrompt.Text = "Enter your name below:";

            // btnStartQuiz
            this.btnStartQuiz.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnStartQuiz.ForeColor = Color.White;
            this.btnStartQuiz.BackColor = Color.MediumSlateBlue;
            this.btnStartQuiz.Location = new System.Drawing.Point(180, 180);
            this.btnStartQuiz.Name = "btnStartQuiz";
            this.btnStartQuiz.Size = new System.Drawing.Size(140, 45);
            this.btnStartQuiz.TabIndex = 3;
            this.btnStartQuiz.Text = "Start Quiz";
            this.btnStartQuiz.UseVisualStyleBackColor = false;
            this.btnStartQuiz.Click += new System.EventHandler(this.btnStartQuiz_Click);
            this.btnStartQuiz.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnStartQuiz.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);

            // WelcomeForm
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.btnStartQuiz);
            this.Controls.Add(this.lblNamePrompt);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.txtPlayerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quiz Application";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblNamePrompt;
        private System.Windows.Forms.Button btnStartQuiz;

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.MediumOrchid;
                btn.ForeColor = Color.White;
            }
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.MediumSlateBlue;
                btn.ForeColor = Color.White;
            }
        }
    }
}
