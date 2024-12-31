using Asp.Versioning;
using BookStore.Application.Reviews.AddReview;
using BookStore.Application.Reviews.DeleteReview;
using BookStore.Application.Reviews.GetReview;
using BookStore.Application.Reviews.UpdateReview;
using BookStore.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers.Reviews
{
    [Authorize]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ISender _sender;

        public ReviewsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(CancellationToken cancellationToken)
        {
            var query = new GetAllReviewsQuery();

            Result<IReadOnlyList<ReviewResponse>> result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewRequest request, CancellationToken cancellationToken)
        {
            var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(Guid id, UpdateReviewRequest request, CancellationToken cancellationToken)
        {
            var update = new UpdateReviewCommand(id, request.Rating, request.Comment);

            Result result = await _sender.Send(update, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id, CancellationToken cancellationToken)
        {
            var delete = new DeleteReviewCommand(id);

            Result result = await _sender.Send(delete, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
