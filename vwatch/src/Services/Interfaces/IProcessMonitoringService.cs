using System.Collections.Generic;
using vwatch.Models;

namespace vwatch.Services.Interfaces
{
    public interface IProcessMonitoringService
    {
        void StartProcessMonitoring();
        void StopProcessMonitoring();
        public void UpdateMonitoringList(IEnumerable<DataGridModel> processes);
    }
}
