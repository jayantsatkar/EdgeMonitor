using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public static class PlcFactory
    {
        public static IPlcClient Create(PlcConfig config)
        {
            IProtocol protocol = config.Protocol switch
            {
                ProtocolType.S7 => new S7Protocol(),
                ProtocolType.ModbusTcp => new ModbusProtocol(),
                ProtocolType.EthernetIP => new EthernetIpProtocol(),
                _ => throw new Exception("Unknown protocol")
            };

            return config.PlcMake switch
            {
                PlcMake.Siemens => new SiemensPlcClient(config, protocol),
                PlcMake.AllenBradley => new AllenBradleyPlcClient(config, protocol),
                PlcMake.Mitsubishi => new MitsubishiPlcClient(config, protocol),
                _ => throw new Exception("Unknown PLC type")
            };
        }
    }

}
