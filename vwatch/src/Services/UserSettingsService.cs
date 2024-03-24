using System.Threading.Tasks;
using Microsoft.VisualStudio.Settings;

using vwatch.Services.Interfaces;

namespace vwatch.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly WritableSettingsStore _settingsStore;

        public async Task SaveConfigurationAsync(string configurationJson)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (!_settingsStore.CollectionExists("VWatchExtensionSettings"))
            {
                _settingsStore.CreateCollection("VWatchExtensionSettings");
            }

            _settingsStore.SetString("VWatchExtensionSettings", "Configuration", configurationJson);
        }

        public async Task<string> LoadConfigurationAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (_settingsStore.CollectionExists("VWatchExtensionSettings"))
            {
                return _settingsStore.GetString("VWatchExtensionSettings", "Configuration", defaultValue: "");
            }

            return "";
        }

    }
}
