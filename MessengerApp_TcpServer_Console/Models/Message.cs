using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp_TcpServer_Console.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int RecipientId { get; set; }
        public User Recipient { get; set; }
    }
}