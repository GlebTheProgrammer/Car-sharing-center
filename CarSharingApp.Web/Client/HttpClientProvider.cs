using System.Net.Http.Headers;

namespace CarSharingApp.Web.Client
{
    public static class HttpClientProvider
    {
        public static HttpClient Configure()
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            return client;
        }

        public static HttpClient Configure(string jwToken)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);

            return client;
        }
    }
}
