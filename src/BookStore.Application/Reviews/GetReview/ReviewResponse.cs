using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Reviews.GetReview
{
    public sealed class ReviewResponse
    {
        public Guid Id { get; init; }

        public Guid ApartmentId { get; init; }

        public Guid BookingId { get; init; }

        public Guid UserId { get; init; }

        public int Rating { get; init; }

        public string Comment { get; init; }

        public DateTime CreatedOnUtc { get; init; }
    }
}
