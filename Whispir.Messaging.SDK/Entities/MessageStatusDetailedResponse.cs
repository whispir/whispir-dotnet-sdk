using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{

    public class Status
    {
        public string type { get; set; }
        public string status { get; set; }
        public string destination { get; set; }
        public long? sentTimestamp { get; set; }
        public long? receivedTimestamp { get; set; }
    }

    public partial class MessageStatus
    {
        public string name { get; set; }
        public string info { get; set; }
        public List<Status> status { get; set; }
    }

    public class MessageStatusDetailedResponse:IResponse
    {
        public List<MessageStatus> messageStatuses { get; set; }
        public string status { get; set; }
        public List<object> link { get; set; }
    }
}
