using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedSignalRClientDaemon.SerilogUtils
{
    public static class ConfigureLogger
    {
        public static ILogger BuildLogger()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("/logs.txt")
                .CreateLogger();
            Log.Logger = logger;
            return logger;
        }
    }
}
