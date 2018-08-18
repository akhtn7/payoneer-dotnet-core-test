using System;
using System.Collections.Generic;
using System.Text;

namespace Payoneer.DotnetCore.Domain
{
    public class PaymentChangeStatusRequest
    {
        public int Id { get; set; }
        public PaymentStatus Status { get; set; }
        public string Reason { get; set; }
    }
}
