global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;

using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

using vwatch.ViewModels;
using vwatch.Services.Interfaces;
using vwatch.Services;

namespace vwatch
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.vwatchString)]
    public sealed class vwatchPackage : ToolkitPackage
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            await this.RegisterCommandsAsync();

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserSettingsService, UserSettingsService>();
            services.AddSingleton<ConfigurationWindowViewModel>();

            services.AddSingleton<WritableSettingsStore>(provider =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var shellSettingsManager = new ShellSettingsManager(this);
                return shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            });
        }

        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }
    }
}