using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class Callback
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool retriesEnabled { get; set; }
        public List<Link> link { get; set; }
    }

    public class CallBacksResponse : IResponse
    {
        public string status { get; set; }
        public List<Callback> callbacks { get; set; }
        public List<object> link { get; set; }
    }


    public class Calls_From
    {
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string voice { get; set; }
    }

    public class Calls_ResponseMessage
    {
        public string channel { get; set; }
        public string undeliverable { get; set; }
        public string content { get; set; }
    }

    public class Calls_Callback
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime attemptedDate { get; set; }
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
    }

    public class Calls_Link
    {
        public string uri { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
        public string type { get; set; }
    }

    public class Call
    {
        public string id { get; set; }
        public string messageId { get; set; }
        public string status { get; set; }
        public string messageLocation { get; set; }
        public Calls_From from { get; set; }
        public Calls_ResponseMessage responseMessage { get; set; }
        public Calls_Callback callback { get; set; }
        public List<Calls_Link> link { get; set; }
    }

    public class CallsResponse : IResponse
    {
        public List<Call> calls { get; set; }
        public string status { get; set; }
        public List<object> link { get; set; }
    }
}
