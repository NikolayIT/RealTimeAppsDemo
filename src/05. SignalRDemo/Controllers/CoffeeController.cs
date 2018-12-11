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

        public CoffeeController(
            IOrderService orderService,
            IHubContext<CoffeeHub> coffeeHub)
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
