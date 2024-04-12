using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

using vwatch.mvm.Models;

namespace vwatch.mvm.ViewModels
{
    public partial class ConfigurationWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsExecutablesEmpty))]
        private ObservableCollection<Executable> executables;

        [ObservableProperty]
        private Executable selectedExecutable;

        public bool IsExecutablesEmpty => Executables.Count == 0;

        public bool CanRemove => !IsExecutablesEmpty;

        public event EventHandler RequestClose;

        public ConfigurationWindowViewModel()
        {
            executables = new ObservableCollection<Executable>();
        }

        [RelayCommand]
        public void AddExecutable()
        {
            Executables.Add(new Executable());
        }

        [RelayCommand(CanExecute = nameof(CanRemove))]
        public void RemoveExecutable(Executable executable)
        {
            if (Executables == null || !Executables.Contains(executable))
            {
                if (Executables.Any())
                {
                    Executables?.RemoveAt(Executables.Count - 1);
                }
            }
            else
            {
                Executables.Remove(executable);
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
