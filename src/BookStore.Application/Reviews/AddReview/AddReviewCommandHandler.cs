using BookStore.Application.Abstractions.Clock;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Bookings;
using BookStore.Domain.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Reviews.AddReview
{
    internal sealed class AddReviewCommandHandler : ICommandHandler<AddReviewCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AddReviewCommandHandler(
            IBookingRepository bookingRepository,
            IReviewRepository reviewRepository,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _bookingRepository = bookingRepository;
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
            if (booking is null)
            {
                return Result.Failure(BookingErrors.NotFound);
            }
            var ratingResult = Rating.Create(request.Rating);
            if (ratingResult.IsFailure)
            {
                return Result.Failure(ratingResult.Error);
            }

            var reviewResult = Review.Create(
                booking,
                ratingResult.Value,
                new Comment(request.Comment),
                _dateTimeProvider.UtcNow);

            if (reviewResult.IsFailure)
            {
                return Result.Failure(reviewResult.Error);
            }

            _reviewRepository.Add(reviewResult.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
