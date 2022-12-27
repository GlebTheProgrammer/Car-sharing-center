using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Administrator : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string PhoneNumber { get; private set; }

        public Administrator(Guid id,
            string firstName, 
            string lastName, 
            string username,
            string phoneNumber)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            PhoneNumber = phoneNumber;
        }
    }
}
