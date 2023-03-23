using CarSharingApp.Application.Contracts.Payment;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IStripePlatformPublicApiClient
    {
        Task<HttpResponseMessage> GetStripeSessionUrl(StripePaymentSessionUrlRequest request);
        Task<HttpResponseMessage> GetStripePaymentDetails(string sessionId);
    }
}
