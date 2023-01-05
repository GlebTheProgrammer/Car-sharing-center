using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class VehicleServicePublicApiClient : PublicApiClient, IVehicleServicePublicApiClient
    {
        private const string clientIdentifier = "VehiclesAPI";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public VehicleServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<VehiclesDisplayOnMapResponse?> GetAllApprovedAndPublishedVehiclesMapRepresentation()
        {
            var client = CreateNewClientInstance();

            HttpResponseMessage response =  await client.GetAsync("MapRepresentation");

            return await response.Content.ReadFromJsonAsync<VehiclesDisplayOnMapResponse>();
        }

        protected override HttpClient CreateNewClientInstance()
        {
            return _httpClientFactory.CreateClient(_configuration[$"Clients:{clientIdentifier}:Name"]
                ?? throw new ArgumentNullException(clientIdentifier));
        }
    }
}
