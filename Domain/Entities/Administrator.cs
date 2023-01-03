using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Administrator : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Login { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string PhoneNumber { get; private set; }

        public Administrator(
            Guid id,
            string firstName, 
            string lastName, 
            string login,
            string email,
            string password,
            string phoneNumber)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }
    }
}
