using Microsoft.EntityFrameworkCore;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payoneer.DotnetCore.Domain.Models;

namespace Payoneer.DotnetCore.Service.External
{
    public class PaymentServiceExternal : IPaymentServiceExternal
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentServiceExternal(
            IUnitOfWork unitOfWork,
            IRepository<Payment> paymentRepository,
            IPaymentValidator paymentValidator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentValidator = paymentValidator ?? throw new ArgumentNullException(nameof(paymentValidator));
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
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            return await _paymentRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PaymentChangeStatusResponse> UpdatePaymentStatusAsync(PaymentChangeStatusRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var payment = _paymentRepository.GetAll().FirstOrDefault(p => p.Id == request.Id);

            if (payment == null) throw new InvalidOperationException(nameof(payment));

            var validationResult = _paymentValidator.Validate(payment, request);
            if (validationResult.Result != PaymentChangeStatusResult.Ok) return validationResult;

            payment.Status = request.Status;
            payment.StatusDescription = Enum.GetName(typeof(PaymentStatus), request.Status);
            payment.Reason = request.Reason;

            _paymentRepository.Update(payment);
            await _unitOfWork.CommitAsync();

            return validationResult;
        }
    }
}
