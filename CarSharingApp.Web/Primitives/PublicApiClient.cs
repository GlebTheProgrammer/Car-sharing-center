using System.Net.Http.Headers;

namespace CarSharingApp.Web.Primitives
{
    public abstract class PublicApiClient
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public PublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected HttpClient CreateNewClientInstance(string clientIdentifier)
        {
            HttpClient client = _httpClientFactory.CreateClient(_configuration[$"Clients:{clientIdentifier}:Name"]
                ?? throw new ArgumentNullException(clientIdentifier));

            string? jwToken = _httpContextAccessor?.HttpContext?.Session.GetString("JWToken");

            if (jwToken is not null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);

            return client;
        }
    }
}
