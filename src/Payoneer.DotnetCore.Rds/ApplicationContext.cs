using Microsoft.EntityFrameworkCore;
using Payoneer.DotnetCore.Domain.Models;
using System;
using System.Globalization;

namespace Payoneer.DotnetCore.Rds
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>().HasData(
                new
                {
                    Id = 832321,
                    AccountHolderId = 15651,
                    AccountHolderName = "Alex Dumsky",
                    PaymentDate = DateTime.ParseExact(
                        "2015-01-23T18:25:43.511Z",
                        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal |
                        DateTimeStyles.AdjustToUniversal),
                    Amount = 445.12m,
                    Currency = "EUR",
                    Status = PaymentStatus.Pending,
                    StatusDescription = "Pending",
                },
                new
                {
                    Id = 806532,
                    AccountHolderId = 46556,
                    AccountHolderName = "Dudi Elias",
                    PaymentDate = DateTime.ParseExact(
                        "2015-02-10T18:25:43.511Z",
                        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal |
                        DateTimeStyles.AdjustToUniversal),
                    Amount = 4511.12m,
                    Currency = "EUR",
                    Status = PaymentStatus.Pending,
                    StatusDescription = "Pending"
                },
                new
                {
                    Id = 7845431,
                    AccountHolderId = 48481,
                    AccountHolderName = "Niv Cohen",
                    PaymentDate = DateTime.ParseExact(
                        "2015-04-01T18:25:43.511Z",
                        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal |
                        DateTimeStyles.AdjustToUniversal),
                    Amount = 10.99m,
                    Currency = "USD",
                    Status = PaymentStatus.Approved,
                    StatusDescription = "Approved",
                    Reason = "Good Person"
                },
                new
                {
                    Id = 545341,
                    AccountHolderId = 32131,
                    AccountHolderName = "Alex Dumsky",
                    PaymentDate = DateTime.ParseExact(
                        "2016-02-21T18:25:43.511Z",
                        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal |
                        DateTimeStyles.AdjustToUniversal),
                    Amount = 9952.48m,
                    Currency = "EUR",
                    Status = PaymentStatus.Rejected,
                    StatusDescription = "Rejected",
                    Reason = "This is suspicious"
                });
        }
    }
}
