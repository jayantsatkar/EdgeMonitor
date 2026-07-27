using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class PlcWorker
    {
        private readonly IProtocolDriver _driver;
        private int _retryAttempt = 0;
        public PlcWorker(PlcConfig config)
        {
            _driver = ProtocolDriverFactory.Create(config);
        }
        public async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await _driver.ConnectAsync(token);
                    _retryAttempt = 0;
                    while (!token.IsCancellationRequested && _driver.IsConnected)
                    {
                        await _driver.PollAsync(token);
                    }
                }
                catch (Exception ex)
                {
                    await _driver.DisconnectAsync();
                    await Task.Delay(GetRetryDelay(), token);
                }
            }
        }

        private TimeSpan GetRetryDelay()
        {
            _retryAttempt++;
            return TimeSpan.FromSeconds(Math.Min(60, Math.Pow(2, _retryAttempt)));
        }
    }
}
