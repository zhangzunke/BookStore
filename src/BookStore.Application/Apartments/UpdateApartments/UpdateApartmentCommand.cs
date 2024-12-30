using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Apartments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Apartments.UpdateApartments
{
    public sealed record UpdateApartmentCommand(
        Guid ApartmentId,
        decimal PriceAmount,
        string PriceAmountCurrency,
        decimal CleaningFeeAmount,
        string CleaningFeeCurrency,
        Amenity[] Amenities) : ICommand;
}
