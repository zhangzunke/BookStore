using BookStore.Domain.Apartments;
using BookStore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.UnitTests.Apartments
{
    internal static class ApartmentData
    {
        public static Apartment Create(Money price, Money? clearningFee = null) => new(
            ApartmentId.New(),
            new Name("Test apartment"),
            new Description("Test description"),
            new Address("Country", "State", "ZipCode", "City", "Street"),
            price,
            clearningFee ?? Money.Zero(),
            []);
    }
}
