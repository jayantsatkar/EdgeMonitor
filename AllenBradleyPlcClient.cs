using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class AllenBradleyPlcClient : IPlcClient
    {
        private readonly IProtocol _protocol;
        private readonly PlcConfig _config;

        public bool IsConnected { get; private set; }

        public AllenBradleyPlcClient(PlcConfig config, IProtocol protocol)
        {
            _config = config;
            _protocol = protocol;
        }

        public async Task ConnectAsync(PlcConfig plcConfig)
        {
            await _protocol.ConnectAsync(_config.IpAddress, _config.Port);
            IsConnected = true;
        }

        public Task DisconnectAsync() => _protocol.DisconnectAsync();

        public Task<T> ReadAsync<T>(TagRequest tagRequest) => _protocol.ReadAsync<T>(tagRequest);
        public Task WriteAsync<T>(TagRequest tagRequest1) {
         //   _protocol.WriteAsync(tagRequest1);
         throw new NotImplementedException();
        }
    }

}
