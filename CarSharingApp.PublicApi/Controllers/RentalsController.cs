using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.Enums;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.PublicApi.Primitives;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    [Route("api/rentals")]
    public sealed class RentalsController : ApiController
    {
        private readonly IRentalsService _rentalsService;
        private readonly IPaymentsService _paymentsService;
        private readonly ICustomerService _customerService;
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(IRentalsService rentalsService, 
                                 IPaymentsService paymentsService,
                                 ILogger<RentalsController> logger,
                                 ICustomerService customerService,
                                 IVehicleService vehicleService)
        {
            _rentalsService = rentalsService;
            _paymentsService = paymentsService;
            _logger = logger;
            _customerService = customerService;
            _vehicleService = vehicleService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewRental([FromBody] CreateNewRentalRequest request)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            // Create new Payment object

            ErrorOr<Payment> requestToPaymentResult = _paymentsService.From(request);
            if (requestToPaymentResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} provided wrong payment information trying to submit new rental.", jwtClaims.Id);
                return Problem(requestToPaymentResult.Errors);
            }
            Payment payment = requestToPaymentResult.Value;

            // Create new Rental object

            ErrorOr<Rental> requestToRentalResult = _rentalsService.From(request, Guid.Parse(jwtClaims.Id));
            if (requestToRentalResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} provided wrong rental information trying to submit new rental.", jwtClaims.Id);
                return Problem(requestToRentalResult.Errors);
            }
            Rental rental = requestToRentalResult.Value;

            // Get rented vehicle Customer object to update his status

            ErrorOr<Customer> requestToRentedCustomerResult = await _customerService.GetCustomerAsync(rental.RentedCustomerId);
            if (requestToRentedCustomerResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} doesn't exist and tried to submit new rental.", rental.RentedCustomerId);
                return Problem(requestToRentedCustomerResult.Errors);
            }
            Customer vehicleRentedCustomer = requestToRentedCustomerResult.Value;

            // Get vehicle owner Customer object to update his status

            ErrorOr<Customer> requestToOwnerCustomerResult = await _customerService.GetCustomerAsync(rental.VehicleOwnerId);
            if (requestToOwnerCustomerResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} tried to order vehicle from customer with ID: {vehicleOwnerId} which doesn't exist.", rental.RentedCustomerId, rental.VehicleOwnerId);
                return Problem(requestToOwnerCustomerResult.Errors);
            }
            Customer vehicleOwnerCustomer = requestToOwnerCustomerResult.Value;

            // Get Vehicle object to update its statistics

            ErrorOr<Vehicle> requestToVehicleResult = await _vehicleService.GetVehicleAsync(rental.VehicleId);
            if (requestToVehicleResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} tried to order vehicle with ID: {vehicleId} which doesn't exist.", rental.RentedCustomerId, rental.VehicleId);
                return Problem(requestToVehicleResult.Errors);
            }
            Vehicle vehicle = requestToVehicleResult.Value;

            // Update both customer models with new statistics
            await _customerService.UpdateCustomerStatisticsAsync(
                UpdateCustomerStatisticsWithAdditionalVehicle(vehicleRentedCustomer, isVehicleOwner: false).Value);
            await _customerService.UpdateCustomerStatisticsAsync(
                UpdateCustomerStatisticsWithAdditionalVehicle(vehicleOwnerCustomer, isVehicleOwner: true).Value);

            // Update vehicle model with new status and timesOrdered value
            await _vehicleService.UpdateVehicleStatusAsync(
                ChangeVehicleStatusAsRented(vehicle).Value);

            // Save new rental
            await _rentalsService.SubmitNewRental(rental, payment);

            return CreatedAtAction(
                actionName: nameof(GetRental),
                routeValues: new { id = rental.Id },
                value: MapRentalResponse(rental));
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetRental([FromRoute] Guid id)
        {
            ErrorOr<Rental> getRentalResult = await _rentalsService.GetRentalAsync(id);

            return getRentalResult.Match(
                rental => Ok(MapRentalResponse(rental)),
                errors => Problem(errors));
        }

        [HttpPut("{id:guid}/finish")]
        [Authorize]
        public async Task<IActionResult> FinishExistingRental([FromRoute] Guid id)
        {
            ErrorOr<Rental> getRentalResult = await _rentalsService.GetRentalAsync(id);
            if (getRentalResult.IsError)
            {
                _logger.LogError("Failed finding rental with ID: {rentalId} when tried to finish it.", id);
                return Problem(getRentalResult.Errors);
            }
            Rental notFinishedRental = getRentalResult.Value;

            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(notFinishedRental.VehicleId);
            if (getVehicleResult.IsError)
            {
                _logger.LogError("Failed finding vehicle with ID: {vehicleId} when tried to finish existing rental.", notFinishedRental.VehicleId.ToString());
                return Problem(getVehicleResult.Errors);
            }
            Vehicle notUpdatedVehicle = getVehicleResult.Value;

            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(notUpdatedVehicle, new UpdateVehicleStatusRequest(
                IsOrdered: false, IsPublished: true, IsConfirmedByAdmin: true));

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }
            Vehicle updatedVehicle = requestToVehicleResult.Value;

            await _rentalsService.FinishExistingRental(id, hasFinishedByTheCustomer: true);
            await _vehicleService.UpdateVehicleStatusAsync(updatedVehicle);

            _logger.LogInformation("Customer with ID: {customerId} has successfully finished rental with ID: {rentalId}.", notFinishedRental.RentedCustomerId, notFinishedRental.Id);

            return NoContent();
        }

        #region Mapping from server models to response models

        [NonAction]
        private RentalResponse MapRentalResponse(Rental rental)
        {
            RentalPaymentResponse payment = new RentalPaymentResponse(
                PaymentId: rental.Payment?.Id.ToString() ?? throw new NotImplementedException(nameof(rental.Payment)),
                StripeTransactionId: rental.Payment.StripeTransactionId,
                PaymentDateTime: rental.Payment.PaymentDateTime,
                Amount: rental.Payment.Amount);

            return new RentalResponse(
                RentalId: rental.Id.ToString(),
                RentedCustomerId: rental.RentedCustomerId.ToString(),
                VehicleOwnerId: rental.VehicleOwnerId.ToString(),
                VehicleId: rental.VehicleId.ToString(),
                Payment: payment,
                RentalStartsDateTime: rental.RentalStartsDateTime,
                RentalEndsDateTime: rental.RentalEndsDateTime,
                IsActive: rental.IsActive);
        }

        #endregion

        #region Mapping on the server side 

        [NonAction]
        private ErrorOr<Vehicle> ChangeVehicleStatusAsRented(Vehicle vehicle)
        {
            return Vehicle.Create(
                vehicle.CustomerId,
                vehicle.Name,
                vehicle.Image,
                vehicle.BriefDescription,
                vehicle.Description,
                vehicle.Tariff.HourlyRentalPrice,
                vehicle.Tariff.DailyRentalPrice,
                vehicle.Location.StreetAddress,
                vehicle.Location.AptSuiteEtc,
                vehicle.Location.City,
                vehicle.Location.Country.Name,
                vehicle.Location.Latitude,
                vehicle.Location.Longitude,
                vehicle.Specifications.ProductionYear,
                vehicle.Specifications.MaxSpeedKph,
                vehicle.Specifications.ExteriorColor.Name,
                vehicle.Specifications.InteriorColor.Name,
                vehicle.Specifications.Drivetrain.Name,
                vehicle.Specifications.FuelType.Name,
                vehicle.Specifications.Transmission.Name,
                vehicle.Specifications.Engine.Name,
                vehicle.Specifications.VIN,
                FlagEnums.GetListFromCategories(vehicle.Categories),
                vehicle.Id,
                vehicle.Status.IsPublished,
                isOrdered: true,
                vehicle.Status.IsConfirmedByAdmin,
                vehicle.TimesOrdered + 1,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered);
        }

        [NonAction]
        private ErrorOr<Customer> UpdateCustomerStatisticsWithAdditionalVehicle(Customer customer, bool isVehicleOwner)
        {
            return Customer.Create(
                customer.FirstName,
                customer.LastName,
                customer.Address.StreetAddress,
                customer.Address.AptSuiteEtc,
                customer.Address.City,
                customer.Address.Country.Name,
                customer.Address.ZipPostCode,
                customer.PhoneNumber,
                customer.DriverLicenseIdentifier,
                customer.HasAcceptedNewsSharing, customer.Credentials.Login,
                customer.Credentials.Email,
                customer.Credentials.Password,
                customer.Id,
                customer.Profile.Description,
                customer.Profile.Image,
                vehiclesOrdered: isVehicleOwner ? customer.Statistics.VehiclesOrdered : customer.Statistics.VehiclesOrdered + 1,
                vehiclesShared: isVehicleOwner ? customer.Statistics.VehiclesShared + 1 : customer.Statistics.VehiclesShared,
                false);
        }

        #endregion
    }
}
