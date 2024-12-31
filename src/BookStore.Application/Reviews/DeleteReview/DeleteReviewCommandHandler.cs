using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Reviews.DeleteReview
{
    internal sealed class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReviewCommandHandler(
            IReviewRepository reviewRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            Review? review = await _reviewRepository.GetByIdAsync(new ReviewId(request.ReviewId), cancellationToken);

            if(review is null)
            {
                return Result.Failure(ReviewErrors.NotFound);
            }

            _reviewRepository.Remove(review);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
