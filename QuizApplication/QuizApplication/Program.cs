using System;
using System.Windows.Forms;
using QuizApplication.DB;
using QuizApplication.Forms;
using QuizApplication.Models;

namespace QuizApplication
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            DbUtil.init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                PlayerForm playerForm = new PlayerForm();
                if (playerForm.ShowDialog() != DialogResult.OK)
                    break;

                string playerName = playerForm.PlayerName;
                playerForm.Dispose();

                if (playerName == "Question")
                {
                    ManageForm manageForm = new ManageForm();
                    Application.Run(manageForm);
                    break;
                }

                MainForm mainForm = new MainForm(playerName);
                Application.Run(mainForm);
                bool restart = mainForm.RestartRequested;
                mainForm.Dispose();

                if (!restart)
                    break;
            }
        }
    }
}