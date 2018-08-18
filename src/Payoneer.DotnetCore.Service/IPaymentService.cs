using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Payoneer.DotnetCore.Domain;

namespace Payoneer.DotnetCore.Service
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPaymentsFiltered(IEnumerable<PaymentStatus> filter);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task UpdatePaymentStatusAsync(PaymentChangeStatusRequest request);
    }
}
