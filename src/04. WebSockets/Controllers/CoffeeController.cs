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
    public class CoffeeController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CoffeeController(
            IOrderService orderService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.orderService = orderService;
            this.httpContextAccessor = httpContextAccessor;
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
            var context = this.httpContextAccessor.HttpContext;
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                CheckResult result;
                do
                {
                    result = this.orderService.GetUpdate(id);
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
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
    }
}
