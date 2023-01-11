using CarSharingApp.Application.Contracts.Vehicle;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IVehicleServicePublicApiClient
    {
        Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesMapRepresentation();
        Task<HttpResponseMessage> CreateNewVehicle(CreateVehicleRequest request);
    }
}
