namespace PollingDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using SharedLibrary;

    [Route("[controller]")]
    public class CoffeeController: Controller
    {
        private readonly OrderChecker orderChecker;

        public CoffeeController(OrderChecker orderChecker)
        {
            this.orderChecker = orderChecker;
        }

        [HttpPost]
        public IActionResult OrderCoffee(Order order)
        {
            // Start process for order
            return this.Accepted(1); // return order id 1
        }

        [HttpGet("{orderNo}")]
        public IActionResult GetUpdateForOrder(int orderNo)
        {
            var result = this.orderChecker.GetUpdate(orderNo);
            if (result.New)
            {
                return new ObjectResult(result);
            }

            return this.NoContent();
        }
    }
}
