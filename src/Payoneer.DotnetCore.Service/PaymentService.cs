using Microsoft.EntityFrameworkCore;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Payment> _paymentRepository;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IRepository<Payment> paymentRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        }

        public async Task<IEnumerable<Payment>> GetPaymentsFiltered(IEnumerable<PaymentStatus> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return await _paymentRepository.GetAll()
                .Where(p => filter.Contains(p.Status))
                .ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

            return await _paymentRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePaymentStatusAsync(PaymentChangeStatusRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var payment = _paymentRepository.GetAll().FirstOrDefault(p => p.Id == request.Id);

            if (payment == null) throw new InvalidOperationException(nameof(payment));

            // ToDo: add validation

            payment.Status = request.Status;
            _paymentRepository.Update(payment);
            await _unitOfWork.CommitAsync();
        }
    }
}
