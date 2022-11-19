using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.ClientData.Includes;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RegistrationController(IClientsRepository clientsRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.clientsRepository = clientsRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var user = new ClientRegistrationViewModel();
            return View(user);
        }

        public IActionResult SaveNewAuthorizedUser(ClientRegistrationViewModel clientViewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", clientViewModel);

            var newClient = mapper.Map<ClientModel>(clientViewModel);
            newClient.VehiclesOrdered = 0;
            newClient.VehiclesShared = 0;
            newClient.AccountDescription = "No description yet";
            newClient.UserImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTdmrjoiXGVFEcd1cX9Arb1itXTr2u8EKNpw&usqp=CAU";

            clientsRepository.AddNewClient(newClient);

            httpContextAccessor.HttpContext.Session.SetString("Registered", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
