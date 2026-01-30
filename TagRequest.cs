using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EdgeMonitor
{
    public class TagRequest
    {
        public string Address { get; set; } = "";

        public PlcDataType DataType { get; set; } // Expected type

        public ushort Length { get; set; } = 1;

        public object Value { get; set; }
    }

    

}
