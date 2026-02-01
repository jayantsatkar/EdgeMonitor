
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace EdgeMonitor
{
    internal class Program
    {
        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); 
            IConfiguration config = builder.Build(); 
            
            // Access values
            string appName = config["AppSettings:AppName"]; 
            int refreshInterval = int.Parse(config["AppSettings:RefreshInterval"]); 
            string connStr = config.GetConnectionString("DefaultConnection"); 
            Console.WriteLine($"App: {appName}, Refresh: {refreshInterval}, Conn: {connStr}");
            var plcList = config.GetSection("PlcList").Get<List<PlcConfig>>();

            var plcWorkers = new List<PlcWorker>();


            //foreach (var plcConfig in plcList)
            //{
            //        var worker = new PlcWorker(plcConfig);
            //        plcWorkers.Add(worker);

            //        Task.Run(() => worker.RunAsync(serviceToken));
            //}


            var cfg = new PlcConfig
            {
                PlcMake = PlcMake.Mitsubishi,
                Protocol = ProtocolType.ModbusTcp,
                IpAddress = "192.168.0.105",
                Port = 502
            };

            //PlcWorker plcWorker = new PlcWorker(cfg);
            //var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            //using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            //Task plcTask = Task.Run(() => plcWorker.RunAsync(cts.Token));


            var plc = PlcFactory.Create(cfg);
            await plc.ConnectAsync(cfg);


            TagResponse tagResponse = null;
            TagRequest usn = new TagRequest();
            usn.Address = "40001";
            usn.Length = 10;


            TagRequest heartbit = new TagRequest();
            heartbit.Address = "3500";
            heartbit.Length = 1;
            heartbit.Value = true;


            while (true)
            {
                //int value = await plc.ReadAsync<int>("10");
                heartbit.Value = !(Boolean)heartbit.Value;

                tagResponse = await plc.ReadAsync<TagResponse>(usn);

                await plc.WriteAsync<TagResponse>(heartbit);
                Console.WriteLine("DMC Number::{0}", tagResponse?.Value);
                Thread.Sleep(4000);

            }

            Console.ReadLine();

        }

        //public async Task RunAsync(CancellationToken serviceToken)
        //{
        //    while (!serviceToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            State = ConnectionState.Connecting;

        //            await ConnectAsync(serviceToken);

        //            State = ConnectionState.Connected;
        //            LastSuccessfulConnection = DateTime.UtcNow;
        //            _retryAttempt = 0;

        //            await PollAsync(serviceToken);
        //        }
        //        catch (OperationCanceledException)
        //        {
        //            break; // service stopping
        //        }
        //        catch (Exception ex)
        //        {
        //            LastError = ex.Message;
        //            State = ConnectionState.Faulted;

        //            await DisconnectAsync();

        //            State = ConnectionState.Retrying;
        //            await Task.Delay(GetRetryDelay(), serviceToken);
        //        }
        //    }

        //}

    }
}
