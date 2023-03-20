using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class CustomerServicePublicApiClient : PublicApiClient, ICustomerServicePublicApiClient
    {
        private const string clientIdentifier = "CustomersAPI";

        public CustomerServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> CreateNewCustomer(CreateCustomerRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);
        }

        public async Task<HttpResponseMessage> EditCustomerInformation(UpdateCustomerInfoRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PutAsync("current/information", content);
        }

        public async Task<HttpResponseMessage> EditCustomerPassword(UpdateCustomerPasswordRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PutAsync("current/password", content);
        }

        public async Task<HttpResponseMessage> GetCreateNewCustomerRequestTemplate()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("template");
        }

        public async Task<HttpResponseMessage> GetCustomerInformation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("current/information");
        }
    }
}
