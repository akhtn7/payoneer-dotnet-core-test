using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Domain.Models;
using Payoneer.DotnetCore.Repository;
using Payoneer.DotnetCore.Service.External;
using Xunit;

namespace Payoneer.DotnetCore.UnitTests.Service.External
{
    public class PaymentServiceExternalTests
    {
        [Fact]
        public void PaymentServiceExternal_Ctor_should_not_throw_if_argument_is_valid()
        {
            //Act
            Action ctor = () => SubjectBuilder.New().Build();

            //Assert
            ctor.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(IncompleteMocksForCtor))]
        public void PaymentServiceExternal_Ctor_should_not_accept_a_not_initialized_parameter(
            IUnitOfWork unitOfWork,
            IRepository<Payment> paymentRepository,
            IPaymentValidator paymentValidator)
        {
            // Act
            Action ctor = () => new PaymentServiceExternal(unitOfWork, paymentRepository, paymentValidator);

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PaymentServiceExternal_GetPaymentsFiltered_should_throw_if_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> func = async () => await subject.GetPaymentsFiltered(null);

            //Assert
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PaymentServiceExternal_GetPaymentsFiltered_should_call_paymentRepository_GetAll()
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

        [Fact]
        public void PaymentServiceExternal_GetPaymentByIdAsync_should_throw_if_argument_is_invalid()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> func = async () => await subject.GetPaymentByIdAsync(0);

            //Assert
            func.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void PaymentServiceExternal_UpdatePaymentStatusAsync_should_throw_if_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> func = async () => await subject.UpdatePaymentStatusAsync(null);

            //Assert
            func.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> IncompleteMocksForCtor => new[]
        {
            new object[] { null, A.Fake<IRepository<Payment>>(), A.Fake<IPaymentValidator>() },
            new object[] { A.Fake<IUnitOfWork>(), null, A.Fake<IPaymentValidator>() },
            new object[] { A.Fake<IUnitOfWork>(), A.Fake<IRepository<Payment>>(), null }
        };

        private class SubjectBuilder
        {
            public IUnitOfWork UnitOfWork { get; }
            public IRepository<Payment> PaymentRepository { get; }
            public IPaymentValidator PaymentValidator { get; }

            public IEnumerable<Payment> Payments { get; }

            private SubjectBuilder()
            {
                UnitOfWork = A.Fake<IUnitOfWork>();
                PaymentRepository = A.Fake<IRepository<Payment>>();
                PaymentValidator = A.Fake<IPaymentValidator>();
                Payments = (new Fixture()).CreateMany<Payment>(6).ToList();
            }

            public static SubjectBuilder New() =>
                new SubjectBuilder();

            public IPaymentServiceExternal Build() =>
                new PaymentServiceExternal(UnitOfWork, PaymentRepository, PaymentValidator);
        }
    }
}