using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;

using vwatch.Models;
using vwatch.Services.Interfaces;

namespace vwatch.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly WritableSettingsStore _settingsStore;

        public UserSettingsService(WritableSettingsStore settingsStore)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _settingsStore = settingsStore;
        }

        public async Task SaveUserSettingsAsync(UserSettingsModel settings)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var settingsJson = JsonSerializer.Serialize(settings);
            if (!_settingsStore.CollectionExists("VWatchExtensionSettings"))
            {
                _settingsStore.CreateCollection("VWatchExtensionSettings");
            }
            _settingsStore.SetString("VWatchExtensionSettings", "UserSettings", settingsJson);
        }

        public async Task<UserSettingsModel> LoadUserSettingsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (_settingsStore.CollectionExists("VWatchExtensionSettings"))
            {
                var settingsJson = _settingsStore.GetString("VWatchExtensionSettings", "UserSettings", string.Empty);
                return JsonSerializer.Deserialize<UserSettingsModel>(settingsJson) ?? new UserSettingsModel();
            }

            return new UserSettingsModel();
        }
    }
}
