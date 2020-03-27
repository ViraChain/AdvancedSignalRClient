using AdvancedSignalRClientDaemon.SerilogUtils;
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
using System.Windows.Shapes;

namespace AdvancedSignalRClientUI
{
    /// <summary>
    /// Interaction logic for LogWinodw.xaml
    /// </summary>
    public partial class LogWinodw : Window
    {
        public LogWinodw()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in ListSink.Loggs)
            {

            }
        }
    }
}
