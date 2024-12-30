using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Bookings.Events;
using BookStore.Domain.Shared;
using BookStore.Domain.UnitTests.Apartments;
using BookStore.Domain.UnitTests.Infrastructure;
using BookStore.Domain.UnitTests.Users;
using BookStore.Domain.Users;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.UnitTests.Bookings
{
    public class BookingTests : BaseTest
    {
        [Fact]
        public void Reserve_Should_RaiseBookingReservedDomainEvent()
        {
            // Arrange
            var user = User.CreateUser(UserData.FirstName, UserData.LastName, UserData.Email);
            var price = new Money(10.0m, Currency.Usd);
            var duration = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var apartment = ApartmentData.Create(price);

            var pricingService = new PricingService();

            // Act
            var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);

            // Assert
            var bookingReservedDomainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);
            bookingReservedDomainEvent.BookingId.Should().Be(booking.Id);
        }
    }
}
