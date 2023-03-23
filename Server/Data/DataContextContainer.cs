using System.Collections.Generic;

namespace Server.Data
{
    public class DataContextContainer
    {
        public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public List<Message> Messages { get; set; } = new List<Message>();
        public void AddMessages(IList<Message> messages)
        {
            Messages.AddRange(messages);
            if (Messages.Count > 50000)
            {
                Messages.RemoveRange(0, messages.Count);
            }
        }
    }
}
