using BookStore.Domain.Apartments;

namespace BookStore.Api.Controllers.Apartments
{
    public sealed record UpdateApartmentRequest(
        decimal PriceAmount,
        string PriceAmountCurrency,
        decimal CleaningFeeAmount,
        string CleaningFeeCurrency,
        Amenity[] Amenities);
}
