using Microsoft.AspNetCore.Http;
using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}
