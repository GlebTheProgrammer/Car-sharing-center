using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Domain.ValueObjects;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;
using IdentityModel.Client;

namespace CarSharingApp.Web.Clients
{
    public sealed class DuendeIdentityServerPublicApiClient : PublicApiClient, IAuthorizationServicePublicApiClient
    {
        private const string clientIdentifier = "DuendeIdentityServerAPI";

        public DuendeIdentityServerPublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> TryAuthorize(AuthorizationRequest request)
        {
            HttpClient client = new HttpClient();

            DiscoveryDocumentResponse document = await client.GetDiscoveryDocumentAsync(_configuration[$"Clients:{clientIdentifier}:BaseAddress"]);

            if (document.IsError)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                    Content = new StringContent("Server is down for maintenance or is overloaded.")
                };
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = document.TokenEndpoint,

                ClientId = "client_id",
                ClientSecret = "client_secret",
                Scope = "PublicAPI.CutomerEndpoints"
            });

            if (tokenResponse.IsError)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Content = new StringContent("Цrong username and/or password.")
                };
            }

            return new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(tokenResponse.AccessToken)
            };
        }
    }
}
