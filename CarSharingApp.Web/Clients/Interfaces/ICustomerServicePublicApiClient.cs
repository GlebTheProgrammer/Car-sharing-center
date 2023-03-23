using CarSharingApp.Application.Contracts.Customer;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface ICustomerServicePublicApiClient
    {
        Task<HttpResponseMessage> CreateNewCustomer(CreateCustomerRequest request);
        Task<HttpResponseMessage> GetCreateNewCustomerRequestTemplate();
        Task<HttpResponseMessage> GetCustomerInformation();
        Task<HttpResponseMessage> EditCustomerInformation(UpdateCustomerInfoRequest request);
        Task<HttpResponseMessage> EditCustomerPassword(UpdateCustomerPasswordRequest request);
    }
}
