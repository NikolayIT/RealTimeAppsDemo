namespace SignalRDemo.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    using SharedLibrary;

    using SignalRDemo.Hubs;

    [Route("[controller]")]
    public class CoffeeController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IHubContext<CoffeeHub> coffeeHub;

        public CoffeeController(IOrderService orderService, IHubContext<CoffeeHub> coffeeHub)
        {
            this.orderService = orderService;
            this.coffeeHub = coffeeHub;
        }

        [HttpPost]
        public async Task<IActionResult> OrderCoffee([FromBody] Order order)
        {
            await this.coffeeHub.Clients.All.SendAsync("NewOrder", order);
            var orderId = this.orderService.NewOrder();
            return this.Accepted(orderId);
        }
    }
}

/* Console client code
static void Main(string[] args)
{
    Console.WriteLine("Press a key to start listening..");
    Console.ReadKey();
    var connection = new HubConnectionBuilder()
        .WithUrl("http://localhost:60909/coffeehub")
        .AddMessagePackProtocol()
        .Build();

    connection.On<Order>("NewOrder", (order) => 
        Console.WriteLine($"Somebody ordered an {order.Product}"));

    connection.StartAsync().GetAwaiter().GetResult();

    Console.WriteLine("Listening. Press a key to quit");
    Console.ReadKey();
}
*/