using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class AccountServicePublicApiClient : PublicApiClient, IAccountServicePublicApiClient
    {
        private const string clientIdentifier = "AccountsAPI";

        public AccountServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> GetActionNotesOfTheSpecificType(string type)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = $"customer/notes/{type}";

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetCustomerAccountData()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("customer/information");
        }

        public async Task<HttpResponseMessage> GetCustomerAccountStatistics()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("customer/statistics");
        }

        public async Task<HttpResponseMessage> GetCustomerRentalsAccountRepresentation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("customer/rentals");
        }

        public async Task<HttpResponseMessage> GetCustomerVehiclesAccountRepresentation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("customer/vehicles");
        }
    }
}
