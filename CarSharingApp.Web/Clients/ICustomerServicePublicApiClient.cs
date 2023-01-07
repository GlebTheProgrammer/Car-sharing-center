using CarSharingApp.Application.Contracts.Customer;

namespace CarSharingApp.Web.Clients
{
    public interface ICustomerServicePublicApiClient
    {
        Task<HttpResponseMessage> CreteNewCustomer(CreateCustomerRequest request);
    }
}
