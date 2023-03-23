using CarSharingApp.Application.Contracts.Payment;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Helpers;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class StripePlatformPublicApiClient : PublicApiClient, IStripePlatformPublicApiClient
    {
        private const string clientIdentifier = "PaymentsAPI";

        public StripePlatformPublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<HttpResponseMessage> GetStripePaymentDetails(string sessionId)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = $"session/{sessionId}/details";

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetStripeSessionUrl(StripePaymentSessionUrlRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = MyCustomQueryBuilder.Build("session", request);

            return await client.GetAsync(requestUri);
        }
    }
}
