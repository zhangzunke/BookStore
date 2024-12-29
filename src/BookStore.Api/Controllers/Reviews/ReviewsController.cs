using Asp.Versioning;
using BookStore.Application.Reviews.AddReview;
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
    }
}
