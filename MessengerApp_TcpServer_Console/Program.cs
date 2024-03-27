using MessengerApp_TcpServer_Console.Data;
using MessengerApp_TcpServer_Console.Server;
using System.Net;

namespace MessengerApp_TcpServer_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var contextFactory = new ApplicationContextFactory();
            var context = contextFactory.CreateDbContext(args);

            MessengerTcpServer server = new MessengerTcpServer(context);

            server.StartServerAsync().GetAwaiter().GetResult();
        }
    }
}