using CarSharingApp.Application.Contracts.Customer;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface ICustomerServicePublicApiClient
    {
        Task<HttpResponseMessage> CreteNewCustomer(CreateCustomerRequest request);
        Task<HttpResponseMessage> GetCreateNewCustomerRequestTemplate();
    }
}
