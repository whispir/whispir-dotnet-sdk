using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class Messagetemplate
    {
        public string messageTemplateName { get; set; }
        public string messageTemplateDescription { get; set; }
        public string id { get; set; }
        public List<Link> link { get; set; }
    }

    public class TemplatesResponse : IResponse
    {
        public string status { get; set; }
        public List<Messagetemplate> messagetemplates { get; set; }
        public List<object> link { get; set; }
    }
}
