namespace QuizApplication
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblQuestion = new Label();
            lblTimer = new Label();
            pictureBox = new PictureBox();
            pnlOptions = new Panel();
            btnSubmit = new Button();
            lblFeedback = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // lblQuestion
            // 
            lblQuestion.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblQuestion.Location = new Point(20, 20);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new Size(700, 50);
            lblQuestion.TabIndex = 0;
            lblQuestion.Text = "Question text here";
            lblQuestion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTimer
            // 
            lblTimer.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTimer.ForeColor = Color.Maroon;
            lblTimer.Location = new Point(720, 20);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(150, 30);
            lblTimer.TabIndex = 1;
            lblTimer.Text = "Time: 60";
            lblTimer.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(40, 90);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(300, 200);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            pictureBox.Visible = false;
            // 
            // pnlOptions
            // 
            pnlOptions.AutoScroll = true;
            pnlOptions.BackColor = Color.WhiteSmoke;
            pnlOptions.BorderStyle = BorderStyle.FixedSingle;
            pnlOptions.Location = new Point(360, 90);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new Size(500, 200);
            pnlOptions.TabIndex = 3;
            // 
            // btnSubmit
            // 
            btnSubmit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSubmit.Location = new Point(370, 310);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(160, 40);
            btnSubmit.TabIndex = 4;
            btnSubmit.Text = "Submit";
            btnSubmit.Click += btnSubmit_Click;
            // 
            // lblFeedback
            // 
            lblFeedback.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            lblFeedback.ForeColor = Color.MediumBlue;
            lblFeedback.Location = new Point(20, 370);
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Size = new Size(850, 30);
            lblFeedback.TabIndex = 5;
            lblFeedback.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(895, 427);
            Controls.Add(lblQuestion);
            Controls.Add(lblTimer);
            Controls.Add(pictureBox);
            Controls.Add(pnlOptions);
            Controls.Add(btnSubmit);
            Controls.Add(lblFeedback);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quiz Application";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblQuestion;
        private Label lblTimer;
        private PictureBox pictureBox;
        private Panel pnlOptions;
        private Button btnSubmit;
        private Label lblFeedback;
    }
}
