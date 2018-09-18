using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class MessageTemplate
    {
        public string MessageTemplateId { get; set; }
        public List<MessageAttribute> Attributes { get; set; }
    }

    public class MessageAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
