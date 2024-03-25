using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

using vwatch.Models;
using vwatch.Services.Interfaces;

namespace vwatch.ViewModels
{
    public class ConfigurationWindowViewModel : ObservableObject
    {
        public AsyncRelayCommand SaveConfigurationCommand { get; }
        public AsyncRelayCommand LoadConfigurationCommand { get; }

        public DataGridViewModel DataGridViewModel { get; set; }

        private readonly IUserSettingsService _userSettingsService;

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
                DataGridItems = DataGridViewModel.Items
            };

            await _userSettingsService.SaveUserSettingsAsync(settings);
        }

        public async Task LoadConfigurationAsync()
        {
            var userSettings = await _userSettingsService.LoadUserSettingsAsync();
            DataGridViewModel.UpdateItems(userSettings.DataGridItems);

            // Assuming there's a way to update DataGridViewModel based on settings
            // DataGridViewModel.SomeProperty = settings.SomeSetting;

            // Notify the UI that the DataGridViewModel has been updated (if needed)
            OnPropertyChanged(nameof(DataGridViewModel));
        }
    }
}
