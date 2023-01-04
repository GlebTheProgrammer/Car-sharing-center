using CarSharingApp.Application.Contracts.Vehicle;

namespace CarSharingApp.Web.Clients
{
    public class VehicleServicePublicApiClient : IVehicleServicePublicApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VehicleServicePublicApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<VehiclesDisplayOnMapResponse?> GetAllApprovedAndPublishedVehiclesMapRepresentation()
        {
            var client = _httpClientFactory.CreateClient("vehiclesApi");

            return
                await client.GetFromJsonAsync<VehiclesDisplayOnMapResponse>(
                    "MapRepresentation");
        }
    }
}
