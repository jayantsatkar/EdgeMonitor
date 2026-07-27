using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public interface IProtocolDriver : IDisposable
    {
        Task ConnectAsync(CancellationToken token);
        Task DisconnectAsync();
        Task PollAsync(CancellationToken token);
        bool IsConnected { get; }
        string? LastError { get; }
    }
}
