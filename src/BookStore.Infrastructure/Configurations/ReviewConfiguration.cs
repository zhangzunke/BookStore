using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Reviews;
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
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Rating)
                .HasConversion(rating => rating.Value, value => Rating.Create(value).Value);

            builder.Property(x => x.Comment)
                .HasMaxLength(200)
                .HasConversion(comment => comment.Value, value => new Comment(value));

            builder.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(x => x.ApartmentId);
            builder.HasOne<Booking>()
                .WithMany()
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
