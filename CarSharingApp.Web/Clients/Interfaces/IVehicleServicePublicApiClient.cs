using CarSharingApp.Application.Contracts.Vehicle;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IVehicleServicePublicApiClient
    {
        Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesMapRepresentation();
        Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesCatalogRepresentation();
        Task<HttpResponseMessage> CreateNewVehicle(CreateVehicleRequest request);
        Task<HttpResponseMessage> DeleteVehicle(string vehicleId);
        Task<HttpResponseMessage> UpdateVehicleStatus(UpdateVehicleStatusRequest request);
    }
}
