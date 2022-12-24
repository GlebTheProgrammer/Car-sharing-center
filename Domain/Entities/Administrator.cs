using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Administrator : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        public Administrator(Guid id,
            string firstName, 
            string lastName, 
            string username,
            string phoneNumber, 
            string email, 
            string login, 
            string password)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            PhoneNumber = phoneNumber;
            Email = email;
            Login = login;
            Password = password;
        }
    }
}
