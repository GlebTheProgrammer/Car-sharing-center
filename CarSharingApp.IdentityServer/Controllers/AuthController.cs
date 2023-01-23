using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.IdentityServer.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<Client> _userManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Customer> _customersRepository;

        public AuthController(/*SignInManager<IdentityUser> signInManager,*/
                              UserManager<Client> userManager,
                              IServiceProvider serviceProvider,
                              IRepository<Customer> customerRepository)
        {
            //_signInManager = signInManager;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _customersRepository = customerRepository;
        }

        [Route("[action]")]
        public IActionResult SignIn(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SignIn(Application.Contracts.Authorization.AuthorizationRequest request)
        {
            Credentials userCredentials = Credentials.CreateForAuthorization(request.EmailOrLogin, request.EmailOrLogin, request.Password);

            Customer? customer = await _customersRepository.GetAsync(c => (c.Credentials.Email == userCredentials.Email ||
            c.Credentials.Login == userCredentials.Login) && c.Credentials.Password == userCredentials.Password);
            
            if (customer is null)
            {
                ModelState.AddModelError("Password", "Customer with provided credentials doesn't exist.");
                return View(request);
            }

            //IdentityServerConfigurations.CreateNewIdentityUser(_serviceProvider, customer);

            var identityUser = await _userManager.FindByIdAsync("hello");

            //if (identityUser is null)
            //{
            //    ModelState.AddModelError("Password", "Something went wrong. Please try again later.");
            //    return View(request);
            //}

            //var signinResult = await _signInManager.PasswordSignInAsync(identityUser, customer.Credentials.Password, false, false);

            if (/*signinResult.Succeeded*/true)
            {
                return Redirect(request.ReturnUrl);
            }

            //ModelState.AddModelError("Password", "Something went wrong. Please try again later.");
            //return View(request);
        }
    }
}
