using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.Application.Users.LoginUser
{
    internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AccessTokenResponse>
    {
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<Result<AccessTokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _jwtService.GetAccessTokenAsync(request.Email, request.Password, cancellationToken);
            if(result.IsFailure)
            {
                return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
            }
            return new AccessTokenResponse(result.Value);
        }
    }
}
