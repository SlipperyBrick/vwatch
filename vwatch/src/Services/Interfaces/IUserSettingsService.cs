using System.Threading.Tasks;
using vwatch.Models;

namespace vwatch.Services.Interfaces
{
    public interface IUserSettingsService
    {
        Task SaveConfigurationAsync(string configuration);
        Task<string> LoadConfigurationAsync();
    }
}
