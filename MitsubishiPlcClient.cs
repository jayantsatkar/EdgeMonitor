using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class MitsubishiPlcClient : IPlcClient
    {
        private readonly IProtocol _protocol;
        private readonly PlcConfig _config;

        public MitsubishiPlcClient(PlcConfig config, IProtocol protocol)
        {
            _config = config;
            _protocol = protocol;
        }

        public bool IsConnected { get; private set; }

        public async Task ConnectAsync(PlcConfig plcConfig)
        {
            await _protocol.ConnectAsync(plcConfig);
            IsConnected = true;
            Console.WriteLine("Connection Status:{0}", IsConnected);
        }

        public Task DisconnectAsync() => _protocol.DisconnectAsync();

        public Task<T> ReadAsync<T>(TagRequest tagRequest) => _protocol.ReadAsync<T>(tagRequest);
        public Task WriteAsync<T>(TagRequest tagRequest)
        {
           return _protocol.WriteAsync<T>(tagRequest);
        }
    }

}
