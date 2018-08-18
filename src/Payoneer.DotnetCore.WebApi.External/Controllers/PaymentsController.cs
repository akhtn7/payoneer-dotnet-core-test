using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Service;

namespace Payoneer.DotnetCore.WebApi.External.Controllers
{
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService) =>
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));

        // GET api/payments?paymentStatus=0&paymentStatus=2
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] IEnumerable<PaymentStatus> paymentStatus)
        {
            if (paymentStatus == null) return BadRequest();

            var payments = await _paymentService.GetPaymentsFiltered(paymentStatus);
            return Ok(payments);
        }

        // GET api/payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            if (id < 0) return BadRequest();

            var payment = await _paymentService.GetPaymentByIdAsync(id);

            if (payment == null) return NotFound();

            return Ok(payment);
        }

        // PUT api/payments/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromBody] PaymentChangeStatusRequest request)
        {
            if (request == null) return BadRequest();

            await _paymentService.UpdatePaymentStatusAsync(request);
            return Ok();
        }
    }
}
