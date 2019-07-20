namespace ServerSentEventsDemo.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using SharedLibrary;

    [Route("[controller]")]
    public class CoffeeController : ControllerBase
    {
        private readonly IOrderService orderService;

        public CoffeeController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public IActionResult OrderCoffee(Order order)
        {
            var orderId = this.orderService.NewOrder();
            return this.Accepted(orderId);
        }

        [HttpGet("{id}")]
        public async Task GetUpdateForOrder(int id)
        {
            this.Response.ContentType = "text/event-stream";
            CheckResult result;

            do
            {
                result = this.orderService.GetUpdate(id);
                if (!result.New)
                {
                    continue;
                }

                await this.HttpContext.Response.WriteAsync($"data: {result.Update}\r\n\r\n");
                await this.HttpContext.Response.Body.FlushAsync();
            }
            while (!result.Finished);

            this.Response.Body.Close();
        }
    }
}
