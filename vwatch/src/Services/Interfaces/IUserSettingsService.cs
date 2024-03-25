using System.Threading.Tasks;

using vwatch.Models;

namespace vwatch.Services.Interfaces
{
    public interface IUserSettingsService
    {
        Task SaveUserSettingsAsync(UserSettingsModel settings);
        Task<UserSettingsModel> LoadUserSettingsAsync();
    }
}
