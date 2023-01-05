namespace CarSharingApp.Web.Primitives
{
    public abstract class PublicApiClient
    {
        protected abstract HttpClient CreateNewClientInstance();
    }
}
