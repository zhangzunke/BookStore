using BookStore.Domain.Apartments;
using BookStore.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Configurations
{
    internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
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

            builder.Property<byte[]>("Version").IsRowVersion();
        }
    }
}
