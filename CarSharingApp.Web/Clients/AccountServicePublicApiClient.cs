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

        public async Task<HttpResponseMessage> GetCustomerAccountData()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            return await client.GetAsync("AccountData");
        }
    }
}
