using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Abstractions;

namespace BookStore.Domain.Reviews
{
    public static class ReviewErrors
    {
        public static readonly Error NotEligible = new(
            "Review.NotEligible",
            "The review is not eligible because the booking is not yet completed");
    }
}
