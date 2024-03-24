using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

using vwatch.Models;
using vwatch.Services.Interfaces;
using System.Configuration;
using System.Diagnostics;

namespace vwatch.ViewModels
{
    internal class ConfigurationWindowViewModel : ObservableObject
    {
        public AsyncRelayCommand SaveConfigurationCommand { get; }

        public DataGridViewModel DataGridViewModel { get; set; }

        private readonly IUserSettingsService _userSettingsService;

        public ConfigurationWindowViewModel(IUserSettingsService userSettingsService)
        {
            SaveConfigurationCommand = new AsyncRelayCommand(async () => await SaveConfigurationAsync());

            DataGridViewModel = new DataGridViewModel();

            _userSettingsService = userSettingsService;
        }

        public string SerializeConfiguration()
        {
            var settings = new
            {
                DataGridItems = DataGridViewModel.Items,
            };

            return JsonConvert.SerializeObject(settings);
        }

        public void DeserializeConfiguration(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                var settings = JsonConvert.DeserializeObject<ConfigurationSettings>(json);

                /*if (settings?.DataGridItems != null)
                {
                    DataGridViewModel.Items = settings.DataGridItems;
                }*/
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Failed to deserialize configuration: {ex.Message}");
            }
        }


        private async Task SaveConfigurationAsync()
        {
            string configurationJson = SerializeConfiguration();
            await _userSettingsService.SaveConfigurationAsync(configurationJson);
        }

        public async Task LoadConfigurationAsync()
        {
            string configurationJson = await _userSettingsService.LoadConfigurationAsync();
            DeserializeConfiguration(configurationJson);
        }
    }
}
