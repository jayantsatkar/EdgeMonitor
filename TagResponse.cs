using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class TagResponse
    {
        public string Address { get; set; }          // Echo back the tag/register that was read
        public PlcDataType DataType { get; set; }    // The type of the value returned
        public object Value { get; set; }            // The actual data read (boxed for flexibility)
        public bool Success { get; set; }            // Indicates if the read was successful
        public string ErrorMessage { get; set; }     // Optional error info if Success = false
        public DateTime Timestamp { get; set; }      // When the value was read
    }
}
