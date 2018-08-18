using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Payoneer.DotnetCore.Net.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Payoneer.DotnetCore.UnitTests.Net.Rest
{
    public class RestClientTests
    {
        [Fact]
        public void InternalRestClient_Ctor_should_create_with_valid_arguments()
        {
            //Act
            Action ctor = () => SubjectBuilder.New().Build();

            //Assert
            ctor.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(IncompleteMocksForCtor))]
        public void InternalRestClient_Ctor_should_not_accept_null_as_any_parameter(
            IHttpClientFactory httpClientFactory,
            IOptions<ExternalPaymentServiceOptions> externalPaymentServiceOptions)
        {
            // Act
            Action ctor = () => new RestClient(httpClientFactory, externalPaymentServiceOptions);

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void InternalRestClient_GetAsync_should_throw_if_any_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> getAsync = async () => await subject.GetAsync(null);

            //Assert
            getAsync.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(IncompleteMocksForPostAsync))]
        public void InternalRestClient_PutAsync_should_throw_if_argument_is_null(string relativeUrl, object value)
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> putAsync = async () => await subject.PutAsync(relativeUrl, value);

            //Assert
            putAsync.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> IncompleteMocksForCtor => new[]
        {
            new object[] { null, A.Fake<IOptions<ExternalPaymentServiceOptions>>() },
            new object[] { A.Fake<IHttpClientFactory>(), null },
        };

        public static IEnumerable<object[]> IncompleteMocksForPostAsync => new[]
        {
            new object[] { null, A.Fake<object>() },
            new object[] { new Fixture().Create<string>(), null }
        };

        private class SubjectBuilder
        {
            private SubjectBuilder() { }

            public static SubjectBuilder New() => new SubjectBuilder();

            public IRestClient Build()
            {
                var httpClientFactory = A.Fake<IHttpClientFactory>();
                var externalPaymentServiceOptions = new ExternalPaymentServiceOptions
                { BaseUrl = "http://example.com/" };
                var externalPaymentServiceOptionsAccessor = new OptionsWrapper<ExternalPaymentServiceOptions>(externalPaymentServiceOptions);

                return new RestClient(httpClientFactory, externalPaymentServiceOptionsAccessor);
            }
        }
    }
}
