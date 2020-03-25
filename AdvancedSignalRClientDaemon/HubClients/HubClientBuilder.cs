using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSignalRClientDaemon.HubClients
{
    public class HubClientBuilder : IAsyncDisposable
    {
        private readonly Dictionary<string, HubClient> hubClients;
        private readonly ILogger logger;

        public HubClientBuilder(ILogger logger)
        {
            this.logger = logger;
            hubClients = new Dictionary<string, HubClient>();
        }

        public HubClient CreateClient(string hubName, string uRL)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(uRL)
                .Build();
            var hubClient = new HubClient(connection, logger, uRL, hubName);
            hubClients.Add(hubName, hubClient);
            return hubClient;
        }
        public HubClient GetHubClient(string hubName)
        {
            hubClients.TryGetValue(hubName, out var hubClient);
            return hubClient;
        }
        public async Task DisposeHubClientAsync(string hubName)
        {
            hubClients.TryGetValue(hubName, out var hubClient);
            await hubClient.DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var item in hubClients)
            {
                await item.Value.DisposeAsync();
            }
            hubClients.Clear();
        }
    }
}
