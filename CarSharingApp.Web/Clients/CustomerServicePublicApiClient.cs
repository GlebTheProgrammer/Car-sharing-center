using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Web.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarSharingApp.Web.Clients
{
    public class CustomerServicePublicApiClient : PublicApiClient, ICustomerServicePublicApiClient
    {
        private const string clientIdentifier = "CustomersAPI";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CustomerServicePublicApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<CustomerResponse?> CreteNewCustomer(CreateCustomerRequest request)
        {
            var client = CreateNewClientInstance();

            JsonContent content = JsonContent.Create(request);

            HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);

            if (!response.IsSuccessStatusCode)
            {
                var obj = new ModelStateDictionary();

                var resu = await response.Content.ReadAsStringAsync();

                obj = System.Text.Json.JsonSerializer.Deserialize<ModelStateDictionary>(resu);
                Console.WriteLine();
            }

            return await response.Content.ReadFromJsonAsync<CustomerResponse>();
        }

        protected override HttpClient CreateNewClientInstance()
        {
            return _httpClientFactory.CreateClient(_configuration[$"Clients:{clientIdentifier}:Name"]
                ?? throw new ArgumentNullException(clientIdentifier));
        }

        public class ExceptionResponse
        {
            public string type { get; set; }
            public string title { get; set; }
            public string status { get; set; }
            public string traceId { get; set; }
            public List<object> errors { get; set; }
        }
    }
}
