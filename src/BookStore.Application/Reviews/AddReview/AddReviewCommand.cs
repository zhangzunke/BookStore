using BookStore.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Reviews.AddReview
{
    public sealed record AddReviewCommand(Guid BookingId, int Rating, string Comment) : ICommand;
}
