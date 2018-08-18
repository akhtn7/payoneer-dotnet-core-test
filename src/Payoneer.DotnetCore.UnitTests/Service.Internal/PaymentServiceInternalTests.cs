using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
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

        private class SubjectBuilder
        {
            public IRestClient RestClient { get; }

            private SubjectBuilder()
            {
                RestClient = A.Fake<IRestClient>();
            }

            public static SubjectBuilder New() =>
                new SubjectBuilder();


            public IPaymentServiceInternal Build() =>
                new PaymentServiceInternal(RestClient);
        }
    }
}
