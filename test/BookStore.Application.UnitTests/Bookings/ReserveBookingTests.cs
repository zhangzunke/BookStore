﻿using BookStore.Application.Abstractions.Clock;
using BookStore.Application.Bookings.ReserveBooking;
using BookStore.Application.Exceptions;
using BookStore.Application.UnitTests.Apartments;
using BookStore.Application.UnitTests.Users;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.UnitTests.Bookings
{
    public class ReserveBookingTests
    {
        private static readonly DateTime UtcNow = DateTime.UtcNow;
        private static readonly ReserveBookingCommand Command = new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateOnly(2024, 1, 1),
            new DateOnly(2024, 1, 10));

        private readonly ReserveBookingCommandHandler _handler;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IApartmentRepository _apartmentRepositoryMock;
        private readonly IBookingRepository _bookingRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        public ReserveBookingTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _apartmentRepositoryMock = Substitute.For<IApartmentRepository>();
            _bookingRepositoryMock = Substitute.For<IBookingRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            IDateTimeProvider dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
            dateTimeProviderMock.UtcNow.Returns(UtcNow);

            _handler = new ReserveBookingCommandHandler(
                _userRepositoryMock,
                _apartmentRepositoryMock,
                _bookingRepositoryMock,
                _unitOfWorkMock,
                new PricingService(),
                dateTimeProviderMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserIsNull()
        {
            // Arrange
            _userRepositoryMock.GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(UserErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenApartmentIsNull()
        {
            // Arrange
            var user = UserData.Create();
            _userRepositoryMock.GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock.GetByIdAsync(new ApartmentId(Command.ApartmentId), Arg.Any<CancellationToken>())
                .Returns((Apartment?)null);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(ApartmentErrors.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenApartmentIsBooked()
        {
            // Arrange
            var user = UserData.Create();
            var apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock.GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock.GetByIdAsync(new ApartmentId(Command.ApartmentId), Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock.IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(true);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(BookingErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
        {
            // Arrange
            var user = UserData.Create();
            var apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock.GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock.GetByIdAsync(new ApartmentId(Command.ApartmentId), Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock.IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(true);

            _unitOfWorkMock.SaveChangesAsync().ThrowsAsync(new ConcurrencyException("Concurrency", new Exception()));

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(BookingErrors.Overlap);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenBookingIsReserved()
        {
            // Arrange
            var user = UserData.Create();
            var apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock
                .GetByIdAsync(new ApartmentId(Command.ApartmentId), Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock
                .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenBookingIsReserved()
        {
            // Arrange
            var user = UserData.Create();
            var apartment = ApartmentData.Create();
            var duration = DateRange.Create(Command.StartDate, Command.EndDate);

            _userRepositoryMock
                .GetByIdAsync(new UserId(Command.UserId), Arg.Any<CancellationToken>())
                .Returns(user);

            _apartmentRepositoryMock
                .GetByIdAsync(new ApartmentId(Command.ApartmentId), Arg.Any<CancellationToken>())
                .Returns(apartment);

            _bookingRepositoryMock
                .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
                .Returns(false);

            // Act
            var result = await _handler.Handle(Command, default);

            // Assert
            _bookingRepositoryMock.Received(1).Add(Arg.Is<Booking>(b => b.Id == new BookingId(result.Value)));
        }
    }
}
