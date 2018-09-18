using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class Category
    {
        public string name { get; set; }
        public int recipientCount { get; set; }
        public string percentageTotal { get; set; }
    }

    public partial class MessageStatus
    {
        public List<Link> link { get; set; }
        public List<Category> categories { get; set; }
    }

    public class MessageStatusResponse:IResponse
    {
        public List<MessageStatus> messageStatuses { get; set; }
        public List<object> link { get; set; }
    }

    public class Dlr
    {
        public string period { get; set; }
        public string rule { get; set; }
        public string type { get; set; }
        public bool publishToWeb { get; set; }
        public int expiryDay { get; set; }
        public int expiryHour { get; set; }
        public int expiryMin { get; set; }
        public string feedIds { get; set; }
        public bool @bool { get; set; }
    }

    public class Email
    {
        public string type { get; set; }
        public string body { get; set; }
        public string footer { get; set; }
    }

    public class Voice
    {
    }

    public class Social2
    {
        public string id { get; set; }
        public string body { get; set; }
    }

    public class Social
    {
        public List<Social2> social { get; set; }
    }

    public class Attribute
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Messageattributes
    {
        public List<Attribute> attribute { get; set; }
    }


    public class MessageSentResponse : IResponse
    {
        public string to { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public int repetitionCount { get; set; }
        public Dlr dlr { get; set; }
        public int repeatDays { get; set; }
        public int repeatHrs { get; set; }
        public int repeatMin { get; set; }
        public Email email { get; set; }
        public Voice voice { get; set; }
        public string from { get; set; }
        public string direction { get; set; }
        public string responseCount { get; set; }
        public Social social { get; set; }
        public long createdTime { get; set; }
        public Messageattributes messageattributes { get; set; }
        public List<Link> link { get; set; }
    }
}
