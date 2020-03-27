using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedSignalRClientDaemon.SerilogUtils
{
    public class ListSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        public static List<LogEvent> Loggs = new List<LogEvent>();
        public ListSink(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            Loggs.Add(logEvent);
        }
    }
}
