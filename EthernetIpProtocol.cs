using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class EthernetIpProtocol : IProtocol
    {
        public Task ConnectAsync(string ip, int port)
        {
            Console.WriteLine($"EtherNet/IP connect to {ip}:{port}");
            return Task.CompletedTask;
        }

        public Task DisconnectAsync() => Task.CompletedTask;

        public Task<T> ReadAsync<T>(TagRequest tagRequest)
        {
            Console.WriteLine($"EIP READ: {tagRequest.Address}");
            return Task.FromResult(default(T));
        }

        public Task WriteAsync<T>(TagRequest tagRequest)
        {
            Console.WriteLine($"EIP WRITE {tagRequest.Address} = {tagRequest.Value}");
            return Task.CompletedTask;
        }
    }


}
