using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class Link
    {
        public string uri { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

    public class Message
    {
        public string subject { get; set; }
        public int repetitionCount { get; set; }
        public int repeatDays { get; set; }
        public int repeatHrs { get; set; }
        public int repeatMin { get; set; }
        public string from { get; set; }
        public string direction { get; set; }
        public string responseCount { get; set; }
        public object createdTime { get; set; }
        public List<Link> link { get; set; }
    }

    public class SMSMessageResponse: IResponse
    {
        public List<Message> messages { get; set; }
        public string status { get; set; }
        public List<object> link { get; set; }
    }
}
