using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Bookings
{
    public record BookingId(Guid Value)
    {
        public static BookingId New() => new(Guid.NewGuid());
    }
}
