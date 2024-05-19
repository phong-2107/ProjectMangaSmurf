using PayPalCheckoutSdk.Orders;
using ProjectMangaSmurf.Models;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Repository
{
    public class PayPalService : IPayPalService
    {

        public async Task<string> CreateOrderAsync(decimal total, string currency)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = currency,
                        Value = total.ToString("F2")
                    }
                }
            }
            });

            var response = await PayPalClient.Client().Execute(request);
            var order = response.Result<Order>();
            return order.Links.FirstOrDefault(link => link.Rel == "approve").Href;
        }
        public async Task<Order> CaptureOrderAsync(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            var response = await PayPalClient.Client().Execute(request);
            return response.Result<Order>();
        }

        public async Task<Order> CreatePayPalOrder(double amount, string name, string phone, int orderId)
        {
            var amountUSD = amount / 25.00; 
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
        {
            new PurchaseUnitRequest()
            {
                AmountWithBreakdown = new AmountWithBreakdown()
                {
                    CurrencyCode = "USD",
                    Value = amountUSD.ToString("F2")
                },
                Description = $"Order #{orderId} by {name}, {phone}"
            }
        }
            });

            var response = await PayPalClient.Client().Execute(request);
            return response.Result<Order>();
        }

    }
}
