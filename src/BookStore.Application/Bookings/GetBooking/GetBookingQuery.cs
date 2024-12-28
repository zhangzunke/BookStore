using BookStore.Application.Abstractions.Caching;
using BookStore.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Bookings.GetBooking
{
    public sealed record GetBookingQuery(Guid BookingId) : ICachedQuery<BookingResponse>
    {
        public string CacheKey => $"bookings-{BookingId}";

        public TimeSpan? Expiration => null;
    }
}
