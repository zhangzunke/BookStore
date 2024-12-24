﻿using BookStore.Application.Abstractions.Authentication;
using BookStore.Domain.Users;
using BookStore.Infrastructure.Authentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Authentication
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private const string PasswordCredentialType = "password";

        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            var userRepresentationModel = UserRepresentationModel.FromUser(user);
            userRepresentationModel.Credentials =
            [
                new()
                {
                    Value = password,
                    Temporary = false,
                    Type = PasswordCredentialType
                }
            ];

            var response = await _httpClient.PostAsJsonAsync(
               "users",
               userRepresentationModel,
               cancellationToken);

            return ExtractIdentityIdFromLocationHeader(response);
        }

        private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
        {
            const string usersSegmentName = "users/";
            var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;
            if (locationHeader is null)
            {
                throw new InvalidOperationException("Location header can't be null");
            }
            var userSegmentValueIndex = locationHeader.IndexOf(usersSegmentName, StringComparison.InvariantCultureIgnoreCase);
            var userIdentityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);
            return userIdentityId;
        }
    }
}
