using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public interface IProtocol
    {
        Task ConnectAsync(string ip, int port);
        Task DisconnectAsync();
        Task<T> ReadAsync<T>(TagRequest tagRequest);
        Task WriteAsync<T>(TagRequest tagRequest);
    }
}
