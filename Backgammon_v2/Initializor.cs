using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon_v2
{
    internal class Initializor
    {
        public Initializor()
        {
            playerLogin1 = new PlayerLogin();
            playerLogin1.Closed += PlayerLogin1_Closed;
            playerLogin1.Show();

        }
        private PlayerLogin playerLogin1;
        private PlayerLogin2 playerLogin2;
        private MainWindow mainWindow;


        private void PlayerLogin1_Closed(object sender, System.EventArgs e)
        {
            playerLogin2 = new PlayerLogin2();
            playerLogin2.Closed += PlayerLogin2_Closed;
            playerLogin2.Show();
        }

        private void PlayerLogin2_Closed(object sender, System.EventArgs e)
        {
            mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
