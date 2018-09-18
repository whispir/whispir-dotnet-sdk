using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class From
    {
        public string name { get; set; }
        public string mri { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string voice { get; set; }
    }

    public class ResponseMessage
    {
        public string content { get; set; }
        public string acknowledged { get; set; }
        public string channel { get; set; }
    }

    public class Messagerespons
    {
        public From from { get; set; }
        public string responseCategory { get; set; }
        public ResponseMessage responseMessage { get; set; }
    }

    public class MessageResponseResponse : IResponse
    {
        public string status { get; set; }
        public List<Messagerespons> messageresponses { get; set; }
        public List<object> link { get; set; }
    }
}
