using AdvancedSignalRClientDaemon.HubClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HubClientBuilder hubClientBuilder;
        private HubClient hubClient;
        private bool isOpen = false;
        public MainWindow(HubClientBuilder hubClientBuilder)
        {
            InitializeComponent();
            this.hubClientBuilder = hubClientBuilder;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isOpen)
            {
                hubClient = hubClientBuilder.CreateClient("Test", URL.Text);
                hubClient.OnStatusChanged += (i) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        switch (i)
                        {
                            case Statuses.Connected:
                                status.Fill = Brushes.Green;
                                break;
                            case Statuses.Disconnected:
                                status.Fill = Brushes.Red;
                                break;
                            case Statuses.Reconnecting:
                                status.Fill = Brushes.Yellow;
                                break;
                            default:
                                break;
                        }
                    });
                };
                await hubClient.StartAsync();
                (sender as Button).Content = "Close Connection";
            }
            else
            {
                await hubClientBuilder.DisposeHubClientAsync("Test");
                status.Fill = Brushes.Gray;
                (sender as Button).Content = "Start Connection";
            }
            isOpen = !isOpen;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await hubClient.SendAsync("GetMarketData", "101");
            try
            {
                await foreach (var item in hubClient.RecieveMessages(Function.Text))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Messages.Items.Add(item);
                    });
                }
            }
            catch
            {

            }
        }
    }
}
