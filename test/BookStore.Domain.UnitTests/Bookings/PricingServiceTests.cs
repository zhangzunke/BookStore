using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Shared;
using BookStore.Domain.UnitTests.Apartments;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.UnitTests.Bookings
{
    public class PricingServiceTests
    {
        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPrice()
        {
            // Arrange
            var price = new Money(10.0m, Currency.Usd);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
            var apartment = ApartmentData.Create(price);
            var pricingService = new PricingService();

            // Act
            var pricingDetails = pricingService.CalculatePrice(apartment, period);

            // Assert
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }

        [Fact]
        public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
        {
            // Arrange
            var price = new Money(10.0m, Currency.Usd);
            var clearningFee = new Money(99.99m, Currency.Usd);
            var period = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));
            var expectedTotalPrice = new Money((price.Amount * period.LengthInDays) + clearningFee.Amount, price.Currency);
            var apartment = ApartmentData.Create(price, clearningFee);
            var pricingService = new PricingService();

            // Act
            var pricingDetails = pricingService.CalculatePrice(apartment, period);

            // Assert
            pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
        }
    }
}
