using CarSharingApp.Models.MongoView;
using CarSharingApp.Repository.MongoDbRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CarSharingApp.Controllers
{
    [Authorize]
    public class EditCustomerDataController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public EditCustomerDataController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            string customerId = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("JWToken")).Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

            CustomerEditModel customerEditModel = await _mongoDbService.GetCustomer_EditRepresentation(customerId);

            return View(customerEditModel);
        }

        public async Task<IActionResult> EditCustomerData(CustomerEditModel customerEditModel)
        {
            if (!ModelState.IsValid)
                return View("Index", customerEditModel);

            await _mongoDbService.EditCustomerData(customerEditModel);

            HttpContext.Session.SetString("ChangedAccountData", "true");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword)
        {
            string userId = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("JWToken")).Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

            await _mongoDbService.ChangePassword(userId, newPassword);

            HttpContext.Session.SetString("ChangedPassword", "true");

            return RedirectToAction("Index");
        }
    }
}
