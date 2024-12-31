using BookStore.Domain.Apartments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Bookings
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken = default);
        Task<bool> IsOverlappingAsync(Apartment apartment, DateRange dateRange, CancellationToken cancellationToken = default);
        void Add(Booking booking);
    }
}
