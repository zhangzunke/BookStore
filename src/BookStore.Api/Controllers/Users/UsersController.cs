﻿using BookStore.Application.Users.LoginUser;
using BookStore.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
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

    }
}
