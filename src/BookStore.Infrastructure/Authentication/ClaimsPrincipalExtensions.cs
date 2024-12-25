﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Authentication
{
    internal static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal? principal)
        {
            var userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(userId, out var parsedUserId) ?
                parsedUserId : throw new ApplicationException("User identifier is unavailable");
        }

        public static string GetIdentityId(this ClaimsPrincipal? principal)
        {
            return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new ApplicationException("User identity is unavailable");
        }
    }
}
