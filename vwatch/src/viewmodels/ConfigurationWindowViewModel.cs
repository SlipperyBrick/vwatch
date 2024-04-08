using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

using vwatch.Models;

namespace vwatch.ViewModels
{
    public partial class ConfigurationWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsExecutablesEmpty))]
        private ObservableCollection<Executable> executables;

        public bool IsExecutablesEmpty => Executables.Count == 0;

        public ConfigurationWindowViewModel()
        {
            executables = new ObservableCollection<Executable>();
        }

        [RelayCommand]
        public void AddExecutable()
        {
            Executables.Add(new Executable());
        }

        [RelayCommand]
        public void RemoveExecutable(Executable executable)
        {
            if (Executables.Contains(executable))
            {
                Executables.Remove(executable);
            }
        }
    }
}
