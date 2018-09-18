using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
   

        public class Workspace
        {
            public string id { get; set; }
            public string projectName { get; set; }
            public string projectNumber { get; set; }
            public string status { get; set; }
            public string billingcostcentre { get; set; }
            public List<Link> link { get; set; }
        }

        public class WorkSpaceResponse : IResponse
        {
            public List<Workspace> workspaces { get; set; }
            public string status { get; set; }
            public List<object> link { get; set; }
        }

}
