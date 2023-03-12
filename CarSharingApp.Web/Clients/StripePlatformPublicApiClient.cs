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

            string requestUri = $"Session/Details/{sessionId}";

            return await client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetStripeSessionUrl(StripePaymentSessionUrlRequest request)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            string requestUri = MyCustomQueryBuilder.Build("Session", request);

            return await client.GetAsync(requestUri);
        }
    }
}
