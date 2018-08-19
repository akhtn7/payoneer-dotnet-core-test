using Payoneer.DotnetCore.Domain;

namespace Payoneer.DotnetCore.Service.External
{
    public interface IPaymentValidator
    {
        PaymentChangeStatusResponse Validate(Payment payment, PaymentChangeStatusRequest paymentChangeStatusRequest);
    }
}
