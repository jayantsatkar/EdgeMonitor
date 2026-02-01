using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class SiemensPlcClient : IPlcClient
    {
        private readonly IProtocol _protocol;
        private readonly PlcConfig _config;

        public bool IsConnected { get; private set; }

        public SiemensPlcClient(PlcConfig config, IProtocol protocol)
        {
            _config = config;
            _protocol = protocol;
        }

        public async Task ConnectAsync(PlcConfig plcConfig)
        {
            await _protocol.ConnectAsync(plcConfig);
            IsConnected = true;
        }

        public Task DisconnectAsync()
        {
            IsConnected = false;
            return _protocol.DisconnectAsync();
        }

        public Task<T> ReadAsync<T>(TagRequest tagRequest) => _protocol.ReadAsync<T>(tagRequest);
        public Task WriteAsync<T>(TagRequest tagRequest1)
        {
            throw new NotImplementedException();
        }
    }

}
