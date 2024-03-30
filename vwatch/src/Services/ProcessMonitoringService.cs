using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using vwatch.Models;
using vwatch.Services.Interfaces;

namespace vwatch.Services
{
    public class ProcessMonitoringService : IProcessMonitoringService
    {
        private List<DataGridModel> _processes = new List<DataGridModel>();
        private CancellationTokenSource _cancellationTokenSource;

        public ProcessMonitoringService()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartProcessMonitoring()
        {
            Debug.WriteLine("Process monitoring started.");
            _ = Task.Run(() => MonitorProcessesAsync(_cancellationTokenSource.Token));
        }

        private async Task MonitorProcessesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var model in _processes)
                {
                    var processName = System.IO.Path.GetFileNameWithoutExtension(model.Filename);
                    var processes = Process.GetProcessesByName(processName);

                    if (processes.Length > 0)
                    {
                        Debug.WriteLine($"Process matched: {model.Filename}");
                        AttachDebuggerToProcess(model);
                    }
                }

                await Task.Delay(5000, cancellationToken);
            }
        }

        public void StopProcessMonitoring()
        {
            Debug.WriteLine("Process monitoring stopped.");
            _cancellationTokenSource.Cancel();
        }

        public void UpdateMonitoringList(IEnumerable<DataGridModel> processes)
        {
            _processes = processes.ToList();
        }

        private void AttachDebuggerToProcess(DataGridModel model)
        {
            // Implement debugger attachment logic based on model properties
            Debug.WriteLine($"Attaching to process: {model}");
            // Note: Actual attachment logic to be implemented as per your requirements.
        }
    }
}
