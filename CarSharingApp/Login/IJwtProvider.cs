using CarSharingApp.Models.Mongo;

namespace CarSharingApp.Login
{
    public interface IJwtProvider
    {
        string Generate(Customer customer, Credentials credentials);
    }
}
