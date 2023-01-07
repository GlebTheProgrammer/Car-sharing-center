using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public class CustomerServicePublicApiClient : PublicApiClient, ICustomerServicePublicApiClient
    {
        private const string clientIdentifier = "CustomersAPI";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CustomerServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> CreteNewCustomer(CreateCustomerRequest request)
        {
            var client = CreateNewClientInstance();

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);

        }

        protected override HttpClient CreateNewClientInstance()
        {
            return _httpClientFactory.CreateClient(_configuration[$"Clients:{clientIdentifier}:Name"]
                ?? throw new ArgumentNullException(clientIdentifier));
        }
    }
}
