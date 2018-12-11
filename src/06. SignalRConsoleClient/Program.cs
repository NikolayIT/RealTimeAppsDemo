namespace SignalRConsoleClient
{
    using System;

    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44317/coffeehub")
                .AddMessagePackProtocol()
                .Build();

            connection.On<Order>(
                "NewOrder",
                (order) => Console.WriteLine($"Somebody ordered an {order.Product} ({order.Size})"));

            connection.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine("Listening. Press enter to exit...");
            Console.ReadLine();
        }
    }
}
