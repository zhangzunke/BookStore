using BookStore.ArchitectureTests.Infrastructure;
using BookStore.Domain.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.ArchitectureTests.Domain
{
    public class DomainTests : BaseTest
    {
        [Fact]
        public void DomainEvents_Should_BeSealed()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void DomainEvent_ShouldHave_DomainEventPostfix()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .HaveNameEndingWith("DomainEvent")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Entities_ShouldHave_PrivateParameterlessConstructor()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(IEntity))
                .GetTypes();

            var failingTypes = new List<Type>();
            foreach (var entityType in entityTypes)
            {
                var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

                if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
                {
                    failingTypes.Add(entityType);
                }
            }

            failingTypes.Should().BeEmpty();
        }
    }
}
