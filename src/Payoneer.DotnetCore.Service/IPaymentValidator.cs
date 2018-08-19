using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Domain.Models;

namespace Payoneer.DotnetCore.Service.External
{
    public interface IPaymentValidator
    {
        PaymentChangeStatusResponse Validate(Payment payment, PaymentChangeStatusRequest paymentChangeStatusRequest);
    }
}
