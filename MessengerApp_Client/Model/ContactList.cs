using System.Collections.Generic;

namespace MessengerApp_Client.Model
{
    public class ContactList
    {
        public int Id { get; set; }
        public string ContactName { get; set; }
        public List<MessageHistory> Messages { get; set; }
        public override string ToString() => string.Format("{0}", ContactName);
    }
}