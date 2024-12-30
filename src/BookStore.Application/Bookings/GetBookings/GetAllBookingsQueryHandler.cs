﻿using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Bookings.GetBookings
{
    internal sealed class GetAllBookingsQueryHandler : IQueryHandler<GetAllBookingsQuery, IReadOnlyList<BookingResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserContext _userContext;

        public GetAllBookingsQueryHandler(IApplicationDbContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<Result<IReadOnlyList<BookingResponse>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            List<BookingResponse> bookings = await _context.Bookings
                .Where(b => b.UserId == _userContext.UserId)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    ApartmentId = b.ApartmentId,
                    UserId = b.UserId,
                    Status = (int)b.Status,
                    PriceAmount = b.PriceForPeriod.Amount,
                    PriceCurrency = b.PriceForPeriod.Currency.Code,
                    CleaningFeeAmount = b.CleaningFee.Amount,
                    CleaningFeeCurrency = b.CleaningFee.Currency.Code,
                    AmenitiesUpChargeAmount = b.AmenitiesUpCharge.Amount,
                    AmenitiesUpChargeCurrency = b.AmenitiesUpCharge.Currency.Code,
                    TotalPriceAmount = b.TotalPrice.Amount,
                    TotalPriceCurrency = b.TotalPrice.Currency.Code,
                    DurationStart = b.Duration.Start,
                    DurationEnd = b.Duration.End,
                    CreatedOnUtc = b.CreatedOnUtc
                })
                .ToListAsync(cancellationToken);

            return bookings;
        }
    }
}
