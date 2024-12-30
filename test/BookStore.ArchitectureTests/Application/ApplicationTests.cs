﻿using BookStore.Application.Abstractions.Messaging;
using BookStore.ArchitectureTests.Infrastructure;
using FluentAssertions;
using FluentValidation;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.ArchitectureTests.Application
{
    public class ApplicationTests : BaseTest
    {
        [Fact]
        public void CommandHandler_ShouldHave_NameEndingWith_CommandHandler()
        {
            var result = Types.InAssembly(ApplicationAssembly)
               .That()
               .ImplementInterface(typeof(ICommandHandler<>))
               .Or()
               .ImplementInterface(typeof(ICommandHandler<,>))
               .Should()
               .HaveNameEndingWith("CommandHandler")
               .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void CommandHandler_Should_NotBePublic()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Should()
                .NotBePublic()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void QueryHandler_ShouldHave_NameEndingWith_QueryHandler()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .HaveNameEndingWith("QueryHandler")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void QueryHandler_Should_NotBePublic()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .NotBePublic()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Validator_ShouldHave_NameEndingWith_Validator()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .HaveNameEndingWith("Validator")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_BePublic()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .BePublic()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
