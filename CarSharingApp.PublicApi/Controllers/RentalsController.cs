using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.PublicApi.Primitives;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    public class RentalsController : ApiController
    {
        private readonly IRentalsService _rentalsService;
        private readonly IPaymentsService _paymentsService;
        private readonly ILogger _logger;

        public RentalsController(IRentalsService rentalsService, 
                                 IPaymentsService paymentsService, 
                                 ILogger logger)
        {
            _rentalsService = rentalsService;
            _paymentsService = paymentsService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewRental(CreateNewRentalRequest request)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            ErrorOr<Payment> requestToPaymentResult = _paymentsService.From(request);

            if (requestToPaymentResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} provided wrong payment information trying to submit new rental.", jwtClaims.Id);
                return Problem(requestToPaymentResult.Errors);
            }

            ErrorOr<Rental> requestToRentalResult = _rentalsService.From(request, Guid.Parse(jwtClaims.Id));

            if (requestToRentalResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} provided wrong rental information trying to submit new rental.", jwtClaims.Id);
                return Problem(requestToRentalResult.Errors);
            }

            Rental rental = requestToRentalResult.Value;
            Payment payment = requestToPaymentResult.Value;

            await _rentalsService.SubmitNewRental(rental, payment);

            //return CreatedAtAction(
            //    actionName: nameof(GetRental),
            //    routeValues: new { id = rental.Id },
            //    value: MapRentalResponse(rental));

            throw new NotImplementedException();
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetRental(Guid id)
        {
            ErrorOr<Rental> getRentalResult = await _rentalsService.GetRentalAsync(id);

            //return getRentalResult.Match(
            //    rental => Ok(MapRentalResponse(rental)),
            //    errors => Problem(errors));

            throw new NotImplementedException();
        }

    }
}
