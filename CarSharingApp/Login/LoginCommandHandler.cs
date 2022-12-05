using CarSharingApp.Models.ClientData;
using CarSharingApp.Repository.Interfaces;

namespace CarSharingApp.Login
{
    public sealed class LoginCommandHandler
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(IRepositoryManager repositoryManager, IJwtProvider jwtProvider)
        {
            _repositoryManager = repositoryManager;
            _jwtProvider = jwtProvider;
        }

        public string Handle(string email, string password)
        {
            ClientModel? client = _repositoryManager.ClientsRepository.TrySignIn(email, password);

            if (client == null)
                return null;

            string token = _jwtProvider.Generate(client);

            return token;
        }
    }
}
