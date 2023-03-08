using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Primitives;

namespace CarSharingApp.Web.Clients
{
    public sealed class AzureADPublicApiClient : PublicApiClient, IAzureADPublicApiClient
    {
        private const string clientIdentifier = "AzureActiveDirectoryAPI";

        public AzureADPublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public string RequestAuthorizationCode()
        {
            var client = CreateNewClientInstance(clientIdentifier);

            UriBuilder authorizationCodeRequestUri = new(
                $"{client.BaseAddress}{_configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize?" +
                $"client_id={_configuration["AzureAd:ClientId"]}" +
                "&response_type=code" +
                $"&redirect_uri={_configuration[$"Clients:{clientIdentifier}:AuthorizationCodeRedirectUri"]}" +
                "&response_mode=query" +
                "&scope=https://graph.microsoft.com/User.Read" +
                "&state=12345" +
                "&code_challenge=sHphVC32GXwvqdlnRLkif3QGk4veCWWb4pKY_CyocfY" + // Here is my own code_challenge generated value 
                "&code_challenge_method=S256");

            return authorizationCodeRequestUri.ToString();
        }

        public async void RequestAccessToken(string authorizationCode, string state)
        {
            var client = CreateNewClientInstance(clientIdentifier);

            var requestUri = $"{client.BaseAddress}{_configuration["AzureAd:TenantId"]}/oauth2/v2.0/token";

            var requestBody = new[]
            {
                new KeyValuePair<string, string>("client_id", _configuration["AzureAd:ClientId"] ?? string.Empty),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/User.Read"),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("redirect_uri", _configuration[$"Clients:{clientIdentifier}:AuthorizationCodeRedirectUri"] ?? string.Empty),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code_verifier", "_-WuxYTLEQvLtAZhMMgzz-BjxQcTCwbMHfCPHtTimFqg~IsSFaDsrbu.jhFCQJp8-L9K.~WF2P93PHKR1W-hBqiGHy6Gx2waX2q4Zszc~WpEo_4dLV-b3TFjWNNU7Lld"),
                new KeyValuePair<string, string>("client_secret", "o5X8Q~a.vEube6KcNN0xHagMCrRN29SJ64sbbblA")
            };

            var result = await client.PostAsync(requestUri, new FormUrlEncodedContent(requestBody));

            string responseContent = await result.Content.ReadAsStringAsync();

            return;
        }


        //public string RequestAuthorizationCode()
        //{
        //    var client = CreateNewClientInstance(clientIdentifier);

        //    UriBuilder authorizationCodeRequestUri = new(
        //        $"{client.BaseAddress}{_configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize?" +
        //        $"client_id={_configuration["AzureAd:ClientId"]}" +
        //        "&response_type=code+id_token" +
        //        $"&redirect_uri={_configuration[$"Clients:{clientIdentifier}:AuthorizationCodeRedirectUri"]}" +
        //        "&response_mode=fragment" +
        //        //"&scope=https%3A%2F%2Fgraph.microsoft.com%2Fmail.read" +
        //        "&scope=openid+offline_access+https://graph.microsoft.com/user.read" +
        //        //"&scope=openid%20offline_access%20https%3A%2F%2Fgraph.microsoft.com/.default" +
        //        "&state=12345" +
        //        "&nonce=abcdefg" +
        //        "&code_challenge=sHphVC32GXwvqdlnRLkif3QGk4veCWWb4pKY_CyocfY" + // Here is my own code_challenge generated value 
        //        "&code_challenge_method=S256");

        //    return authorizationCodeRequestUri.ToString();
        //}

        //public async void RequestAccessToken(string authorizationCode, string state, string id_token)
        //{
        //    var client = CreateNewClientInstance(clientIdentifier);

        //    var requestUri = $"{client.BaseAddress}{_configuration["AzureAd:TenantId"]}/oauth2/v2.0/token";

        //    var requestBody = new[]
        //    {
        //        new KeyValuePair<string, string>("client_id", _configuration["AzureAd:ClientId"] ?? string.Empty),
        //        //new KeyValuePair<string, string>("scope", "https%3A%2F%2Fgraph.microsoft.com%2Fmail.read"),
        //        new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/User.Read"),
        //        //new KeyValuePair<string, string>("scope", "openid%20offline_access%20https%3A%2F%2Fgraph.microsoft.com/.default"),
        //        new KeyValuePair<string, string>("code", authorizationCode),
        //        new KeyValuePair<string, string>("redirect_uri", _configuration[$"Clients:{clientIdentifier}:AuthorizationCodeRedirectUri"] ?? string.Empty),
        //        new KeyValuePair<string, string>("grant_type", "authorization_code"),
        //        new KeyValuePair<string, string>("code_verifier", "_-WuxYTLEQvLtAZhMMgzz-BjxQcTCwbMHfCPHtTimFqg~IsSFaDsrbu.jhFCQJp8-L9K.~WF2P93PHKR1W-hBqiGHy6Gx2waX2q4Zszc~WpEo_4dLV-b3TFjWNNU7Lld"),
        //        new KeyValuePair<string, string>("client_secret", "o5X8Q~a.vEube6KcNN0xHagMCrRN29SJ64sbbblA")
        //    };

        //    var result = await client.PostAsync(requestUri, new FormUrlEncodedContent(requestBody));

        //    string responseContent = await result.Content.ReadAsStringAsync();

        //    return;
        //}
    }
}
