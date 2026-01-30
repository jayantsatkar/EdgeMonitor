using NModbus;
using NModbus.Extensions.Enron;
using System.Net.Sockets;

namespace EdgeMonitor
{
    public class ModbusProtocol : IProtocol
    {
        private TcpClient? _client;
        private NetworkStream? _stream;
        private IModbusMaster? _master;
        private ushort _transactionId = 1;
        public async Task ConnectAsync(string ip, int port)
        {
            //Console.WriteLine($"Modbus TCP connect to {ip}:{port}");
            //return Task.CompletedTask;

            _client = new TcpClient();

            Console.WriteLine($"Connecting Modbus TCP to {ip}:{port} ...");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            try
            {
                await _client.ConnectAsync(ip, port, cts.Token);
                _stream = _client.GetStream();
                var factory = new ModbusFactory();
                IModbusMaster _master = factory.CreateMaster(_client);
                Console.WriteLine("Modbus Connected.");
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException("Modbus connection timed out.");
            }
        }

        public Task DisconnectAsync()
        {
            _stream?.Close();
            _client?.Close();
            _stream = null;
            _client = null;

            Console.WriteLine("Modbus disconnected.");
            return Task.CompletedTask;
        }

        public async Task<T> ReadAsync<T>(TagRequest tagRequest)
        {
            object result = null;
            try
            {
              
                if (_stream == null)
                    throw new Exception("Modbus not connected.");

                Console.WriteLine($"Modbus READ: {tagRequest.Address}");
                var factory = new ModbusFactory();
                IModbusMaster _master = factory.CreateMaster(_client);

                ushort[] registers = _master.ReadHoldingRegisters(1, Convert.ToUInt16(tagRequest.Address), tagRequest.Length);
                Console.WriteLine("Holding Registers:");
                char[] chars = new char[registers.Length];
                for (int i = 0; i < registers.Length; i++)
                { chars[i] = (char)registers[i]; }
                string DMC = new string(chars);
               

                TagResponse tagResponse = new TagResponse();
                tagResponse.Address = tagRequest.Address;
                tagResponse.Timestamp = DateTime.Now;
                tagResponse.Value = DMC;

                result = Convert.ChangeType(tagResponse, typeof(T));
              
            }

            catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);

                Console.WriteLine("Disconnecting PLC");
                DisconnectAsync();
                Thread.Sleep(5000);
                ConnectAsync("192.168.0.105", 502);


            }
            return (T)result;
        }

        public Task WriteAsync<T>(TagRequest tagRequest)
        {
            try
            {
                Console.WriteLine($"Modbus WRITE {tagRequest.Address} = {tagRequest.Value}");
                var factory = new ModbusFactory();
                IModbusMaster _master = factory.CreateMaster(_client);

                _master.WriteSingleCoilAsync(1, Convert.ToUInt16(tagRequest.Address), (Boolean)tagRequest.Value);
                ushort[] registers = _master.ReadHoldingRegisters(1, Convert.ToUInt16(tagRequest.Address), tagRequest.Length);
            }

            catch (Exception ex) 
            {
                Console.WriteLine(ex.StackTrace);

                Console.WriteLine("Disconnecting PLC");
                DisconnectAsync();
                Thread.Sleep(5000);
                ConnectAsync("192.168.0.105",502);
            }

            return Task.CompletedTask;
        }

        private (byte function, ushort start) ParseAddress(string address)
        {
            int addr = int.Parse(address);

            if (addr >= 40000)
                return (3, (ushort)(addr - 40001));
            if (addr >= 30000)
                return (4, (ushort)(addr - 30001));
            if (addr >= 10000)
                return (2, (ushort)(addr - 10001));
            else
                return (1, (ushort)(addr - 1));
        }

        private byte[] BuildReadFrame(byte function, ushort start, ushort quantity)
        {
            byte[] frame = new byte[12];

            frame[0] = (byte)(_transactionId >> 8);
            frame[1] = (byte)(_transactionId & 0xFF);
            _transactionId++;

            frame[2] = 0; // protocol high
            frame[3] = 0; // protocol low
            frame[4] = 0; // length high
            frame[5] = 6; // length low
            frame[6] = 1; // unit id
            frame[7] = function;

            frame[8] = (byte)(start >> 8);
            frame[9] = (byte)(start & 0xFF);

            frame[10] = (byte)(quantity >> 8);
            frame[11] = (byte)(quantity & 0xFF);

            return frame;
        }
    }

}
