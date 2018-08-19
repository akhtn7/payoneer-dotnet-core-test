using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payoneer.DotnetCore.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int AccountHolderId { get; set; }
        [MaxLength(100)]
        public string AccountHolderName { get; set; }
        public DateTime PaymentDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [MaxLength(3)]
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }
        [MaxLength(50)]
        public string StatusDescription { get; set; }
        [MaxLength(250)]
        public string Reason { get; set; }
    }
}
