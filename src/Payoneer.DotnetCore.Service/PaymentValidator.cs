using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Domain.Models;

namespace Payoneer.DotnetCore.Service.External
{
    public class PaymentValidator : IPaymentValidator
    {
        public PaymentChangeStatusResponse Validate(Payment payment, PaymentChangeStatusRequest paymentChangeStatusRequest)
        {
            if(payment.Status == PaymentStatus.Approved 
               || payment.Status == PaymentStatus.Rejected)
            return new PaymentChangeStatusResponse()
            {
                Result = PaymentChangeStatusResult.Error,
                Message = "Can't change status for payment which has 'Approved' or 'Rejected' status"
            };

            switch (paymentChangeStatusRequest.Status)
            {
                case PaymentStatus.Approved when payment.Currency == "EUR" && payment.Amount > 4000:
                    return new PaymentChangeStatusResponse()
                    {
                        Result = PaymentChangeStatusResult.ValidationError,
                        Message = "Can't approve payment with amount more than 4000 for EUR"
                    };
                case PaymentStatus.Rejected when paymentChangeStatusRequest.Reason != "This is suspicious":
                    return new PaymentChangeStatusResponse()
                    {
                        Result = PaymentChangeStatusResult.ValidationError,
                        Message = "Reason should be 'This is suspicious' for Reject"
                    };
            }

            return new PaymentChangeStatusResponse()
            {
               Result = PaymentChangeStatusResult.Ok,
               Message = string.Empty
            };
        }
    }
}