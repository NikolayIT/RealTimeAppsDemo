namespace SignalRDemo.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using SharedLibrary;

    public class CoffeeHub : Hub
    {
        private readonly IOrderService orderService;

        public CoffeeHub(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task GetUpdateForOrder(int orderId)
        {
            CheckResult result;
            do
            {
                result = this.orderService.GetUpdate(orderId);
                if (result.New)
                {
                    await this.Clients.Caller.SendAsync("ReceiveOrderUpdate", result.Update);
                }
            }
            while (!result.Finished);
            await this.Clients.Caller.SendAsync("Finished");
        }
    }
}
