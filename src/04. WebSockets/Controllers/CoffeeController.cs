namespace WebSocketsDemo.Controllers
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using SharedLibrary;

    [Route("[controller]")]
    public class CoffeeController : Controller
    {
        private readonly OrderChecker orderChecker;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CoffeeController(OrderChecker orderChecker, IHttpContextAccessor httpContextAccessor)
        {
            this.orderChecker = orderChecker;
            this.httpContextAccessor = httpContextAccessor;
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
            var context = this.httpContextAccessor.HttpContext;
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await this.SendEvents(webSocket, orderNo);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task SendEvents(WebSocket webSocket, int orderNo)
        {
            CheckResult result;

            do
            {
                result = this.orderChecker.GetUpdate(orderNo);
                Thread.Sleep(1000);

                if (!result.New)
                {
                    continue;
                }

                var jsonMessage = $"\"{result.Update}\"";
                await webSocket.SendAsync(
                    buffer: new ArraySegment<byte>(
                        array: Encoding.ASCII.GetBytes(jsonMessage),
                        offset: 0,
                        count: jsonMessage.Length),
                    messageType: WebSocketMessageType.Text,
                    endOfMessage: true,
                    cancellationToken: CancellationToken.None);
            }
            while (!result.Finished);
        }
    }
}
