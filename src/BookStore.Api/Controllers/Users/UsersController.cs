﻿using Asp.Versioning;
using BookStore.Application.Users.GetLoggedInUser;
using BookStore.Application.Users.LoginUser;
using BookStore.Application.Users.RegisterUser;
using BookStore.Application.Users.UpdateUser;
using BookStore.Domain.Abstractions;
using BookStore.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers.Users
{
    [ApiVersion(ApiVersions.V1)]
    [ApiVersion(ApiVersions.V2)]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        [HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
        {
            var query = new GetLoggedInUserQuery();
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await _sender.Send(command, cancellationToken);
            if(result.IsFailure)
            {
                return Unauthorized(result.Error);
            }
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password);

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("profile")]
        [HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserProfileCommand(
                    request.UserId,
                    request.FirstName,
                    request.LastName);

            Result result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
