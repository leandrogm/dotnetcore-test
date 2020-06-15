using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet.UI.Settings
{
    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string ApiBaseUri { get; set; }
        public string From { get; set; }
        public string Domain { get; set; }
        public string RequestUri { get; set; }
        public string DefaultTo { get; set; }
    }
}
