using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IVNPayRepository
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
