using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class WhispirSettings
    {
        public string ApiAuthorization { get;  set; }
        public string ApiBaseUrl { get;  set; }

        public string ApiKey { get;  set; }
        public string LoggingFolder { get;  set; }
        public string DataBaseFolder { get; set; }
        public int LoggingHours { get; set; }

    }
}
