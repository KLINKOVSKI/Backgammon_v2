using System;
using System.Windows;

namespace Backgammon_v2
{
    public partial class PlayerLogin2 : Window
    {
        public PlayerLogin2()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Check if the username and password are valid
            mariaDB db = new mariaDB();
            bool isAuthenticated = db.AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                MessageBox.Show("Login successful!");

                // Close the login window
                Close();

                // Show the game screen (MainWindow)
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

    }
}
