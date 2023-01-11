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

        public async Task<HttpResponseMessage> CreteNewCustomer(CreateCustomerRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);

        }
    }
}
