using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace CarSharingApp.IdentityServer.Storages
{
    public class CustomersStorage : IClientStore
    {
        private readonly IRepository<Customer> _customersRepository;

        public CustomersStorage(IRepository<Customer> customersRepository)
        {
            _customersRepository = customersRepository;
        }

        /// <summary>
        /// Method takes email from unauthenticated user and try to find matches in the DB repository.
        /// If such customer was found => create a new client instance and then check for client secret 
        /// for coincidence with secret that comes from the client side (f.e. password).
        /// </summary>
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            Customer customer = await _customersRepository.GetAsync(customer => customer.Credentials.Email == clientId);

            if (customer is null)
            {
                return new Client
                {
                    AllowedScopes = { },
                    ClientSecrets= { },
                    AllowedGrantTypes = GrantTypes.ClientCredentials
                };
            }

            return new Client()
            {
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientId = customer.Credentials.Email,
                ClientSecrets =
                {
                    new Secret(customer.Credentials.Password.Sha256())
                },

                AllowedScopes =
                {
                    "accessShareNewVehiclePage"
                },

                Claims =
                {
                    new ClientClaim("guid", customer.Id.ToString()),
                    new ClientClaim("login", customer.Credentials.Login)
                }
            };
        }
    }
}
