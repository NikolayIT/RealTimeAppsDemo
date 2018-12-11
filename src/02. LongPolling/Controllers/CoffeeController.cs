namespace LongPollingDemo.Controllers
{
    using System.Threading;

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
            CheckResult result;
            do
            {
                result = this.orderChecker.GetUpdate(orderNo);
                Thread.Sleep(1000);
            }
            while (!result.New);

            return new ObjectResult(result);
        }
    }
}
