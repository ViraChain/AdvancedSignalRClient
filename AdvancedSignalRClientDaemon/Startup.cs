using Autofac;
using AutofacSerilogIntegration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedSignalRClientDaemon
{
    public class Startup
    {
        public ContainerBuilder container { get; }
        public Startup()
        {
            container = new ContainerBuilder();
            container.RegisterLogger();
        }
        public IContainer BuildContainer()
            => container.Build();

    }
}
