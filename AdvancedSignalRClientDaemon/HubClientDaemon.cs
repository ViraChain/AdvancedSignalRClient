using AdvancedSignalRClientDaemon.HubClients;
using Autofac;
using AutofacSerilogIntegration;

namespace AdvancedSignalRClientDaemon
{
    public class HubClientDaemon
    {
        private ContainerBuilder containerBuilder { get; }
        public HubClientDaemon()
        {
            containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterLogger();
            containerBuilder.RegisterType<HubClientBuilder>().AsSelf();
            containerBuilder.Build();
        }
        public HubClientDaemon(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
            this.containerBuilder.RegisterType<HubClientBuilder>().AsSelf();
        }

    }
}
