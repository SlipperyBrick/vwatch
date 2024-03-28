using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

using vwatch.Models;
using vwatch.Services.Interfaces;

namespace vwatch.Services
{
    public class ProcessMonitoringService : IProcessMonitoringService
    {
        private List<DataGridModel> _processes = new List<DataGridModel>();

        public void StartProcessMonitoring()
        {
            // Implement process monitoring logic here
            Debug.WriteLine("Process monitoring started.");

            // Example using ManagementEventWatcher (needs to run on a separate thread/task)
            var startWatchQuery = new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace");
            var startWatch = new ManagementEventWatcher(startWatchQuery);
            startWatch.EventArrived += (sender, args) => ProcessStarted(args);

            startWatch.Start();
        }

        private void ProcessStarted(EventArrivedEventArgs args)
        {
            string startedProcessName = args.NewEvent.Properties["ProcessName"].Value.ToString();

            var matchingModel = _processes.FirstOrDefault(model => model.Filename == startedProcessName);
            if (matchingModel != null)
            {
                // Match found - attach debugger based on the model's properties
                Debug.WriteLine($"Process matched: {startedProcessName}");
                AttachDebuggerToProcess(matchingModel);
            }
        }

        public void StopProcessMonitoring()
        {
            // Implement logic to stop monitoring here
            Debug.WriteLine("Process monitoring stopped.");
        }

        public void UpdateMonitoringList(IEnumerable<DataGridModel> processes)
        {
            _processes = processes.ToList();
        }

        private void AttachDebuggerToProcess(DataGridModel model)
        {
            // Implement debugger attachment logic based on model properties
            Debug.WriteLine($"Attaching to process: {model}");
        }
    }
}
