using MessengerApp_Client.Client;
using System.Windows;

namespace MessengerApp_Client.View
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }
        private async void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            string username = accountTextBox.Text;
            string password = passwordTextBox.Text;

            MessengerTcpClient tcpClient = new MessengerTcpClient();
            bool success = await tcpClient.RegisterAsync(username, password);

            if (success)
            {
                MessageBox.Show("Registration successful!");
                MessengerWindow messengerWindow = new MessengerWindow(username);
                messengerWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed. Please try again.");
            }
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}