using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using Newtonsoft.Json;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Domain.Models;
using Payoneer.DotnetCore.Net.Rest;
using Payoneer.DotnetCore.Repository;
using Payoneer.DotnetCore.Service.Internal;
using Xunit;

namespace Payoneer.DotnetCore.UnitTests.Service.Internal
{
    public class PaymentServiceInternalTests
    {
        [Fact]
        public void PaymentServiceInternal_Ctor_should_not_throw_if_argument_is_valid()
        {
            //Act
            Action ctor = () => SubjectBuilder.New().Build();

            //Assert
            ctor.Should().NotThrow();
        }

        [Fact]
        public void PaymentServiceInternal_Ctor_should_throw_if_argument_is_null()
        {
            //Act
            Action ctor = () => new PaymentServiceInternal(null);

            //Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void PaymentServiceInternal_GetPaymentsFiltered_should_throw_if_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> func = async () => await subject.GetPaymentsFiltered(null);

            //Assert
            func.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(MocksForGetPaymentsFiltered))]
        public async Task PaymentServiceInternal_GetPaymentsFiltered_should_create_url(
            IEnumerable<PaymentStatus> filter,
            string url)
        {
            //Arrange
            var builder = SubjectBuilder.New();
            var subject = builder.WithPayments(filter).Build();

            //Act
            await subject.GetPaymentsFiltered(filter);

            //Assert
            A.CallTo(() => builder.RestClient.GetAsync(A<string>.That.IsEqualTo(url)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void PaymentServiceInternal_GetPaymentsFiltered_should_throw_InvalidOperationException_if_restClient_GetAsync_returns_error()
        {
            //Arrange
            var subject = SubjectBuilder.New().WithInvalidOperationException().Build();

            //Act
            Func<Task> func = async () => await subject.GetPaymentsFiltered(
                new List<PaymentStatus> { PaymentStatus.Pending });

            //Assert
            func.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [MemberData(nameof(MocksForGetPaymentsFiltered))]
        public async Task PaymentServiceInternal_GetPaymentsFiltered_should_return_payments(
            IList<PaymentStatus> filter,
            string url)
        {
            //Arrange
            var builder = SubjectBuilder.New();
            var subject = builder.WithPayments(filter).Build();
            var expectedPayments = builder.Payments
                .Where(p => filter.Contains(p.Status))
                .OrderBy(p => p.Id)
                .ToList();

            //Act
            var result = (await subject.GetPaymentsFiltered(filter)).OrderBy(p => p.Id).ToList();

            //Assert
            for (var i = 0; i < result.Count; i++)
                result[i].Should().BeEquivalentTo(expectedPayments[i]);
        }

        [Fact]
        public void PaymentServiceInternal_GetPaymentByIdAsync_should_throw_if_argument_is_invalid()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> func = async () => await subject.GetPaymentByIdAsync(0);

            //Assert
            func.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void PaymentServiceInternal_UpdatePaymentStatusAsync_should_throw_if_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> func = async () => await subject.UpdatePaymentStatusAsync(null);

            //Assert
            func.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> MocksForGetPaymentsFiltered => new[]
        {
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending }, "api/payments?paymentStatus=0" },
            new object[] { new List<PaymentStatus> { PaymentStatus.Approved }, "api/payments?paymentStatus=1" },
            new object[] { new List<PaymentStatus> { PaymentStatus.Rejected }, "api/payments?paymentStatus=99" },
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending, PaymentStatus.Approved }, "api/payments?paymentStatus=0&paymentStatus=1" },
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending, PaymentStatus.Rejected }, "api/payments?paymentStatus=0&paymentStatus=99" },
            new object[] { new List<PaymentStatus> { PaymentStatus.Approved, PaymentStatus.Rejected }, "api/payments?paymentStatus=1&paymentStatus=99" },
            new object[] { new List<PaymentStatus> { PaymentStatus.Pending, PaymentStatus.Approved, PaymentStatus.Rejected }, "api/payments?paymentStatus=0&paymentStatus=1&paymentStatus=99" }
        };

        private class SubjectBuilder
        {
            public IRestClient RestClient { get; }
            public IEnumerable<Payment> Payments { get; }

            private SubjectBuilder()
            {
                RestClient = A.Fake<IRestClient>();
                Payments = (new Fixture()).CreateMany<Payment>(6).ToList();
            }

            public static SubjectBuilder New() =>
                new SubjectBuilder();

            public SubjectBuilder WithPayments(IEnumerable<PaymentStatus> filter)
            {
                var filterdPayments = Payments.Where(p => filter.Contains(p.Status));
                var content = JsonConvert.SerializeObject(filterdPayments);
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                };

                A.CallTo(() => RestClient.GetAsync(A<string>.Ignored))
                    .Returns(Task.FromResult(response));

                return this;
            }

            public SubjectBuilder WithInvalidOperationException()
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                };

                A.CallTo(() => RestClient.GetAsync(A<string>.Ignored))
                    .Returns(Task.FromResult(response));

                return this;
            }

            public IPaymentServiceInternal Build()
            {
                return new PaymentServiceInternal(RestClient);
            }
        }
    }
}