using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeMonitor
{
    public class PlcManager
    {
        private readonly Dictionary<string, CancellationTokenSource> _plcTokens = new();
        private readonly Dictionary<string, Task> _plcTasks = new();

        public void StartPlc(PlcConfig config)
        {
            if (_plcTasks.ContainsKey(config.PlcName))//Id can be used 
                return;

            var cts = new CancellationTokenSource();
            var runner = new PlcRunner(config, cts.Token);

            _plcTokens[config.PlcName] = cts; ///Id can be used 
            _plcTasks[config.PlcName] = Task.Run(() => runner.RunAsync()); /// Id can be used 
        }

        public async Task StopPlcAsync(string plcId)
        {
            if (!_plcTokens.TryGetValue(plcId, out var cts))
                return;

            cts.Cancel();

            try
            {
                await _plcTasks[plcId];
            }
            catch { /* ignore */ }

            _plcTokens.Remove(plcId);
            _plcTasks.Remove(plcId);
        }
    }

}
