using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class S7Protocol : IProtocol
    {
        public Task ConnectAsync(string ip, int port)
        {
            Console.WriteLine($"Connecting using S7 protocol to {ip}:{port}");
            return Task.CompletedTask;
        }



        public Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }

        public Task<T> ReadAsync<T>(TagRequest tagRequest)
        {
            Console.WriteLine($"S7 READ: {tagRequest.Address}");
            return Task.FromResult(default(T));
        }

        public Task WriteAsync<T>(TagRequest tagRequest)
        {
            Console.WriteLine($"S7 WRITE: {tagRequest.Address} = {tagRequest.Value}");
            return Task.CompletedTask;
        }
    }

}
