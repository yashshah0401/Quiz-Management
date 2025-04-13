namespace QuizApplication
{
    partial class ResultForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnBoard;
        private System.Windows.Forms.Button btnReviewIncorrect;
        private System.Windows.Forms.Button btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblSummary = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnBoard = new System.Windows.Forms.Button();
            this.btnReviewIncorrect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSummary
            // 
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(50, 30);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(700, 400);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRestart
            // 
            this.btnRestart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRestart.Location = new System.Drawing.Point(50, 470);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(150, 50);
            this.btnRestart.Text = "Play Again";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            this.btnRestart.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnRestart.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            // 
            // btnBoard
            // 
            this.btnBoard.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBoard.Location = new System.Drawing.Point(220, 470);
            this.btnBoard.Name = "btnBoard";
            this.btnBoard.Size = new System.Drawing.Size(150, 50);
            this.btnBoard.Text = "Show Board";
            this.btnBoard.UseVisualStyleBackColor = true;
            this.btnBoard.Click += new System.EventHandler(this.btnBoard_Click);
            this.btnBoard.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnBoard.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            // 
            // btnReviewIncorrect
            // 
            this.btnReviewIncorrect.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReviewIncorrect.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnReviewIncorrect.Location = new System.Drawing.Point(390, 470);
            this.btnReviewIncorrect.Name = "btnReviewIncorrect";
            this.btnReviewIncorrect.Size = new System.Drawing.Size(180, 50);
            this.btnReviewIncorrect.Text = "Review";
            this.btnReviewIncorrect.UseVisualStyleBackColor = false;
            this.btnReviewIncorrect.Click += new System.EventHandler(this.btnReviewIncorrect_Click);
            this.btnReviewIncorrect.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnReviewIncorrect.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.Location = new System.Drawing.Point(590, 470);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(150, 50);
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnExit.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 560);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnBoard);
            this.Controls.Add(this.btnReviewIncorrect);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblSummary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quiz Result";
            this.Load += new System.EventHandler(this.ResultForm_Load);
            this.ResumeLayout(false);
        }
    }
}