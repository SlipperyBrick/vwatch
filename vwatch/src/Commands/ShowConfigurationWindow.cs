using vwatch.Controls;

namespace vwatch
{
    [Command(PackageIds.ShowConfigurationWindow)]
    internal sealed class ShowConfigurationWindow : BaseCommand<ShowConfigurationWindow>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            ConfigurationWindow window = new ConfigurationWindow();
            window.Show();
        }
    }
}
