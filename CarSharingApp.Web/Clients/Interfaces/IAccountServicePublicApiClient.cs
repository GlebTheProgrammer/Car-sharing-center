namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IAccountServicePublicApiClient
    {
        Task<HttpResponseMessage> GetCustomerAccountData();
    }
}
