using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public static  class ContentTypes
    {
        public const string MessageContent = "application/vnd.whispir.message-v1+json";
        public const string MessageStatusContent = "application/vnd.whispir.messagestatus-v1+json";
        public const string WorkSpaceContent = "application/vnd.whispir.workspace-v1+json";
        public const string MessageResponseContent = "application/vnd.whispir.messageresponse-v1+json";
        public const string TemplateResponseContent = "application/vnd.whispir.template-v1+json";
        public const string CallBacksResponseContent = "application/vnd.whispir.api-callback-v1+json";
        public const string CallsResponseContent = "application/vnd.whispir.api-call-v1+json";
        
    }
}
