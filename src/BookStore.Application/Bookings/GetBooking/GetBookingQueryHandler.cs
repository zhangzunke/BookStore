using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Bookings;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Bookings.GetBooking
{
    internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IUserContext _userContext;

        public GetBookingQueryHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IUserContext userContext)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _userContext = userContext;
        }

        public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT 
                    Id,
                    ApartmentId,
                    UserId,
                    Status,
                    PriceAmount,
                    PriceCurrency,
                    CleaningFeeAmount,
                    CleaningFeeCurrency,
                    AmenitiesUpChargeAmount,
                    AmenitiesUpChargeCurrency,
                    TotalPriceAmount,
                    TotalPriceCurrency,
                    DurationStart,
                    DurationEnd,
                    CreatedOnUtc
                FROM Bookings
                WHERE Id = @BookingId
                """;

            var booking = await connection.QueryFirstOrDefaultAsync<BookingResponse>(
                sql,
                new
                {
                    request.BookingId
                });

            if (booking is null || booking.UserId != _userContext.UserId)
            {
                return Result.Failure<BookingResponse>(BookingErrors.NotFound);
            }

            return booking;
        }
    }
}
