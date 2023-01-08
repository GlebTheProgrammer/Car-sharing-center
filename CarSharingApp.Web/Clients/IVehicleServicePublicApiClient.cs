using CarSharingApp.Application.Contracts.Vehicle;

namespace CarSharingApp.Web.Clients
{
    public interface IVehicleServicePublicApiClient
    {
        Task<HttpResponseMessage> GetAllApprovedAndPublishedVehiclesMapRepresentation();
    }
}
