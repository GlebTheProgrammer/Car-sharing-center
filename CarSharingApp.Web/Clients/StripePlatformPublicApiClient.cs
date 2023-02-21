﻿using CarSharingApp.Application.Contracts.Payment;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public class StripePlatformPublicApiClient : PublicApiClient, IStripePlatformPublicApiClient
    {
        private const string clientIdentifier = "PaymentsAPI";

        public StripePlatformPublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> GetStripeSessionUrl(StripePaymentSessionUrlRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            JsonContent content = JsonContent.Create(request);

            return await client.PostAsync(client.BaseAddress, content);
        }
    }
}
