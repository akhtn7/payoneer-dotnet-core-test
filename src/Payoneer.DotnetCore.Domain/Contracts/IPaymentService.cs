using Payoneer.DotnetCore.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Domain.Contracts
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPaymentsFiltered(IEnumerable<PaymentStatus> filter);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<PaymentChangeStatusResponse> UpdatePaymentStatusAsync(PaymentChangeStatusRequest request);
    }
}