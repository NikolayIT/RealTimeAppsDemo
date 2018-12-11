namespace ServerSentEventsDemo.Controllers
{
    using System.Threading;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using SharedLibrary;

    [Route("[controller]")]
    public class CoffeeController : Controller
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
        public async void GetUpdateForOrder(int orderNo)
        {
            this.Response.ContentType = "text/event-stream";
            CheckResult result;

            do
            {
                result = this.orderChecker.GetUpdate(orderNo);
                Thread.Sleep(1000);
                if (!result.New)
                {
                    continue;
                }

                await this.HttpContext.Response.WriteAsync(result.Update);
                await this.HttpContext.Response.Body.FlushAsync();
            }
            while (!result.Finished);

            this.Response.Body.Close();
        }
    }
}
