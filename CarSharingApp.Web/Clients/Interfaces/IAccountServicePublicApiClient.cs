namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IAccountServicePublicApiClient
    {
        Task<HttpResponseMessage> GetCustomerAccountData();
        Task<HttpResponseMessage> GetActionNotesOfTheSpecificType(string type);
        Task<HttpResponseMessage> GetCustomerAccountStatistics();
        Task<HttpResponseMessage> GetCustomerVehiclesAccountRepresentation();
        Task<HttpResponseMessage> GetCustomerRentalsAccountRepresentation();
    }
}
