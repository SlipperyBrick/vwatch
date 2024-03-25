using System.Collections.ObjectModel;

namespace vwatch.Models
{
    public class UserSettingsModel
    {
        public ObservableCollection<DataGridModel> DataGridItems { get; set; } = new ObservableCollection<DataGridModel>();
    }
}
