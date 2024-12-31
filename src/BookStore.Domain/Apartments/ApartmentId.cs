using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Apartments
{
    public record ApartmentId(Guid Value)
    {
        public static ApartmentId New() => new(Guid.NewGuid());
    }
}
