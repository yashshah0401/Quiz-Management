using Microsoft.VisualBasic.ApplicationServices;
using QuizApplication.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizApplication.Forms
{
    public partial class BoardForm : Form
    {
        public BoardForm()
        {
            InitializeComponent();

            // Build score table dynamically
            var table = BuildScoreTable(DbUtil.GetAllUserScores());
            Controls.Add(table);

            // Close button
            var btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(160, 40),
                Location = new Point(150, table.Bottom + 20)
            };
            btnClose.Click += (s, e) => this.Close();
            Controls.Add(btnClose);

            // Adjust form size
            this.ClientSize = new Size(460, btnClose.Bottom + 40);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Score Board";
        }

        private TableLayoutPanel BuildScoreTable(List<Models.UserScore> users)
        {
            return BuildScoreTable(users, SortType.Score);
        }

        private TableLayoutPanel BuildScoreTable(List<Models.UserScore> users, SortType sortType)
        {
            var table = new TableLayoutPanel
            {
                ColumnCount = 3,
                Location = new Point(30, 20),
                Size = new Size(400, 36 * (users.Count + 1)),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                AutoScroll = true
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            table.RowCount = users.Count + 1;
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            Label CreateHeader(string text, Action onClick)
            {
                var lbl = new Label
                {
                    Text = text,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand,
                    Padding = new Padding(4),
                    BackColor = Color.LightGray
                };

                lbl.Click += (s, e) =>
                {
                    onClick();
                };

                return lbl;
            }

            // Header
            if (sortType == SortType.Name)
            {
                table.Controls.Add(new Label
                {
                    Text = "Name",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                }, 0, 0);
            } 
            else
            {
                table.Controls.Add(CreateHeader("Name", () =>
                {
                    RefreshBoard(SortType.Name);
                }), 0, 0);
            }
            if (sortType == SortType.Score)
            {
                table.Controls.Add(new Label
                {
                    Text = "Score",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                }, 1, 0);

            }
            else
            {
                table.Controls.Add(CreateHeader("Score", () =>
                {
                    RefreshBoard(SortType.Score);
                }), 1, 0);
            }
            if (sortType == SortType.Time)
            {
                table.Controls.Add(new Label
                {
                    Text = "Time",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                }, 2, 0);
            }
            else
            {
                table.Controls.Add(CreateHeader("Time", () =>
                {
                    RefreshBoard(SortType.Time);
                }), 2, 0);
            }

            // Sort users by type
            switch(sortType)
            {
                case SortType.Name:
                    users = users.OrderBy(u => u.Username).ToList();
                    break;
                case SortType.Score:
                    users.Sort();
                    break;
                case SortType.Time:
                    users = users.OrderBy(u => u.AnswerTime).ToList();
                    break;
            }

            // Data
            for (int i = 0; i < users.Count; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                table.Controls.Add(new Label
                {
                    Text = users[i].Username,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(4),
                    AutoSize = false
                }, 0, i + 1);

                table.Controls.Add(new Label
                {
                    Text = users[i].Score.ToString("0.00") + "%",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(4),
                    AutoSize = false
                }, 1, i + 1);

                table.Controls.Add(new Label
                {
                    Text = users[i].AnswerTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(4),
                    AutoSize = false
                }, 2, i + 1);
            }

            return table;
        }

        private void RefreshBoard(SortType sortType)
        {
            foreach (Control ctrl in this.Controls.OfType<TableLayoutPanel>().ToList())
            {
                this.Controls.Remove(ctrl);
                ctrl.Dispose();
            }
            var users = DbUtil.GetAllUserScores();
            var table = BuildScoreTable(users, sortType);
            Controls.Add(table);

            var btn = this.Controls.OfType<Button>().FirstOrDefault();
            if (btn != null)
            {
                btn.Top = table.Bottom + 20;
                this.ClientSize = new Size(460, btn.Bottom + 40);
            }
        }
    }

    enum SortType
    {
        Name,
        Score,
        Time
    }
}