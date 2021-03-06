﻿using Serilog;
using Serilog.Core;

namespace AdvancedSignalRClientDaemon.SerilogUtils
{
    public static class ConfigureLogger
    {
        public static ILogger BuildLogger()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Sink(new ListSink())
                .WriteTo.Console()
                .WriteTo.File("./logs.log")
                .CreateLogger();
            return logger;
        }
        public static ILogger BuildLogger(ILogEventSink customsink)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Sink(customsink)
                .WriteTo.Console()
                .WriteTo.File("./logs.log")
                .CreateLogger();
            return logger;
        }
    }
}
