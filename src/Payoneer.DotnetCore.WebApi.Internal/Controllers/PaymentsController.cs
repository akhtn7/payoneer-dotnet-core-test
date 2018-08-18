using Microsoft.AspNetCore.Mvc;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Service.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payoneer.DotnetCore.WebApi.Internal.Controllers
{
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentServiceInternal _paymentServiceInternal;

        public PaymentsController(IPaymentServiceInternal paymentServiceInternal) =>
            _paymentServiceInternal = paymentServiceInternal ?? throw new ArgumentNullException(nameof(paymentServiceInternal));

        // GET api/payments?paymentStatus=0&paymentStatus=2
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] IEnumerable<PaymentStatus> paymentStatus)
        {
            if (paymentStatus == null) return BadRequest();

            var payments = await _paymentServiceInternal.GetPaymentsFiltered(paymentStatus);
            return Ok(payments);
        }

        // GET api/payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();

            var payment = await _paymentServiceInternal.GetPaymentByIdAsync(id);

            if (payment == null) return NotFound();

            return Ok(payment);
        }

        // PUT api/payments/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromBody] PaymentChangeStatusRequest request)
        {
            if (request == null) return BadRequest();

            await _paymentServiceInternal.UpdatePaymentStatusAsync(request);
            return Ok();
        }
    }
}
