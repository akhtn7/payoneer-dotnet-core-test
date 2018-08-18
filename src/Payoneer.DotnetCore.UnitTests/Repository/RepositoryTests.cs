using FluentAssertions;
using Payoneer.DotnetCore.Repository;
using System;
using System.Threading.Tasks;
using FakeItEasy;
using Payoneer.DotnetCore.Rds;
using Xunit;

namespace Payoneer.DotnetCore.UnitTests.Repository
{
    public class RepositoryTests
    {
        [Fact]
        public void Repository_Ctor_should_not_throw_if_argument_is_valid()
        {
            //Act
            Action ctor = () => SubjectBuilder.New().Build();

            //Assert
            ctor.Should().NotThrow();
        }

        [Fact]
        public void Repository_Ctor_should_throw_if_argument_is_null()
        {
            //Act
            Action ctor = () => new Repository<FakeDbModel>(null);

            //Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Repository_Ctor_should_call_dbFactory_GetDbContext()
        {
            //Arrange
            var builder = SubjectBuilder.New();

            //Act
            var subject = builder.Build();

            //Assert
            A.CallTo(() => builder.DbFactory.GetDbContext())
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Repository_InsertAsync_should_throw_if_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> insertAsync = async () => await subject.InsertAsync(null);

            //Assert
            insertAsync.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Repository_DeleteAsync_should_throw_if_argument_is_invalid()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Func<Task> deleteAsync = async () => await subject.DeleteAsync(0);

            //Assert
            deleteAsync.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Repository_Updatec_should_throw_if_argument_is_null()
        {
            //Arrange
            var subject = SubjectBuilder.New().Build();

            //Act
            Action update = () => subject.Update(null);

            //Assert
            update.Should().Throw<ArgumentNullException>();
        }

        private class SubjectBuilder
        {
            public IDbFactory DbFactory { get; }

            private SubjectBuilder() => 
                DbFactory = A.Fake<IDbFactory>();

            public static SubjectBuilder New() => 
                new SubjectBuilder();

            public IRepository<FakeDbModel> Build() =>
                new Repository<FakeDbModel>(DbFactory);
        }

        private class FakeDbModel { }
    }
}