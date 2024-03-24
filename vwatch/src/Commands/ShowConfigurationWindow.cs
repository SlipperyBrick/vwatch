using Microsoft.Extensions.DependencyInjection;

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

            var viewModel = new ConfigurationWindowViewModel(userSettingsService);
            //await viewModel.LoadConfigurationAsync();

            ConfigurationWindow window = new ConfigurationWindow
            {
                DataContext = viewModel
            };

            window.Show();
        }
    }
}
