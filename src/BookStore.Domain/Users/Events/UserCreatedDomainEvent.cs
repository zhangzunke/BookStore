using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Abstractions;

namespace BookStore.Domain.Users.Events
{
    public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
}
