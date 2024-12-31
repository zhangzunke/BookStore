using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Reviews.UpdateReview
{
    internal sealed class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            Review? review = await _reviewRepository.GetByIdAsync(new ReviewId(request.ReviewId), cancellationToken);

            if (review is null)
            {
                return Result.Failure(ReviewErrors.NotFound);
            }

            Result<Rating> ratingResult = Rating.Create(request.Rating);

            if (ratingResult.IsFailure)
            {
                return Result.Failure(ratingResult.Error);
            }

            review.Update(ratingResult.Value, new Comment(request.Comment));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
