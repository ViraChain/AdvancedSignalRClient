using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("1:https://socket.greatex.ir/");
                    Console.WriteLine("2:https://localhost:5202/");
                    Console.WriteLine("Which URL:");
                    var baseUrl = Console.ReadLine() == "1"
                        ? "https://socket.greatex.ir/prices?authToken="
                        : "https://localhost:5202/prices?authToken=";
                    Console.Write(baseUrl);
                    var url = $"{baseUrl}{Console.ReadLine()}";
                    var connection = new HubConnectionBuilder()
                        .WithUrl(url, op =>
                        {
                            op.HttpMessageHandlerFactory = (message) =>
                            {
                                if (message is HttpClientHandler clientHandler)
                                    clientHandler.ServerCertificateCustomValidationCallback +=
                                         (sender, certificate, chain, sslPolicyErrors) => { return true; };
                                return message;
                            };
                        })
                        .Build();
                    connection.Closed += ex =>
                    {
                        Console.WriteLine("Connection Lost!!!");
                        WriteException(ex);
                        return Task.CompletedTask;
                    };
                    connection.Reconnected += id =>
                    {
                        Console.WriteLine("Reconnected!!!");
                        Console.WriteLine(id);
                        return Task.CompletedTask;
                    };
                    connection.Reconnecting += ex =>
                    {
                        Console.WriteLine("Reconnecting!!!");
                        WriteException(ex);
                        return Task.CompletedTask;
                    };
                    await connection.StartAsync();
                    Console.WriteLine(connection.State);
                    if (connection.State != HubConnectionState.Connected) continue;
                    Console.WriteLine("Enter MarketId:(defaul 1101)");
                    var marketId = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(marketId)) marketId = "1101";
                    await connection.InvokeAsync("GetMarketData", marketId);
                    Console.WriteLine("Enter Function Name:(defaul getMarketData)");
                    var fname = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(fname)) fname = "getMarketData";
                    var csv = new CancellationTokenSource();
                    Task.Run(async () =>
                    {
                        connection.On<string>(fname, data =>
                        {
                            Console.WriteLine(data);
                        });
                        while (true)
                        {
                            await Task.Delay(1000);
                        }
                    }, csv.Token);
                    var res = await ReadKey(connection, csv);
                    if (res) continue;
                    else break;
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }

        }
        private static void WriteException(Exception ex)
        {
            if (ex is null) return;
            Console.WriteLine("________________________________");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("________________________________");
            Console.WriteLine();
        }
        private static async Task<bool> ReadKey(HubConnection connection, CancellationTokenSource tokenSource)
        {
            while (true)
            {
                var k = Console.ReadKey().Key;
                if (k == ConsoleKey.Escape)
                {
                    Console.WriteLine("Closing application");
                    tokenSource.Cancel();
                    await connection.StopAsync();
                    await connection.DisposeAsync();
                    tokenSource.Dispose();
                    return false; ;
                }
                else if (k == ConsoleKey.Enter)
                {
                    tokenSource.Cancel();
                    await connection.StopAsync();
                    await connection.DisposeAsync();
                    tokenSource.Dispose();
                    return true;
                }
            }
        }
    }
}
