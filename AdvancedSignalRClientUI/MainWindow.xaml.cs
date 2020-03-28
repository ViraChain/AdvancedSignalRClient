using AdvancedSignalRClientDaemon.HubClients;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                    Dispatcher.Invoke(() =>
                    {
                        switch (i)
                        {
                            case Statuses.Connected:
                                SendButton.IsEnabled = true;
                                ReceiveButton.IsEnabled = true;
                                status.Fill = Brushes.Green;
                                break;
                            case Statuses.Disconnected:
                                SendButton.IsEnabled = false;
                                ReceiveButton.IsEnabled = false;
                                status.Fill = Brushes.Red;
                                break;
                            case Statuses.Reconnecting:
                                SendButton.IsEnabled = false;
                                ReceiveButton.IsEnabled = false;
                                status.Fill = Brushes.Yellow;
                                break;
                            case Statuses.Connecting:
                                SendButton.IsEnabled = false;
                                ReceiveButton.IsEnabled = false;
                                status.Fill = Brushes.YellowGreen;
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
            Messages.Items.Clear();
            try
            {
                await foreach (var item in hubClient.RecieveMessages(Function.Text))
                {
                    Dispatcher.Invoke(() =>
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

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
