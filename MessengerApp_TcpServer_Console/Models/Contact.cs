using System.Collections.Generic;

namespace MessengerApp_TcpServer_Console.Models
{
    public class Contact
    {
        public int UserId { get; set; }
        public int ContactUserId { get; set; }
        public User User { get; set; }
        public User ContactUser { get; set; }
        public List<Message> Messages { get; set; }
    }
}
