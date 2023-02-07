using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public class AccountServicePublicApiClient : PublicApiClient, IAccountServicePublicApiClient
    {
        private const string clientIdentifier = "AccountsAPI";

        public AccountServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> GetActionNotesOfTheSpecificType(string type)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("AccountNotes" + $"?type={type}");
        }

        public async Task<HttpResponseMessage> GetCustomerAccountData()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("AccountData");
        }

        public async Task<HttpResponseMessage> GetCustomerAccountStatistics()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("AccountStatistics");
        }

        public async Task<HttpResponseMessage> GetCustomerVehiclesAccountRepresentation()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("AccountVehicles");
        }
    }
}
