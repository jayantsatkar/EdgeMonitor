
using Microsoft.Extensions.Configuration;

namespace EdgeMonitor
{
    internal class Program
    {
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

            var cfg = new PlcConfig
            {
                PlcMake = PlcMake.Mitsubishi,
                Protocol = ProtocolType.ModbusTcp,
                IpAddress = "192.168.0.105",
                Port = 502
            };

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



    }
}
