using CarSharingApp.Application.Contracts.Account;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.SmartEnums;
using CarSharingApp.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    public class AccountsController : ApiController
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<ActionNote> _noteRepository;

        public AccountsController(
            IRepository<Customer> customerRepository, 
            IRepository<Vehicle> vehicleRepository, 
            IRepository<ActionNote> noteRepository)
        {
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _noteRepository = noteRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> AccountData()
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            Guid customerId = Guid.Parse(jwtClaims.Id);

            var customer = await _customerRepository.GetAsync(customerId);
            var notes = await _noteRepository.GetAsyncWithLimit(n => n.ActorId == customerId || n.ActionEntityId == customerId, skip:0, limit:5);
            var vehicles = await _vehicleRepository.GetAllAsync(v => v.CustomerId == customerId);

            return Ok(MapAccountDataResponse(customer, notes, vehicles));
        }

        private AccountDataResponse MapAccountDataResponse(
            Customer customer, 
            IReadOnlyCollection<ActionNote> notes, 
            IReadOnlyCollection<Vehicle> vehicles)
        {
            CustomerToBeDisplayedInAccount customerToBeDisplayedInAccount = new(
                Id: customer.Id,
                FirstName: customer.FirstName,
                LastName: customer.LastName,
                Username: customer.Credentials.Login,
                Image: customer.Profile.Image,
                Description: customer.Profile.Description,
                Email: customer.Credentials.Email,
                Country: customer.Address.Country.ToString(),
                City: customer.Address.City,
                StreetAddress: customer.Address.StreetAddress,
                AptSuiteEtc: customer.Address.AptSuiteEtc,
                PhoneNumber: customer.PhoneNumber);

            List<VehicleToBeDisplayedInAccount> vehiclesToBeDisplayed = new List<VehicleToBeDisplayedInAccount>();
            foreach (var vehicle in vehicles)
            {
                vehiclesToBeDisplayed.Add(new VehicleToBeDisplayedInAccount(
                    Image: vehicle.Image,
                    Name: vehicle.Name,
                    HourlyTariff: vehicle.Tariff.HourlyRentalPrice,
                    DailyTariff: vehicle.Tariff.DailyRentalPrice,
                    PublishedTime: vehicle.PublishedTime,
                    TimesOrdered: vehicle.TimesOrdered,
                    IsConfirmedByAdmin: vehicle.Status.IsConfirmedByAdmin,
                    IsPublished: vehicle.Status.IsPublished,
                    IsOrdered: vehicle.Status.IsOrdered));
            }

            List<NoteToBeDisplayedInAccount> notesToBeDisplayedInAccount = new List<NoteToBeDisplayedInAccount>();
            foreach (var note in notes)
            {
                notesToBeDisplayedInAccount.Add(new NoteToBeDisplayedInAccount(
                    ActorId: note.ActorId,
                    Type: NoteType.FromValue(note.Type)?.ToString(),
                    Note: note.Note,
                    ActionMadeTime: note.ActionMadeTime,
                    ActionEntityId: note.ActionEntityId));
            }



            return new AccountDataResponse(
                customerToBeDisplayedInAccount,
                notesToBeDisplayedInAccount,
                vehiclesToBeDisplayed);
        }
    }
}
