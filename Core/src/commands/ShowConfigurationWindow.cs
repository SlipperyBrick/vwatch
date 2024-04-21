using vwatch.ViewModels;

namespace vwatch
{
    [Command(PackageIds.ShowConfigurationWindow)]
    internal sealed class ShowConfigurationWindow : BaseCommand<ShowConfigurationWindow>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            ConfigurationWindow window = new ConfigurationWindow(new ConfigurationWindowViewModel());
            window.Show();
        }
    }
}