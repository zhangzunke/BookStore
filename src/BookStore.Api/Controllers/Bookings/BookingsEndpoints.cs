using Asp.Versioning;
using BookStore.Application.Bookings.CancelBooking;
using BookStore.Application.Bookings.CompleteBooking;
using BookStore.Application.Bookings.ConfirmBooking;
using BookStore.Application.Bookings.GetBooking;
using BookStore.Application.Bookings.GetBookings;
using BookStore.Application.Bookings.RejectBooking;
using BookStore.Application.Bookings.ReserveBooking;
using BookStore.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using static Bogus.DataSets.Name;

namespace BookStore.Api.Controllers.Bookings
{
    public static class BookingsEndpoints
    {
        public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("bookings/{id}", GetBooking)
                .RequireAuthorization()
                .WithName(nameof(GetBooking));

            builder.MapPost("bookings", ReserveBooking)
                .RequireAuthorization();

            builder.MapGet("bookings", GetBookings)
                .RequireAuthorization();

            builder.MapPut("{id}/complete", CompleteBooking)
                .RequireAuthorization();

            builder.MapPut("{id}/cancel", CancelBooking)
                .RequireAuthorization();

            builder.MapPut("{id}/confirm", ConfirmBooking)
                .RequireAuthorization();

            builder.MapPut("{id}/reject", RejectBooking)
                .RequireAuthorization();

            return builder;
        }

        public static async Task<IResult> GetBooking(ISender sender, Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookingQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result) : Results.NotFound();
        }
         
        public static async Task<Results<CreatedAtRoute<Guid>, BadRequest<Error>>> ReserveBooking(
            ISender sender,
            ReserveBookingRequest request,
            CancellationToken cancellationToken)
        {
            var command = new ReserveBookingCommand(
                request.ApartmentId, 
                request.UserId, 
                request.StartDate, 
                request.EndDate);

            var result = await sender.Send(command, cancellationToken);
            if(result.IsFailure)
            {
                return TypedResults.BadRequest(result.Error);
            }
            return TypedResults.CreatedAtRoute(result.Value, nameof(GetBooking), new { id = result.Value });
        }

        public static async Task<IResult> GetBookings(ISender sender, CancellationToken cancellationToken)
        {
            var query = new GetAllBookingsQuery();

            Result<IReadOnlyList<Application.Bookings.GetBookings.BookingResponse>> result =
                await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }

        public static async Task<IResult> CompleteBooking(ISender sender, Guid id, CancellationToken cancellationToken)
        {
            var update = new CompleteBookingCommand(id);

            Result result = await sender.Send(update, cancellationToken);

            if (result.IsFailure)

            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok();
        }

        public static async Task<IResult> CancelBooking(ISender sender, Guid id, CancellationToken cancellationToken)
        {
            var update = new CancelBookingCommand(id);

            Result result = await sender.Send(update, cancellationToken);

            if (result.IsFailure)

            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok();
        }

        public static async Task<IResult> ConfirmBooking(ISender sender, Guid id, CancellationToken cancellationToken)
        {
            var update = new ConfirmBookingCommand(id);

            Result result = await sender.Send(update, cancellationToken);

            if (result.IsFailure)

            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok();
        }

        public static async Task<IResult> RejectBooking(ISender sender, Guid id, CancellationToken cancellationToken)
        {
            var update = new RejectBookingCommand(id);

            Result result = await sender.Send(update, cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok();
        }
    }
}
