namespace CarSharingApp.Models.ClientData
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age { get; set; }
        public bool Gender { get; set; }
        public string RegistrationAddress { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
