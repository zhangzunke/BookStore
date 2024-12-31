using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Abstractions;

namespace BookStore.Domain.Reviews.Events
{
    public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent;
}
