using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Reviews
{
    public record ReviewId(Guid Value)
    {
        public static ReviewId New() => new(Guid.NewGuid());
    }
}
