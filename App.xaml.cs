using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FireTestingApp_net8
{
    public partial class App : Application
    {
        private readonly Bootstrapper _bootstrapper = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _bootstrapper.ServiceProvider.GetRequiredService<MainWindow>();
            base.OnStartup(e);
            MainWindow.Show();
        }
    }
}
