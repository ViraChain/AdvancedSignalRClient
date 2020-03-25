using AdvancedSignalRClientDaemon.HubClients;
using Autofac;
using AutofacSerilogIntegration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedSignalRClientDaemon
{
    public class HubClientDaemon
    {
        private ContainerBuilder containerBuilder { get; }
        public HubClientDaemon()
        {
            this.containerBuilder = new ContainerBuilder();
            this.containerBuilder.RegisterLogger();
            this.containerBuilder.RegisterType<HubClientBuilder>().AsSelf();
            this.containerBuilder.Build();
        }
        public HubClientDaemon(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
            this.containerBuilder.RegisterType<HubClientBuilder>().AsSelf();
        }

    }
}
