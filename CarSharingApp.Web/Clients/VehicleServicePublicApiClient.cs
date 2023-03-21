using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Helpers;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class VehicleServicePublicApiClient : PublicApiClient, IVehicleServicePublicApiClient
    {
        private const string clientIdentifier = "VehiclesAPI";

        public VehicleServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> CreateNewVehicle(CreateVehicleRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);
        }

        public async Task<HttpResponseMessage> DeleteVehicle(string vehicleId)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.DeleteAsync(vehicleId);
        }

        public async Task<HttpResponseMessage> GetAllApprovedAndPublishedNearbyVehiclesMapRepresentation(GetNearbyVehiclesMapRepresentationRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = MyCustomQueryBuilder.Build("nearbyMapRepresentation", request);
            
            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesCatalogRepresentation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("catalogRepresentation");
        }

        public async Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesMapRepresentation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("mapRepresentation");
        }

        public async Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesWithFilterCatalogRepresentation(GetVehiclesByCriteriaRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = MyCustomQueryBuilder.Build("criteriaCatalogRepresentation", request);

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetVehicleInformation(Guid id)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = $"{id}/information";

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetVehicleInformationForEdit(Guid id)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = $"{id}/information/edit";

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> UpdateVehicleInformation(Guid id, UpdateVehicleRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = $"{id}";

            JsonContent content = JsonContent.Create(request);

            return await client.PutAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> UpdateVehicleStatus(Guid id, UpdateVehicleStatusRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);
            
            string requestUri = $"{id}/status";

            JsonContent content = JsonContent.Create(request);

            return await client.PutAsync(requestUri, content);
        }
    }
}
