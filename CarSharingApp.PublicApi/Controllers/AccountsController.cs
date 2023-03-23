using CarSharingApp.Application.Contracts.Account;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.SmartEnums;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.PublicApi.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using CarSharingApp.Application.ServiceErrors;

namespace CarSharingApp.PublicApi.Controllers
{
    [Route("api/accounts")]
    public sealed class AccountsController : ApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IVehicleService _vehicleService;
        private readonly IRentalsService _rentalsService;
        private readonly IPaymentsService _paymentsService;
        private readonly IActionNotesService _notesService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            ICustomerService customerService,
            IVehicleService vehicleService,
            IRentalsService rentalsService,
            IPaymentsService paymentsService,
            IActionNotesService notesService,
            ILogger<AccountsController> logger)
        {
            _customerService = customerService;
            _vehicleService = vehicleService;
            _rentalsService = rentalsService;
            _paymentsService = paymentsService;
            _notesService = notesService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [Route("customer/information")]
        public async Task<IActionResult> AccountData()
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            _logger.LogInformation("Customer with ID: {customerId} asked for account data.", jwtClaims.Id);

            Guid customerId = Guid.Parse(jwtClaims.Id);

            ErrorOr<Customer> customerRequest = await _customerService.GetCustomerAsync(customerId);
            if (customerRequest.IsError)
            {
                return Problem(customerRequest.Errors);
            }
            Customer customer = customerRequest.Value;

            var vehicles = await _vehicleService.GetAllCustomerVehiclesAsync(customerId);

            return Ok(MapAccountDataResponse(customer, vehicles));
        }

        [HttpGet("customer/notes/{type}")]
        [Authorize]
        public async Task<IActionResult> AccountNotes([FromRoute] string type)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            _logger.LogInformation("Customer with ID: {customerId} asked for account notes data.", jwtClaims.Id);

            if (type is null)
                throw new Exception(nameof(type));

            if (type.ToLower().Equals("all"))
            {
                var notes = await _notesService.GetNotesWithLimitConnectedWithAnEntityAsync(Guid.Parse(jwtClaims.Id), skip: 0, limit: 5);
                return Ok(MapAccountActionNotesResponse(notes));
            }
            else if (type.ToLower().Equals("account"))
            {
                var notes = await _notesService.GetCustomerNotesWithLimitConnectedWithAccount(Guid.Parse(jwtClaims.Id), skip: 0, limit: 5);
                return Ok(MapAccountActionNotesResponse(notes));
            }
            else if (type.ToLower().Equals("vehicles"))
            {
                var notes = await _notesService.GetCustomerNotesWithLimitConnectedWithVehicles(Guid.Parse(jwtClaims.Id), skip: 0, limit: 5);
                return Ok(MapAccountActionNotesResponse(notes));
            }
            else if (type.ToLower().Equals("orders"))
            {
                var notes = await _notesService.GetCustomerNotesWithLimitConnectedWithOrders(Guid.Parse(jwtClaims.Id), skip: 0, limit: 5);
                return Ok(MapAccountActionNotesResponse(notes));
            }
            else
                return Problem(new List<Error> { ApplicationErrors.ActionNote.WrongType });
        }

        [HttpGet]
        [Authorize]
        [Route("customer/statistics")]
        public async Task<IActionResult> AccountStatistics()
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            _logger.LogInformation("Customer with ID: {customerId} asked for account statistics data.", jwtClaims.Id);

            Guid customerId = Guid.Parse(jwtClaims.Id);
            ErrorOr<Customer> customerRequest = await _customerService.GetCustomerAsync(customerId);
            if (customerRequest.IsError)
            {
                return Problem(customerRequest.Errors);
            }

            Customer customer = customerRequest.Value;
            var vehicles = await _vehicleService.GetAllCustomerVehiclesAsync(customerId);

            return Ok(MapAccountStatisticsDataResponse(customer, vehicles));
        }

        [HttpGet]
        [Authorize]
        [Route("customer/vehicles")]
        public async Task<IActionResult> AccountVehicles()
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            _logger.LogInformation("Customer with ID: {customerId} asked for account vehicles data.", jwtClaims.Id);

            List<Vehicle> vehicles = await _vehicleService.GetAllCustomerVehiclesAsync(Guid.Parse(jwtClaims.Id));

            return Ok(MapAccountVehiclesDataResponse(vehicles));
        }

        [HttpGet]
        [Authorize]
        [Route("customer/rentals")]
        public async Task<IActionResult> AccountRentals()
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            _logger.LogInformation("Customer with ID: {customerId} asked for account rentals data.", jwtClaims.Id);

            List<Rental> rentals = await _rentalsService.GetAllCustomerRentalsAsync(Guid.Parse(jwtClaims.Id));

            return Ok(await MapAccountRentalsDataResponse(rentals));
        }

        #region Response mapping section

        [NonAction]
        private AccountVehiclesDataResponse MapAccountVehiclesDataResponse(List<Vehicle> vehicles)
        {
            List<AccountVehicleData> accountVehicleDatas = new List<AccountVehicleData>();

            foreach (Vehicle vehicle in vehicles)
            {
                accountVehicleDatas.Add(new AccountVehicleData(
                    VehicleId: vehicle.Id.ToString(),
                    Name: vehicle.Name,
                    Image: vehicle.Image,
                    HourlyPrice: $"{vehicle.Tariff.HourlyRentalPrice}",
                    DailyPrice: $"{vehicle.Tariff.DailyRentalPrice}",
                    PublishedTime: vehicle.PublishedTime,
                    TimesOrdered: vehicle.TimesOrdered,
                    IsOrdered: vehicle.Status.IsOrdered,
                    IsPublished: vehicle.Status.IsPublished,
                    IsConfirmedByAdmin: vehicle.Status.IsConfirmedByAdmin));
            }

            return new AccountVehiclesDataResponse(accountVehicleDatas);
        }

        [NonAction]
        private async Task<AccountRentalsDataResponse> MapAccountRentalsDataResponse(List<Rental> rentals)
        {
            List<AccountRentalData> accountRentalDatas = new List<AccountRentalData>();
            ErrorOr<Vehicle> vehicle;
            ErrorOr<Payment> payment;

            foreach (Rental rental in rentals)
            {
                vehicle = await _vehicleService.GetVehicleAsync(rental.VehicleId);
                payment = await _paymentsService.GetByRentalIdAsync(rental.Id);

                if (vehicle.IsError || payment.IsError)
                    throw new NullReferenceException(nameof(MapAccountRentalsDataResponse));

                accountRentalDatas.Add(new AccountRentalData(
                    RentalId: rental.Id.ToString(),
                    VehicleName: vehicle.Value.Name,
                    VehicleImage: vehicle.Value.Image,
                    Amount: $"{payment.Value.Amount}",
                    RentalMadeDateTime: payment.Value.PaymentDateTime,
                    StartsDateTime: rental.RentalStartsDateTime,
                    TimeLeftInMinutes: Convert.ToInt32((rental.RentalEndsDateTime - rental.RentalStartsDateTime).TotalMinutes),
                    ExpiresDateTime: rental.RentalEndsDateTime,
                    IsActive: rental.IsActive));
            }

            return new AccountRentalsDataResponse(accountRentalDatas);
        }

        [NonAction]
        private AccountStatisticsDataResponse MapAccountStatisticsDataResponse(Customer customer, List<Vehicle> vehicles)
        {
            int sharedVehicles = customer.Statistics.VehiclesShared;
            int rentedVehicles = customer.Statistics.VehiclesOrdered;

            return new AccountStatisticsDataResponse(
            vehicles.Count == 0 ? 0 : vehicles.Where(v => v.Status.IsOrdered).Count() * 100 / vehicles.Count,
            vehicles.Count == 0 ? 0 : vehicles.Where(v => v.Status.IsPublished).Count() * 100 / vehicles.Count,
            vehicles.Count == 0 ? 0 : vehicles.Where(v => !v.Status.IsPublished).Count() * 100 / vehicles.Count,
            sharedVehicles == 0 || rentedVehicles == 0 ? 0 :
                sharedVehicles > rentedVehicles ? rentedVehicles * 100 / sharedVehicles : sharedVehicles * 100 / rentedVehicles);
        }

        [NonAction]
        private AccountDataResponse MapAccountDataResponse(Customer customer, IReadOnlyCollection<Vehicle> vehicles)
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
                PhoneNumber: customer.PhoneNumber,
                VehiclesRented: customer.Statistics.VehiclesOrdered,
                VehiclesShared: customer.Statistics.VehiclesShared);

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

            return new AccountDataResponse(
                customerToBeDisplayedInAccount,
                vehiclesToBeDisplayed);
        }

        [NonAction]
        private AccountActionNotesResponse MapAccountActionNotesResponse(List<ActionNote> notes)
        {
            return new AccountActionNotesResponse(MapListOfNotes(notes));
        }

        [NonAction]
        private List<NoteToBeDisplayedInAccount> MapListOfNotes(IReadOnlyCollection<ActionNote> notes)
        {
            List<NoteToBeDisplayedInAccount> notesToBeDisplayedInAccount = new List<NoteToBeDisplayedInAccount>();
            foreach (var note in notes)
            {
                notesToBeDisplayedInAccount.Add(new NoteToBeDisplayedInAccount(
                    ActorId: note.ActorId,
                    Type: NoteType.FromValue(note.Type)?.ToString() ?? throw new Exception(nameof(note.Type)),
                    Note: note.Note,
                    ActionMadeTime: note.ActionMadeTime,
                    ActionEntityId: note.ActionEntityId));
            }

            return notesToBeDisplayedInAccount;
        }

        #endregion
    }
}
