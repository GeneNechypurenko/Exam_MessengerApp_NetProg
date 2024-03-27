using System.Windows;

namespace MessengerApp_Client.View
{
    public partial class StartupWindow : Window
    {
        LogInWindow logInWindow;
        RegistrationWindow registrationWindow;

        public StartupWindow()
        {
            InitializeComponent();
            logInWindow = new LogInWindow();
            registrationWindow = new RegistrationWindow();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            logInWindow.Show();
            this.Close();
        }

        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {
            registrationWindow.Show();
            this.Close();
        }
    }
}