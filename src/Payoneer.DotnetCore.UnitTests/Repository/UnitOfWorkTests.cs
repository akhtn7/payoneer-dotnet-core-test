using FakeItEasy;
using FluentAssertions;
using Payoneer.DotnetCore.Rds;
using Payoneer.DotnetCore.Repository;
using System;
using Xunit;

namespace Payoneer.DotnetCore.UnitTests.Repository
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void UnitOfWork_Ctor_should_not_throw_if_argument_is_valid()
        {
            //Act
            Action ctor = () => SubjectBuilder.New().Build();

            //Assert
            ctor.Should().NotThrow();
        }

        [Fact]
        public void UnitOfWork_Ctor_should_throw_if_argument_is_null()
        {
            //Act
            Action ctor = () => new UnitOfWork(null);

            //Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitOfWork_Ctor_should_call_dbFactory_GetDbContext()
        {
            //Arrange
            var builder = SubjectBuilder.New();

            //Act
            var subject = builder.Build();

            //Assert
            A.CallTo(() => builder.DbFactory.GetDbContext())
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private class SubjectBuilder
        {
            public IDbFactory DbFactory { get; }

            private SubjectBuilder() =>
                DbFactory = A.Fake<IDbFactory>();

            public static SubjectBuilder New() =>
                new SubjectBuilder();

            public IUnitOfWork Build() =>
                new UnitOfWork(DbFactory);
        }
    }
}
