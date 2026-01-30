using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EdgeMonitor.Program;

namespace EdgeMonitor
{
    public class PlcConfig
    {
        public string PlcName { get; set; } = "";
        public PlcMake PlcMake { get; set; }
        public ProtocolType Protocol { get; set; }
        public string IpAddress { get; set; } = "";
        public int Port { get; set; }
    }
}
