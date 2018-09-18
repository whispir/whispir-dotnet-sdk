using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public enum CurrentMessageStatus
    {
        Sent,
        Pending,
        Received,
        Acknowledged,
        Undeliverable
    }
}
