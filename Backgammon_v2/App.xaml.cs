using System.Configuration;
using System.Data;
using System.Windows;

namespace Backgammon_v2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            playerLogin1 = new PlayerLogin();
            playerLogin1.Closed += PlayerLogin1_Closed;
            

        }
        private PlayerLogin playerLogin1;
        private PlayerLogin2 playerLogin2;
        private MainWindow mainWindow;


        private void PlayerLogin1_Closed(object sender, System.EventArgs e)
        {
            playerLogin2 = new PlayerLogin2();
            playerLogin2.Closed += PlayerLogin2_Closed;
            
        }

        private void PlayerLogin2_Closed(object sender, System.EventArgs e)
        {
            mainWindow = new MainWindow();
            
        }
    }

}
