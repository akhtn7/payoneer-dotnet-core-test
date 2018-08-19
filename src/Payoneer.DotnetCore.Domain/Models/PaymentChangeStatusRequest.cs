namespace Payoneer.DotnetCore.Domain.Models
{
    public class PaymentChangeStatusRequest
    {
        public int Id { get; set; }
        public PaymentStatus Status { get; set; }
        public string Reason { get; set; }
    }
}
