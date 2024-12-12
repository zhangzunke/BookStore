using BookStore.Application.Abstractions.Data;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
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

        public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
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

            return booking;
        }
    }
}
