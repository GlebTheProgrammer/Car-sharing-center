using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class RentalServicePublicApiClient : PublicApiClient, IRentalServicePublicApiClient
    {
        private const string clientIdentifier = "RentalsAPI";

        public RentalServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> CreateRentalRequest(CreateNewRentalRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);
        }

        public async Task<HttpResponseMessage> FinishRentalRequest(Guid id)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = $"{id}/finish";

            return await client.PutAsync(requestUri, null);
        }
    }
}
