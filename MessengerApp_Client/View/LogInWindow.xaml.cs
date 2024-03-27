using MessengerApp_Client.Client;
using System.Windows;

namespace MessengerApp_Client.View
{
    public partial class LogInWindow : Window
    {
        private MessengerTcpClient client;
        public LogInWindow()
        {
            InitializeComponent();
            client = new MessengerTcpClient();
        }
        private async void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            string username = accountTextBox.Text;
            string password = passwordTextBox.Text;

            bool isAuthenticated = await client.AuthenticateAsync(username, password);

            if (isAuthenticated)
            {
                MessengerWindow messengerWindow = new MessengerWindow(username);
                messengerWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
