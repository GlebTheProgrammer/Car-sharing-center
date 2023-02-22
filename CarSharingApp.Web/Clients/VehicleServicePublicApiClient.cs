using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
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

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync("NearbyVehiclesMapRepresentation", content);
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

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync("CriteriaCatalogRepresentation", content);
        }

        public async Task<HttpResponseMessage> GetVehicleInformation(Guid id)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("Information" + $"?id={id}");
        }

        public async Task<HttpResponseMessage> UpdateVehicleStatus(UpdateVehicleStatusRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PutAsync("UpdateVehicleStatus", content);
        }
    }
}
