using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class PlcRunner
    {
        private readonly PlcConfig _config;
        private readonly IPlcClient _client;
        private readonly CancellationToken _token;

        public PlcRunner(PlcConfig config, CancellationToken token)
        {
            _config = config;
            _client = PlcFactory.Create(config);
            _token = token;
        }

        public async Task RunAsync()
        {
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    await _client.ConnectAsync(_config);

                    while (!_token.IsCancellationRequested)
                    {


                        //await _client.PollAsync();   // read all tags
                        //await Task.Delay(_config.PollIntervalMs, _token);
                        //Testing 

                        TagResponse tagResponse = null;
                        TagRequest usn = new TagRequest();
                        usn.Address = "40001";
                        usn.Length = 10;

                        tagResponse = await _client.ReadAsync<TagResponse>(usn);

                        //    await plc.WriteAsync<TagResponse>(heartbit);
                        //    Console.WriteLine("DMC Number::{0}", tagResponse?.Value);
                        Thread.Sleep(4000);


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PLC {_config.PlcName} error: {ex.Message}");
                    await Task.Delay(3000, _token); // retry
                }
            }
        }
    }

}
