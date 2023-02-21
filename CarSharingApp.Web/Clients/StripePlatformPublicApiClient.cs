using CarSharingApp.Application.Contracts.Payment;
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

        public async Task<HttpResponseMessage> GetStripeSessionUrl(StripePaymentSessionRequest payment, string successUrl, string cancelationUrl)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            List<object> parameters = new() { payment, successUrl, cancelationUrl };

            JsonContent content = JsonContent.Create(parameters);

            return await client.PostAsync(client.BaseAddress, content);
        }
    }
}
