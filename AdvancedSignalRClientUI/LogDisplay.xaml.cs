using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdvancedSignalRClientUI
{
    /// <summary>
    /// Interaction logic for LogDisplay.xaml
    /// </summary>
    public partial class LogDisplay : UserControl
    {
        public LogDisplay(LogEvent log)
        {
            InitializeComponent();
        }
    }
}
