using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Users
{
    public record UserId(Guid Value)
    {
        public static UserId New() => new(Guid.NewGuid());
    }
}
