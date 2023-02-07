namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IAzureADPublicApiClient
    {
        public string RequestAuthorizationCode();
        public void RequestAccessToken(string authorizationCode, string state);
        //public void RequestAccessToken(string authorizationCode, string state, string id_token);
    }
}
