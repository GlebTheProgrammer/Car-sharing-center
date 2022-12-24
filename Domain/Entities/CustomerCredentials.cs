using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class CustomerCredentials : Entity
    {
        public string Login { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        public Guid CustomerId { get; private set; } // 1:1
        public Customer? Customer { get; private set; }

        public CustomerCredentials(Guid id,
            Guid customerId, 
            string login, 
            string email, 
            string password)
            : base(id)
        {
            CustomerId = customerId;
            Login = login;
            Email = email;
            Password = password;
        }
    }
}
