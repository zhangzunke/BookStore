using BookStore.Domain.Apartments;
using BookStore.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Configurations
{
    internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        internal static readonly char[] separator = [','];

        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("Apartments");
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Address);
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .HasConversion(name => name.Value, value => new Name(value));

            builder.Property(x => x.Description)
                .HasMaxLength(2000)
                .HasConversion(description => description.Value, value => new Description(value));

            builder.OwnsOne(x => x.Price, priceBuilder => 
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(x => x.CleaningFee, priceBuilder => 
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.Property(x => x.Amenities)
                   .HasConversion(
                        amenities => string.Join(",", amenities.Select(a => (int)a)),
                        dbValue => dbValue.Split(separator).Select(int.Parse).Cast<Amenity>().ToList());

            builder.Property<byte[]>("Version").IsRowVersion();
        }
    }
}
