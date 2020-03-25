using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedSignalRClientDaemon.SerilogUtils
{
    public class HubEnricher<T> : Serilog.Core.ILogEventEnricher
    {
        private readonly string HubName;
        private readonly string URL;

        public HubEnricher(string hubName, string uRL)
        {
            HubName = hubName;
            URL = uRL;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("HubName", HubName));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ServerName", URL));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SourceContext", typeof(T).FullName));
        }
    }
}
