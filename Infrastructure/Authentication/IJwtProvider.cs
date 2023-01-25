using CarSharingApp.Domain.Entities;

namespace CarSharingApp.Infrastructure.Authentication
{
    public interface IJwtProvider
    {
        string Generate(Customer customer);
    }
}
