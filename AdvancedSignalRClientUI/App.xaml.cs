using AdvancedSignalRClientDaemon;
using AdvancedSignalRClientDaemon.SerilogUtils;
using Autofac;
using AutofacSerilogIntegration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AdvancedSignalRClientUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            Log.Logger = ConfigureLogger.BuildLogger(new MessegeBoxSink());
            var containerBuilder = new ContainerBuilder();
            var daemon = new HubClientDaemon(containerBuilder);
            containerBuilder.RegisterLogger();
            containerBuilder.RegisterType<MainWindow>().AsSelf();
            containerBuilder.RegisterType<Receiver>().AsSelf();
            var container = containerBuilder.Build();

            await using var scope = container.BeginLifetimeScope();
            var logger = scope.Resolve<ILogger>();
            logger.Information("Starting Application");

            var mainWindow = scope.Resolve<MainWindow>();
            mainWindow.ShowDialog();

            logger.Information("Closing Application");
        }
    }
}
