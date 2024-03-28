using Community.VisualStudio.Toolkit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using System.Threading.Tasks;

using vwatch.Controls;
using vwatch.Services.Interfaces;
using vwatch.ViewModels;

namespace vwatch
{
    [Command(PackageIds.ShowConfigurationWindow)]
    internal sealed class ShowConfigurationWindow : BaseCommand<ShowConfigurationWindow>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var userSettingsService = vwatchPackage.ServiceProvider.GetService<IUserSettingsService>();
            var processMonitoringService = vwatchPackage.ServiceProvider.GetService<IProcessMonitoringService>();

            var viewModel = new ConfigurationWindowViewModel(userSettingsService, processMonitoringService);
            await viewModel.LoadConfigurationAsync();

            ConfigurationWindow window = new ConfigurationWindow
            {
                DataContext = viewModel
            };

            window.Show();
        }
    }
}
