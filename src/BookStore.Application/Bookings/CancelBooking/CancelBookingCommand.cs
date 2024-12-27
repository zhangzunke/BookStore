using BookStore.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Bookings.CancelBooking
{
    public record CancelBookingCommand(Guid BookingId) : ICommand;
}
