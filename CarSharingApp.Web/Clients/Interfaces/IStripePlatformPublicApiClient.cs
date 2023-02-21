using CarSharingApp.Application.Contracts.Payment;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IStripePlatformPublicApiClient
    {
        Task<HttpResponseMessage> GetStripeSessionUrl(StripePaymentSessionRequest payment, string successUrl, string cancelationUrl);
    }
}
