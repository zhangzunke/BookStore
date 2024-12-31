using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Shared;
using BookStore.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Configurations
{
    internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");
            builder.HasKey(x => x.Id);

            builder.Property(booking => booking.Id)
                .HasConversion(bookingId => bookingId.Value, value => new BookingId(value));

            builder.OwnsOne(x => x.PriceForPeriod, builder => 
            {
                builder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });
            builder.OwnsOne(x => x.CleaningFee, builder => 
            {
                builder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });
            builder.OwnsOne(x => x.AmenitiesUpCharge, builder => 
            {
                builder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });
            builder.OwnsOne(x => x.TotalPrice, builder => 
            {
                builder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(x => x.Duration);

            builder.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(x => x.ApartmentId);
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
