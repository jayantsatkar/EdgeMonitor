using NModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class ModbusTcpDriver : IProtocolDriver
    {
        private readonly PlcConfig _config;

        private TcpClient? _tcpClient;
        //private NetworkStream? _stream;
        private IModbusMaster? _master;

        public bool IsConnected => _tcpClient?.Connected == true;
        public string? LastError { get; private set; }

        public ModbusTcpDriver(PlcConfig config)
        {
            _config = config;
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(_config.IpAddress, _config.Port, token);

               // _stream = _tcpClient.GetStream();

                var factory = new ModbusFactory();
                _master = factory.CreateMaster(_tcpClient);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                throw;
            }
        }

        public async Task PollAsync(CancellationToken token)
        {
            try
            {
                var data = _master!.ReadHoldingRegisters(
                    _config.SlaveId, 0, 10);

                // process data
                await Task.Delay(1000, token);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                throw;
            }
        }

        public Task DisconnectAsync()
        {
            _master?.Dispose();
           // _stream?.Dispose();
            _tcpClient?.Dispose();

            _master = null;
           // _stream = null;
            _tcpClient = null;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            DisconnectAsync().Wait();
        }
    }

}
