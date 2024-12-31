using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Reviews.Events;
using BookStore.Domain.Users;

namespace BookStore.Domain.Reviews
{
    public sealed class Review : Entity<ReviewId>
    {
        private Review(
            ReviewId id,
            ApartmentId apartmentId,
            BookingId bookingId,
            UserId userId,
            Rating rating,
            Comment comment,
            DateTime createdOnUtc)
            : base(id)
        {
            ApartmentId = apartmentId;
            BookingId = bookingId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedOnUtc = createdOnUtc;
        }

        private Review()
        {
        }

        public ApartmentId ApartmentId { get; private set; }
        public BookingId BookingId { get; private set; }
        public UserId UserId { get; private set; }
        public Rating Rating { get; private set; }
        public Comment Comment { get; private set; }
        public DateTime CreatedOnUtc { get; private set; }
        public static Result<Review> Create(
            Booking booking, 
            Rating rating, 
            Comment comment, 
            DateTime createdOnUtc)
        {
            if (booking.Status != BookingStatus.Completed)
            {
                return Result.Failure<Review>(ReviewErrors.NotEligible);
            }

            var review = new Review(
                ReviewId.New(),
                booking.ApartmentId,
                booking.Id,
                booking.UserId,
                rating,
                comment,
                createdOnUtc);

            review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

            return review;
        }

        public void Update(Rating rating, Comment comment)
        {
            Rating = rating;
            Comment = comment;
        }
    }
}
