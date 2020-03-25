using AdvancedSignalRClientDaemon.HubClients;
using Autofac;
using AutofacSerilogIntegration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedSignalRClientDaemon
{
    public class Startup
    {
        public ContainerBuilder containerBuilder { get; }
        public Startup()
        {
            containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterLogger();
            containerBuilder.RegisterType<HubClientBuilder>().AsSelf();
        }
        public IContainer BuildContainer()
            => containerBuilder.Build();

    }
}
