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

            string requestUri = MyCustomQueryBuilder.Build("NearbyMapRepresentation", request);
            
            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesCatalogRepresentation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("CatalogRepresentation");
        }

        public async Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesMapRepresentation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("MapRepresentation");
        }

        public async Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesWithFilterCatalogRepresentation(GetVehiclesByCriteriaRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = MyCustomQueryBuilder.Build("CriteriaCatalogRepresentation", request);

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetVehicleInformation(Guid id)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = $"Information/{id}";

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> UpdateVehicleStatus(Guid id, UpdateVehicleStatusRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);
            
            string requestUri = $"Status/{id}";

            JsonContent content = JsonContent.Create(request);

            return await client.PutAsync(requestUri, content);
        }
    }
}
