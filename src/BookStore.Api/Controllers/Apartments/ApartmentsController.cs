using Asp.Versioning;
using BookStore.Application.Apartments.AddApartments;
using BookStore.Application.Apartments.SearchApartments;
using BookStore.Application.Apartments.UpdateApartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers.Apartments
{
    [Authorize]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISender _sender;
        public ApartmentsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> SearchApartments(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
        {
            var query = new SearchApartmentsQuery(startDate, endDate);
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateApartment(
            AddApartmentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new AddApartmentCommand(
                request.Name,
                request.Description,
                request.Country,
                request.State,
                request.ZipCode,
                request.City,
                request.Street,
                request.PriceAmount,
                request.PriceCurrency,
                request.CleaningFeeAmount,
                request.CleaningFeeCurrency,
                request.Amenities);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApartment(
            Guid id, 
            UpdateApartmentRequest request, 
            CancellationToken cancellationToken)
        {
            var update = new UpdateApartmentCommand(
                id,
                request.PriceAmount,
                request.PriceAmountCurrency,
                request.CleaningFeeAmount,
                request.CleaningFeeCurrency,
                request.Amenities);

            var result = await _sender.Send(update, cancellationToken);

            if (result.IsFailure) 
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
