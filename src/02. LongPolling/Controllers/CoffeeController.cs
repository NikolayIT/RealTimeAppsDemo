namespace LongPollingDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using SharedLibrary;

    [Route("[controller]")]
    public class CoffeeController : Controller
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
        public IActionResult GetUpdateForOrder(int id)
        {
            CheckResult result;
            do
            {
                result = this.orderService.GetUpdate(id);
            }
            while (!result.New);

            return new ObjectResult(result);
        }
    }
}
