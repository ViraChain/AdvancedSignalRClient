using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AdvancedSignalRClientUI
{
    public class MessegeBoxSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;

        public MessegeBoxSink(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);
            MessageBox.Show(message);
        }
    }
}
