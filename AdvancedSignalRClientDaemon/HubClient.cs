using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedSignalRClientDaemon
{
    public interface IHubClient
    {
        Task StartAsync();
        Task StopAsync();
        Task<string> SendAsync(string methodName, string message);
        Task<string> SendAsync(string methodName, params string[] messages);
        Task<string> SendAsync(string methodName, object message);
        Task<string> SendAsync(string methodName, params object[] messages);
    }
    public class HubClient : IHubClient, IAsyncDisposable
    {
        private readonly HubConnection hubConnection;
        private readonly string URL;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly ILogger logger;

        public HubClient(HubConnection hubConnection, ILogger logger, string uRL)
        {
            cancellationTokenSource = new CancellationTokenSource();
            URL = uRL;
            this.hubConnection = hubConnection;
            this.logger = logger;

        }

        private void Init()
        {
            hubConnection.Closed += ex =>
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                    logger.Error(ex, "The connection to the {uRL} has been intrupted.", URL);
                else if (hubConnection.State == HubConnectionState.Reconnecting)
                    logger.Warning("Trying to restablish the connection to the {uRL}.", URL);
                return Task.CompletedTask;
            };

            hubConnection.Reconnecting += _ =>
            {
                logger.Warning("Trying to restablish the connection to the {uRL}.", URL);
                return Task.CompletedTask;
            };

            hubConnection.Reconnected += connectionId =>
            {
                logger.Information("New connection has been stablished to the {uRL} with connection ID {connectionId}.", URL, connectionId);
                return Task.CompletedTask;
            };
        }
        public async ValueTask DisposeAsync()
        {
            cancellationTokenSource.Dispose();
            await hubConnection?.DisposeAsync();
        }

        public async Task StartAsync()
        {
            try
            {
                await hubConnection.StartAsync(cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unhandled exception has occurred while trying to start the connection to {uRL}.", URL);
            }
        }
        public async Task StopAsync()
        {
            try
            {
                cancellationTokenSource.Cancel();
                await hubConnection.StopAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unhandled exception has occurred while trying to close the connection to {uRL}.", URL);
            }
        }

        public async Task<string> SendAsync(string methodName, string message)
        {
            _ = methodName ?? throw new ArgumentNullException("Argument methodName cant be null");
            _ = message ?? throw new ArgumentNullException("Argument message cant be null");
            try
            {
                await hubConnection.InvokeAsync(methodName, message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unhandled exception has occurred while trying to send message to {methodName}", methodName);
            }
            return default;
        }
        public async Task<string> SendAsync(string methodName, params string[] messages)
        {
            _ = methodName ?? throw new ArgumentNullException("Argument methodName cant be null");
            _ = messages ?? throw new ArgumentNullException("Argument messages cant be null");
            if (messages.Length > 10) throw new ArgumentException("You cant send more than 10 arguments.");
            try
            {

                switch (messages.Length)
                {
                    case 2:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1]);
                        break;
                    case 3:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2]);
                        break;
                    case 4:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3]);
                        break;
                    case 5:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4]);
                        break;
                    case 6:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5]);
                        break;
                    case 7:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5], messages[6]);
                        break;
                    case 8:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5], messages[6], messages[7]);
                        break;
                    case 9:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5], messages[6], messages[7], messages[8]);
                        break;
                    case 10:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                         messages[4], messages[5], messages[6], messages[7], messages[8], messages[9]);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unhandled exception has occurred while trying to send message to {methodName}", methodName);
            }
            return default;
        }

        public async Task<string> SendAsync(string methodName, object message)
        {
            _ = methodName ?? throw new ArgumentNullException("Argument methodName cant be null");
            _ = message ?? throw new ArgumentNullException("Argument message cant be null");
            try
            {
                await hubConnection.InvokeAsync(methodName, message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unhandled exception has occurred while trying to send message to {methodName}", methodName);
            }
            return default;
        }
        public async Task<string> SendAsync(string methodName, params object[] messages)
        {
            _ = methodName ?? throw new ArgumentNullException("Argument methodName cant be null");
            _ = messages ?? throw new ArgumentNullException("Argument messages cant be null");
            if (messages.Length > 10) throw new ArgumentException("You cant send more than 10 arguments.");
            try
            {

                switch (messages.Length)
                {
                    case 2:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1]);
                        break;
                    case 3:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2]);
                        break;
                    case 4:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3]);
                        break;
                    case 5:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4]);
                        break;
                    case 6:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5]);
                        break;
                    case 7:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5], messages[6]);
                        break;
                    case 8:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5], messages[6], messages[7]);
                        break;
                    case 9:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                            messages[4], messages[5], messages[6], messages[7], messages[8]);
                        break;
                    case 10:
                        await hubConnection.InvokeAsync(methodName, messages[0], messages[1], messages[2], messages[3],
                         messages[4], messages[5], messages[6], messages[7], messages[8], messages[9]);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unhandled exception has occurred while trying to send message to {methodName}", methodName);
            }
            return default;
        }
    }
}
