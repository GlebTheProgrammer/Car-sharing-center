using CarSharingApp.Application.Contracts.Vehicle;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IVehicleServicePublicApiClient
    {
        Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesMapRepresentation();
        Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesCatalogRepresentation();
        Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesWithFilterCatalogRepresentation(GetVehiclesByCriteriaRequest request);
        Task<HttpResponseMessage> GetAllApprovedAndPublishedNearbyVehiclesMapRepresentation(GetNearbyVehiclesMapRepresentationRequest request);
        Task<HttpResponseMessage> GetVehicleInformationForEdit(Guid id);
        Task<HttpResponseMessage> GetVehicleInformation(Guid id);
        Task<HttpResponseMessage> CreateNewVehicle(CreateVehicleRequest request);
        Task<HttpResponseMessage> DeleteVehicle(string vehicleId);
        Task<HttpResponseMessage> UpdateVehicleStatus(Guid id, UpdateVehicleStatusRequest request);
        Task<HttpResponseMessage> UpdateVehicleInformation(Guid id, UpdateVehicleRequest request);
    }
}
