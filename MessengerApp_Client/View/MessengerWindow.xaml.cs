using System.Windows;
using MessengerApp_Client.ViewModel;

namespace MessengerApp_Client.View
{
    public partial class MessengerWindow : Window
    {
        public MessengerWindow(string currentUserName)
        {
            InitializeComponent();
            DataContext = new MessengerWindowViewModel(currentUserName);
        }
    }
}