using BookStore.Domain.Apartments;
using BookStore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.UnitTests.Apartments
{
    internal static class ApartmentData
    {
        public static Apartment Create() => new(
            ApartmentId.New(),
            new Name("Test apartment"),
            new Description("Test description"),
            new Address("Country", "State", "ZipCode", "City", "Street"),
            new Money(100.0m, Currency.Usd),
            Money.Zero(),
            []);
    }
}
