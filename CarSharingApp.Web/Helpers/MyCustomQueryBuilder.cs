using System.Text;

namespace CarSharingApp.Web.Helpers
{
    public static class MyCustomQueryBuilder
    {
        public static string Build<T>(string publicApiEndpointName, T requestModel)
        {
            StringBuilder queryBuilder = new StringBuilder($"{publicApiEndpointName}?");

            foreach (var property in requestModel?.GetType().GetProperties() ?? throw new NullReferenceException(nameof(requestModel)))
            {
                queryBuilder.Append($"{property.Name}={property.GetValue(requestModel)}&");
            }

            queryBuilder.Remove(queryBuilder.Length - 1, 1);

            return queryBuilder.ToString();
        }
    }
}
