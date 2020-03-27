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
        public bool IsOpen { get; set; } = false;
        public MainWindow(HubClientBuilder hubClientBuilder)
        {
            InitializeComponent();
            this.hubClientBuilder = hubClientBuilder;
        }

        private async void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOpen)
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
                SendButton.IsEnabled = true;
                ReceiveButton.IsEnabled = true;
            }
            else
            {
                SendButton.IsEnabled = false;
                ReceiveButton.IsEnabled = false;
                await hubClientBuilder.DisposeHubClientAsync("Test");
                status.Fill = Brushes.Gray;
                (sender as Button).Content = "Start Connection";
            }
            IsOpen = !IsOpen;
        }

        private async void Receive_Button_Click(object sender, RoutedEventArgs e)
        {
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

        private async void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            await hubClient.SendAsync(sfName.Text, message.Text);
        }
    }
}
