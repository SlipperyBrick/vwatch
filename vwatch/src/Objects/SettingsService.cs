using Microsoft.VisualStudio.Settings;
using System.Threading.Tasks;

namespace vwatch.Objects
{
    public class SettingsService
    {
        private readonly WritableSettingsStore _settingsStore;

        public SettingsService(WritableSettingsStore settingsStore)
        {
            _settingsStore = settingsStore;
        }

        public Task SaveSettingsAsync(string settingsJson)
        {
            return Task.Run(() =>
            {
                _settingsStore.SetString("VWatchExtensionSettings", "Configuration", settingsJson);
            });
        }

        public Task<string> LoadSettingsAsync()
        {
            return Task.Run(() =>
            {
                return _settingsStore.GetString("VWatchExtensionSettings", "Configuration", string.Empty);
            });
        }
    }
}
