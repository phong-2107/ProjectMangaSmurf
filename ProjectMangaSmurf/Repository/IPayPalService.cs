
using PayPalCheckoutSdk.Orders;

namespace ProjectMangaSmurf.Repository
{
    public interface IPayPalService
    {
        public Task<string> CreateOrderAsync(decimal total, string currency);
        public Task<Order> CaptureOrderAsync(string orderId);
        public Task<Order> CreatePayPalOrder(double amount, string name, string phone, int orderId);
    }
}
