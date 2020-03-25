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
        public MainWindow(HubClientBuilder hubClientBuilder)
        {
            InitializeComponent();
            this.hubClientBuilder = hubClientBuilder;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            hubClient = hubClientBuilder.CreateClient("Test", URL.Text);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await foreach (var item in hubClient.RecieveMessages(Function.Text))
            {
                Messages.Items.Add(item);
            }
        }
    }
}
