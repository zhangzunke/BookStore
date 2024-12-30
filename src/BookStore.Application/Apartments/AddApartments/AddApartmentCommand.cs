using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Apartments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Apartments.AddApartments
{
    public sealed record AddApartmentCommand(
        string Name,
        string Description,
        string Country,
        string State,
        string ZipCode,
        string City,
        string Street,
        decimal PriceAmount,
        string PriceCurrency,
        decimal CleaningFeeAmount,
        string CleaningFeeCurrency,
        Amenity[] Amenities) : ICommand;
}
