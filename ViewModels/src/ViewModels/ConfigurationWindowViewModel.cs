using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

using vwatch.Models;

namespace vwatch.ViewModels
{
    public partial class ConfigurationWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool enabled = true;

        [ObservableProperty]
        private ObservableCollection<Executable> executables = new ObservableCollection<Executable>();

        [ObservableProperty]
        private Executable? selectedExecutable;

        public bool IsExecutablesEmpty => Executables.Count == 0;
        public bool CanRemove => !IsExecutablesEmpty;

        public event EventHandler? RequestClose;

        [RelayCommand]
        public void AddExecutable()
        {
            Executables.Add(new Executable());

            UpdateExecutablesState();
        }

        [RelayCommand]
        public void RemoveExecutable()
        {
            if (SelectedExecutable == null && Executables.Count > 0)
            {
                Executables.RemoveAt(Executables.Count - 1);
            }
            else if (SelectedExecutable != null)
            {
                Executables.Remove(SelectedExecutable);
            }

            UpdateExecutablesState();
        }

        [RelayCommand]
        public void Cancel()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateExecutablesState()
        {
            OnPropertyChanged(nameof(IsExecutablesEmpty));
            OnPropertyChanged(nameof(CanRemove));
        }
    }
}