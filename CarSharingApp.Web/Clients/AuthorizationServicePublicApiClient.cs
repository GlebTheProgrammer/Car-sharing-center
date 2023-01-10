using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class AuthorizationServicePublicApiClient : PublicApiClient, IAuthorizationServicePublicApiClient
    {
        private const string clientIdentifier = "AuthorizationAPI";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthorizationServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> TryAuthorize(AuthorizationRequest request)
        {
            var client = CreateNewClientInstance();

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);
        }

        protected override HttpClient CreateNewClientInstance()
        {
            return _httpClientFactory.CreateClient(_configuration[$"Clients:{clientIdentifier}:Name"]
                ?? throw new ArgumentNullException(clientIdentifier));
        }
    }
}
