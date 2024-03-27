using MessengerApp_Client.Client;
using MessengerApp_Client.Helpers;
using MessengerApp_Client.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerApp_Client.ViewModel
{
    public class MessengerWindowViewModel : ObservableObject
    {
        private readonly MessengerTcpClient _tcpClient;
        private ContactList _selectedContact;
        private ObservableCollection<ContactList> _contacts;
        private ObservableCollection<MessageHistory> _messages;
        private string _currentUserName;

        public ObservableCollection<ContactList> Contacts
        {
            get { return _contacts; }
            set { SetProperty(ref _contacts, value); }
        }

        public ObservableCollection<MessageHistory> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        public ContactList SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                if (SetProperty(ref _selectedContact, value))
                    LoadMessagesAsync(_selectedContact.Id);
            }
        }

        public RelayCommand LoadContactsCommand { get; private set; }

        public MessengerWindowViewModel(string currentUserName)
        {
            _tcpClient = new MessengerTcpClient();
            _currentUserName = currentUserName;
            LoadContactsAsync();
            LoadContactsCommand = new RelayCommand(async (_) => await LoadContactsAsync());
        }

        private async Task LoadContactsAsync()
        {
            try
            {
                var contacts = await _tcpClient.GetContactsAsync(_currentUserName);
                if (contacts != null)
                {
                    Contacts = new ObservableCollection<ContactList>(contacts);
                    OnPropertyChanged(nameof(Contacts));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error loading contacts", ex);
            }
        }

        private async Task LoadMessagesAsync(int contactId)
        {
            try
            {
                var messages = await _tcpClient.GetMessagesAsync(contactId);
                if (messages != null)
                {
                    Messages = new ObservableCollection<MessageHistory>(messages);
                    OnPropertyChanged(nameof(Messages));
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error loading messages", ex);
            }
        }

        private void ShowErrorMessage(string title, Exception ex)
        {
            MessageBox.Show($"{title}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
