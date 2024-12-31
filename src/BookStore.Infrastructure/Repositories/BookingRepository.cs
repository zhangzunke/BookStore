using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    internal sealed class BookingRepository : Repository<Booking, BookingId>, IBookingRepository
    {
        private static readonly BookingStatus[] ActiveBookingStatuses = 
        {
            BookingStatus.Reserved,
            BookingStatus.Confirmed,
            BookingStatus.Completed
        };

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<Booking>()
                .AnyAsync(b =>
                    b.ApartmentId == apartment.Id &&
                    b.Duration.Start <= duration.End &&
                    b.Duration.End >= duration.Start &&
                    ActiveBookingStatuses.Contains(b.Status), cancellationToken);

        }
    }
}
