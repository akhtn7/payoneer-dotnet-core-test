using System.Collections.Generic;
using System.Threading.Tasks;
using Payoneer.DotnetCore.Domain;

namespace Payoneer.DotnetCore.Service.Internal
{
    public interface IPaymentServiceInternal
    {
        Task<IEnumerable<Payment>> GetPaymentsFiltered(IEnumerable<PaymentStatus> filter);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task UpdatePaymentStatusAsync(PaymentChangeStatusRequest request);
    }
}
