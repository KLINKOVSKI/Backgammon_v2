using System;
using System.Windows;

namespace Backgammon_v2
{
    public partial class PlayerLogin : Window
    {
        public PlayerLogin()
        {
            InitializeComponent();
        }


        // In your PlayerLogin.xaml.cs file

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Check if the username and password are valid
            mariaDB db = new mariaDB();
            db.Connect();
            bool isAuthenticated = db.AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                MessageBox.Show("Login successful!");

                // Close the login window
                Close();

                // Show PlayerLogin2 window
                PlayerLogin2 playerLogin2 = new PlayerLogin2();
                playerLogin2.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }


    }
}
