using System.Collections.Generic;

namespace BulkSenderSMTP.Models
{
    public class MailModelCollections
    {      
        public IList<string> EmailList { get; } = new List<string>();
        public IList<string> MessageList { get; } = new List<string>();

    }
}
