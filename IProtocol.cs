using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public interface IProtocol : IDisposable // IProtocolDriver
    {
        Task ConnectAsync(PlcConfig plcConfig);
        Task DisconnectAsync();
        Task<T> ReadAsync<T>(TagRequest tagRequest);
        Task WriteAsync<T>(TagRequest tagRequest);

        //bool IsConnected { get; }
        //string? LastError { get; }
    }
}
