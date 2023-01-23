﻿using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class AuthorizationServicePublicApiClient : PublicApiClient, IAuthorizationServicePublicApiClient
    {
        private const string clientIdentifier = "AuthorizationAPI";

        public AuthorizationServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> TryAuthorize(AuthorizationRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);
        }
    }
}