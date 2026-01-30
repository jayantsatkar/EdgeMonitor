using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public enum PlcMake
    {
        Siemens,
        AllenBradley,
        Mitsubishi
    }

    public enum ProtocolType
    {
        S7 = 1,
        EthernetIP = 2,
        ModbusTcp =3
    }

    public enum PlcDataType
    {
        Bool,        // Single bit (coil, discrete input)
        Int16,       // 16-bit signed integer
        UInt16,      // 16-bit unsigned integer
        Int32,       // 32-bit signed integer
        UInt32,      // 32-bit unsigned integer
        Float,       // IEEE 754 single precision
        Double,      // IEEE 754 double precision
        String,      // Text values (EtherNet/IP tags often support this)
        ByteArray    // Raw bytes (for custom parsing)
    }

    public enum ConnectionState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected =  2,
        Faulted = 3,
        Retrying = 4
    }
}
