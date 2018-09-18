using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
      public class messageattributes
    {
        public List<attribute> attribute { get; set; }
    }
    public class attribute
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
