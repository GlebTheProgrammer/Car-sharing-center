using CarSharingApp.Models.ClientData;

namespace CarSharingApp.Login
{
    public interface IJwtProvider
    {
        string Generate(ClientModel client);
    }
}
