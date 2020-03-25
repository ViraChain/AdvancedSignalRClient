using AdvancedSignalRClientDaemon.SerilogUtils;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedSignalRClientDaemon.HubClients
{
    public enum Statuses
    {
        Connected,
        Disconnected,
        Reconnecting
    }
    public delegate void StatusChanged(Statuses staus);
    public interface IHubClient
    {
        Task StartAsync();
        Task StopAsync();
        Task<string> SendAsync(string methodName, string message);
        Task<string> SendAsync(string methodName, params string[] messages);
        Task<string> SendAsync(string methodName, object message);
        Task<string> SendAsync(string methodName, params object[] messages);
        IAsyncEnumerable<string> RecieveMessages(string serverMethodName);
        void CloseReciever(string serverMethodName);
        event StatusChanged OnStatusChanged;
    }
    public class HubClient : IHubClient, IAsyncDisposable
    {

        private readonly HubConnection hubConnection;
        private readonly ILogger logger;
        private readonly string URL;
        private readonly string HubName;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Dictionary<string, CancellationTokenSource> Receivers;

        public event StatusChanged OnStatusChanged;

        public HubClient(HubConnection hubConnection, ILogger logger, string uRL, string hubName)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Receivers = new Dictionary<string, CancellationTokenSource>();
            URL = uRL;
            HubName = hubName;
            this.hubConnection = hubConnection;
            this.logger = logger.ForContext(new HubEnricher<HubClient>(hubName, URL));
        }

        private void Init()
        {
            hubConnection.Closed += HandleClosed;
            hubConnection.Reconnecting += HandleReconnecting;
            hubConnection.Reconnected += HandleReconnected;
        }

        private Task HandleClosed(Exception ex)
        {
            OnStatusChanged?.Invoke(Statuses.Disconnected);
            if (hubConnection.State == HubConnectionState.Disconnected)
                logger.Error(ex, "The connection to the server has been intrupted.");
            else if (hubConnection.State == HubConnectionState.Reconnecting)
                logger.Warning("Trying to restablish the connection to the server.");
            return Task.CompletedTask;
        }
        private Task HandleReconnecting(Exception ex)
        {
            OnStatusChanged?.Invoke(Statuses.Reconnecting);
            logger.Warning("Trying to restablish the connection to the server.");
            return Task.CompletedTask;
        }
        private Task HandleReconnected(string connectionId)
        {
            OnStatusChanged?.Invoke(Statuses.Connected);
            logger.Information("New connection has been stablished to the server with connection ID {connectionId}.", connectionId);
            return Task.CompletedTask;
        }
        public async ValueTask DisposeAsync()
        {
            hubConnection.Closed -= HandleClosed;
            hubConnection.Reconnecting -= HandleReconnecting;
            hubConnection.Reconnected -= HandleReconnected;

            foreach (var item in Receivers)
            {
                item.Value.Cancel();

            }
            foreach (var item in Receivers)
            {
                item.Value.Dispose();
            }
            Receivers.Clear();

            await StopAsync();
            cancellationTokenSource.Dispose();
            await hubConnection?.DisposeAsync();
        }

        public async Task StartAsync()
        {
            try
            {
                await hubConnection.StartAsync(cancellationTokenSource.Token);
                OnStatusChanged?.Invoke(Statuses.Connected);
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke(Statuses.Disconnected);
                logger.Error(ex, "An unhandled exception has occurred while trying to start the connection to server.");
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
                logger.Error(ex, "An unhandled exception has occurred while trying to close the connection to server.");
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

        public async IAsyncEnumerable<string> RecieveMessages(string serverMethodName)
        {
            if (Receivers.ContainsKey(serverMethodName))
            {
                var tokenSrc = new CancellationTokenSource();
                var res = new ConcurrentQueue<string>();
                var _signal = new SemaphoreSlim(0);

                hubConnection.On<string>(serverMethodName, data =>
                    {
                        res.Enqueue(data);
                        _signal.Release();
                    });
                Receivers.Add(serverMethodName, tokenSrc);
                while (!tokenSrc.Token.IsCancellationRequested)
                {
                    await _signal.WaitAsync(tokenSrc.Token);
                    res.TryDequeue(out var ret);
                    yield return ret;
                }
                yield return $"Closing the connection to {serverMethodName}!!!";
            }
        }
        public void CloseReciever(string serverMethodName)
        {
            if (Receivers.ContainsKey(serverMethodName))
            {
                var token = Receivers[serverMethodName];
                token.Cancel();
                Receivers.Remove(serverMethodName);
            }
            hubConnection.Remove(serverMethodName);
        }
    }
}
