using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class DBMessage
    {
        public string ID {get;set;}
        public DateTime TimeStamp { get; set; }
        public string MessageType { get; set; }
        public string ErrorMessage { get; set; }
        public string MessageStatus { get; set; }
    }

}
