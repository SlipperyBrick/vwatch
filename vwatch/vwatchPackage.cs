using Community.VisualStudio.Toolkit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Settings;
using System;
using System.Runtime.InteropServices;
using System.Threading;

using Task = System.Threading.Tasks.Task;

using vwatch.Services;
using vwatch.Services.Interfaces;
using vwatch.ViewModels;

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
            await this.RegisterCommandsAsync();
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ConfigurationWindowViewModel>();

            services.AddSingleton(provider =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var shellSettingsManager = new ShellSettingsManager(this);
                return shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            });

            services.AddSingleton<IUserSettingsService, UserSettingsService>(provider =>
                new UserSettingsService(provider.GetRequiredService<WritableSettingsStore>()));

            services.AddSingleton(provider =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return (EnvDTE80.DTE2)GetGlobalService(typeof(EnvDTE.DTE));
            });

            services.AddSingleton<IProcessMonitoringService, ProcessMonitoringService>();
        }

        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
