using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    public sealed class CustomersController : ApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomer(CreateCustomerRequest request)
        {
            ErrorOr<Customer> requestToCustomerResult = _customerService.From(request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            ErrorOr<Created> createCustomerResult = await _customerService.CreateCustomerAsync(customer);

            return createCustomerResult.Match(
                created => CreatedAtAction(
                        actionName: nameof(GetCustomer),
                        routeValues: new { id = customer.Id },
                        value: MapCustomerResponse(customer)),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Administrator, Customer")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            if (!IsRequestAllowed(id)) 
            {
                return Forbid();
            }

            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            return getCustomerResult.Match(
                customer => Ok(MapCustomerResponse(customer)),
                errors => Problem(errors));
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrator, Customer")]
        public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerRequest request)
        {
            if (!IsRequestAllowed(id))
            {
                return Forbid();
            }

            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                return Problem(getCustomerResult.Errors);
            }

            Customer notUpdatedCustomerYet = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(id, request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            ErrorOr<Updated> updateCustomerResult = await _customerService.UpdateCustomerAsync(customer);

            return updateCustomerResult.Match(
                updated => NoContent(),
                errors => Problem(errors));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator, Customer")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            if (!IsRequestAllowed(id))
            {
                return Forbid();
            }

            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                return Problem(getCustomerResult.Errors);
            }

            Customer notDeletedCustomerYet = getCustomerResult.Value;

            ErrorOr<Deleted> deleteCustomerResult = await _customerService.DeleteCustomerAsync(id);

            return deleteCustomerResult.Match(
                deleted => NoContent(),
                errors => Problem(errors));
        }

        private static CustomerResponse MapCustomerResponse(Customer customer)
        {
            return new CustomerResponse(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Country,
                customer.City,
                customer.Address,
                customer.PhoneNumber,
                customer.DriverLicenseIdentifier,
                customer.ProfileDescription,
                customer.ProfileImage,
                customer.Postcode,
                customer.VehiclesOrdered,
                customer.VehiclesShared,
                customer.HasAcceptedNewsSharing,
                customer.IsOnline,
                customer.Credentials);
        }

        private bool IsRequestAllowed(Guid requestedId)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims == null || (jwtClaims.Id != requestedId.ToString() && jwtClaims.Role != "Administrator"))
            {
                return false;
            }

            return true;
        }
    }
}
