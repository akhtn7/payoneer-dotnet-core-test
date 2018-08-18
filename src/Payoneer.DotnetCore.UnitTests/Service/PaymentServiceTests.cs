using FakeItEasy;
using FluentAssertions;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Repository;
using Payoneer.DotnetCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace Payoneer.DotnetCore.UnitTests.Service
{
    public class PaymentServiceTests
    {
        [Fact]
        public void PaymentService_Ctor_should_not_throw_if_argument_is_valid()
        {
            //Act
            Action ctor = () => SubjectBuilder.New().Build();

            //Assert
            ctor.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(IncompleteMocksForCtor))]
        public void PaymentService_Ctor_should_not_accept_a_not_initialized_parameter(
            IUnitOfWork unitOfWork,
            IRepository<Payment> paymentRepository)
        {
            // Act
            Action ctor = () => new PaymentService(unitOfWork, paymentRepository);

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PaymentService_GetPaymentsFiltered_should_throw_if_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> func = async () => await subject.GetPaymentsFiltered(null);

            //Assert
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PaymentService_GetPaymentsFiltered_should_call_paymentRepository_GetAll()
        {
            //Arrange
            var builder = SubjectBuilder.New();
            var subject = builder.Build();

            //Act
            subject.GetPaymentsFiltered(Enumerable.Empty<PaymentStatus>());

            //Assert
            A.CallTo(() => builder.PaymentRepository.GetAll())
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Theory]
        [MemberData(nameof(MocksForGetPaymentsFiltered))]
        public async Task PaymentService_GetPaymentsFiltered_should_filter_Payments(
            IList<PaymentStatus> filter)
        {
            //Arrange
            var builder = SubjectBuilder.New();
            var subject = builder.Build();

            var x = builder.Payments
                .Where(p => filter.Contains(p.Status))
                .OrderBy(p => p.Id);

            //Act
            var result = (await subject.GetPaymentsFiltered(filter)).OrderBy(p => p.Id).ToList();

            //Assert
            builder.Payments
                .Where(p => filter.Contains(p.Status))
                .OrderBy(p => p.Id)
                .SequenceEqual(result)
                .Should()
                .BeTrue();
        }

        public static IEnumerable<object[]> IncompleteMocksForCtor => new[]
        {
            new object[] { null, A.Fake<IRepository<Payment>>() },
            new object[] { A.Fake<IUnitOfWork>(), null }
        };

        public static IEnumerable<object[]> MocksForGetPaymentsFiltered => new[]
        {
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending } },
            new object[] { new List<PaymentStatus> { PaymentStatus.Approved } },
            new object[] { new List<PaymentStatus> { PaymentStatus.Rejected } },
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending, PaymentStatus.Approved } },
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending, PaymentStatus.Rejected } },
            new object[] { new List<PaymentStatus> { PaymentStatus.Approved, PaymentStatus.Rejected } },
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending, PaymentStatus.Approved, PaymentStatus.Rejected } }
        };

        private class SubjectBuilder
        {
            public IUnitOfWork UnitOfWork { get; }
            public IRepository<Payment> PaymentRepository { get; }

            public IEnumerable<Payment> Payments { get; }

            private SubjectBuilder()
            {
                UnitOfWork = A.Fake<IUnitOfWork>();
                PaymentRepository = A.Fake<IRepository<Payment>>();
                Payments = (new Fixture()).CreateMany<Payment>(6).ToList();
            }

            public static SubjectBuilder New() =>
                new SubjectBuilder();

            public SubjectBuilder WithPayments()
            {
                A.CallTo(() => PaymentRepository.GetAll()).Returns(Payments.AsQueryable());

                return this;
            }

            public IPaymentService Build() =>
                new PaymentService(UnitOfWork, PaymentRepository);
        }
    }
}
