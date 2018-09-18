using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class SMS: ISMS
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string To { get; set; }
        public bool Sent { get; set; }
        public DateTime SentAt { get; set; }
        public string MessageID { get; set; }
        public MessageTemplate MessageTemplate { get; set; }
        public string ErrorMessage { get; set; }
        public CurrentMessageStatus MessageStatus { get; set; }
        public string CallBackID { get; set; }
    }
}
