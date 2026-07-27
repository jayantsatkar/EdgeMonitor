using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public static class ProtocolDriverFactory
    {
        public static IProtocolDriver Create(PlcConfig config)
        {
            return config.Protocol switch
            {
                ProtocolType.ModbusTcp => new ModbusTcpDriver(config),
                //ProtocolType.S7 => new S7Driver(config),
                _ => throw new NotSupportedException(
                        $"Protocol {config.Protocol} not supported")
            };
        }
    }
}
