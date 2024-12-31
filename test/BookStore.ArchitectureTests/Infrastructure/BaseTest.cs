using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Infrastructure;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.ArchitectureTests.Infrastructure
{
    public class BaseTest
    {
        protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;
        protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;
        protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
        protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;

    }
}
