using MessengerApp_Client.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerApp_Client.Client
{
    public class MessengerTcpClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private readonly IPAddress ipAddress = IPAddress.Loopback;
        private readonly int port = 12345;

        public MessengerTcpClient()
        {
            client = new TcpClient();
        }
        private async Task<string> SendRequestAsync(string message)
        {
            try
            {
                if (!client.Connected)
                    await client.ConnectAsync(ipAddress, port);

                stream = client.GetStream();

                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);

                byte[] responseBuffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

                MessageBox.Show($"Response from server: {response}"); // только для отладки

                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending request: {ex.Message}"); // только для отладки
                return null;
            }
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            string message = $"AUTHENTICATE|{username}|{password}";
            string response = await SendRequestAsync(message);
            return response == "SUCCESS";
        }

        public async Task<bool> RegisterAsync(string name, string password)
        {
            string message = $"REGISTER|{name}|{password}";
            string response = await SendRequestAsync(message);
            return response == "SUCCESS";
        }
        public async Task<List<ContactList>> GetContactsAsync(string username)
        {
            string message = $"GET_CONTACTS|{username}";
            string response = await SendRequestAsync(message);
            MessageBox.Show($"Response from server: {response}");
            return response != null ? DeserializeContacts(response) : null;
        }

        public async Task<List<MessageHistory>> GetMessagesAsync(int contactId)
        {
            string message = $"GET_MESSAGES|{contactId}";
            string response = await SendRequestAsync(message);
            return response != null ? DeserializeMessages(response) : null;
        }
        public List<ContactList> DeserializeContacts(string responseData)
        {
            List<ContactList> contacts = new List<ContactList>();

            try
            {
                JArray jsonArray = JArray.Parse(responseData);

                foreach (JObject item in jsonArray)
                {
                    int contactId = item["Id"].ToObject<int>();
                    string contactName = item["ContactName"].ToString();
                    JArray messagesArray = (JArray)item["Messages"];

                    List<MessageHistory> messages = new List<MessageHistory>();
                    foreach (var messageItem in messagesArray)
                    {
                        string messageText = messageItem["Message"].ToString();
                        MessageHistory message = new MessageHistory
                        {
                            Message = messageText
                        };
                        messages.Add(message);
                    }

                    ContactList contact = new ContactList
                    {
                        Id = contactId,
                        ContactName = contactName,
                        Messages = messages
                    };

                    contacts.Add(contact);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deserializing contacts: {ex.Message}"); // только для отладки
            }

            return contacts;
        }
        public List<MessageHistory> DeserializeMessages(string responseData)
        {
            List<MessageHistory> messages = new List<MessageHistory>();

            try
            {
                JArray jsonArray = JArray.Parse(responseData);

                foreach (JObject item in jsonArray)
                {
                    string messageText = item["Message"].ToString();

                    MessageHistory message = new MessageHistory
                    {
                        Message = messageText
                    };

                    messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deserializing messages: {ex.Message}"); // только для отладки
            }

            return messages;
        }
    }
}