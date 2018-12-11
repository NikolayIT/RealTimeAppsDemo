namespace SharedLibrary
{
    public interface IOrderService
    {
        CheckResult GetUpdate(int orderId);

        int NewOrder();
    }
}
