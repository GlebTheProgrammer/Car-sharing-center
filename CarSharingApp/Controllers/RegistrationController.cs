using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegistrationController(IRepositoryManager repositoryManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var user = new ClientRegistrationViewModel();
            return View(user);
        }

        public IActionResult Register(ClientRegistrationViewModel clientViewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", clientViewModel);

            var newClient = _mapper.Map<ClientModel>(clientViewModel);
            newClient.VehiclesOrdered = 0;
            newClient.VehiclesShared = 0;
            newClient.AccountDescription = "No description yet";
            newClient.UserImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTdmrjoiXGVFEcd1cX9Arb1itXTr2u8EKNpw&usqp=CAU";

            _repositoryManager.ClientsRepository.AddNewClient(newClient);

            _httpContextAccessor.HttpContext.Session.SetString("Registered", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
