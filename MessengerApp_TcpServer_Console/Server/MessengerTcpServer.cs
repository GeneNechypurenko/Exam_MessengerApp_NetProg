using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MessengerApp_TcpServer_Console.Data;
using MessengerApp_TcpServer_Console.Models;

namespace MessengerApp_TcpServer_Console.Server
{
    public class MessengerTcpServer
    {
        private readonly int port = 12345;
        private readonly IPAddress ipAddress = IPAddress.Loopback;
        private TcpListener listener;
        private readonly UserRepository _userRepository;

        public MessengerTcpServer(ApplicationContext context)
        {
            _userRepository = new UserRepository(context);
        }

        public async Task StartServerAsync()
        {
            try
            {
                listener = new TcpListener(ipAddress, port);
                listener.Start();
                Console.WriteLine("Server started. Waiting for connections...");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {dataReceived}");

                    string[] parts = dataReceived.Split('|');
                    if (parts.Length >= 3)
                    {
                        if (parts[0] == "REGISTER")
                        {
                            string name = parts[1];
                            string password = parts[2];

                            bool success = await _userRepository.AddUserAsync(name, password);

                            string response = success ? "SUCCESS" : "FAILURE";
                            byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
                            await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                        }
                        else if (parts[0] == "AUTHENTICATE")
                        {
                            string name = parts[1];
                            string password = parts[2];

                            bool isAuthenticated = await _userRepository.AuthenticateAsync(name, password);

                            string response = isAuthenticated ? "SUCCESS" : "FAILURE";
                            byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
                            await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                        }
                        else if (parts[0] == "GET_CONTACTS")
                        {
                            string username = parts[1];
                            string contactsJson = await _userRepository.GetContactsJson(username);
                            Console.WriteLine(contactsJson);
                            byte[] responseBuffer = Encoding.UTF8.GetBytes(contactsJson);
                            await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                        }
                        else
                        {
                            byte[] responseBuffer = Encoding.UTF8.GetBytes("INVALID FORMAT");
                            await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                        }
                    }
                }

                Console.WriteLine($"Client disconnected: {client.Client.RemoteEndPoint}");
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}