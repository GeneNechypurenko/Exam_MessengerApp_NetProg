using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp_Client.Model
{
    public class MessageHistory
    {
        public int ContactId { get; set; }
        public string Message { get; set; }
        public override string ToString() => string.Format("{0}", Message);
    }
}