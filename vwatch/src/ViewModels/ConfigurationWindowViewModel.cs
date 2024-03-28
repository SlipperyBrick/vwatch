using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using vwatch.Models;
using vwatch.Services.Interfaces;

namespace vwatch.ViewModels
{
    public class ConfigurationWindowViewModel : ObservableObject
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IProcessMonitoringService _processMonitoringService;

        public AsyncRelayCommand SaveConfigurationCommand { get; }
        public AsyncRelayCommand LoadConfigurationCommand { get; }

        public DataGridViewModel DataGridViewModel { get; set; }

        private bool _checkboxState;
        public bool CheckboxState
        {
            get => _checkboxState;
            set
            {
                if (SetProperty(ref _checkboxState, value))
                {
                    if (_checkboxState)
                    {
                        _processMonitoringService.StartProcessMonitoring();
                    }
                    else
                    {
                        _processMonitoringService.StopProcessMonitoring();
                    }
                }
            }
        }

        public ConfigurationWindowViewModel(IUserSettingsService userSettingsService, IProcessMonitoringService processMonitoringService)
        {
            SaveConfigurationCommand = new AsyncRelayCommand(async () => await SaveConfigurationAsync());
            LoadConfigurationCommand = new AsyncRelayCommand(async () => await LoadConfigurationAsync());

            DataGridViewModel = new DataGridViewModel();

            _userSettingsService = userSettingsService;
            _processMonitoringService = processMonitoringService;

            DataGridViewModel.Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (DataGridModel item in e.OldItems)
                {
                    // Unsubscribe from PropertyChanged event
                    item.PropertyChanged -= PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (DataGridModel item in e.NewItems)
                {
                    // Subscribe to PropertyChanged event
                    item.PropertyChanged += PropertyChanged;
                }
            }

            // Update the monitoring service with the new list
            _processMonitoringService.UpdateMonitoringList(DataGridViewModel.Items);
        }

        private new void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Item property changed; update the monitoring list accordingly
            Debug.WriteLine($"Item property changed: {e.PropertyName}");
            _processMonitoringService.UpdateMonitoringList(DataGridViewModel.Items);
        }

        private async Task SaveConfigurationAsync()
        {
            var settings = new UserSettingsModel
            {
                DataGridItems = DataGridViewModel.Items,
                CheckboxState = this.CheckboxState
            };

            await _userSettingsService.SaveUserSettingsAsync(settings);
        }

        public async Task LoadConfigurationAsync()
        {
            var userSettings = await _userSettingsService.LoadUserSettingsAsync();
            DataGridViewModel.UpdateItems(userSettings.DataGridItems);
            CheckboxState = userSettings.CheckboxState;

            OnPropertyChanged(nameof(DataGridViewModel));
            OnPropertyChanged(nameof(CheckboxState));
        }
    }
}
