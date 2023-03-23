using CarSharingApp.Application.Contracts.Rental;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IRentalServicePublicApiClient
    {
        Task<HttpResponseMessage> CreateRentalRequest(CreateNewRentalRequest request);
        Task<HttpResponseMessage> FinishRentalRequest(Guid id);
    }
}
