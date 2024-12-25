﻿using BookStore.Domain.Users;
using BookStore.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Authorization
{
    internal sealed class CustomClaimsTransformation : IClaimsTransformation
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomClaimsTransformation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity is not { IsAuthenticated : true } ||
                principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
                principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
            {
                return principal;
            }

            using var scope = _serviceProvider.CreateScope();
            var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();
            
            var identityId = principal.GetIdentityId();
            var userRoles = await authorizationService.GetRolesForUserAsync(identityId);

            var claimsIdentity = new ClaimsIdentity();
            
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.UserId.ToString()));
            
            foreach (var role in userRoles.Roles) 
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            principal.AddIdentity(claimsIdentity);

            return principal;
        }
    }
}
