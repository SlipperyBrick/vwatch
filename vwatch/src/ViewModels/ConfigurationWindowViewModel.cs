using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

using vwatch.Models;
using vwatch.Services.Interfaces;

namespace vwatch.ViewModels
{
    public class ConfigurationWindowViewModel : ObservableObject
    {
        private readonly IUserSettingsService _userSettingsService;

        public AsyncRelayCommand SaveConfigurationCommand { get; }
        public AsyncRelayCommand LoadConfigurationCommand { get; }

        public DataGridViewModel DataGridViewModel { get; set; }

        private bool _checkboxState;
        public bool CheckboxState
        {
            get => _checkboxState;
            set => SetProperty(ref _checkboxState, value);
        }

        public ConfigurationWindowViewModel(IUserSettingsService userSettingsService)
        {
            SaveConfigurationCommand = new AsyncRelayCommand(async () => await SaveConfigurationAsync());
            LoadConfigurationCommand = new AsyncRelayCommand(async () => await LoadConfigurationAsync());

            DataGridViewModel = new DataGridViewModel();

            _userSettingsService = userSettingsService;
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
