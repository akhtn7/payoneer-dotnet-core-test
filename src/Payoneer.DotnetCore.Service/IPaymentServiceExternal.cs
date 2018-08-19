using Payoneer.DotnetCore.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Service.External
{
    public interface IPaymentServiceExternal
    {
        Task<IEnumerable<Payment>> GetPaymentsFiltered(IEnumerable<PaymentStatus> filter);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<PaymentChangeStatusResponse> UpdatePaymentStatusAsync(PaymentChangeStatusRequest request);
    }
}
