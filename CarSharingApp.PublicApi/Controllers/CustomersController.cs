using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    public class CustomersController : ApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
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
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            return getCustomerResult.Match(
                customer => Ok(MapCustomerResponse(customer)),
                errors => Problem(errors));

        }
        
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerRequest request)
        {
            ErrorOr<Customer> requestToCustomerResult = Error.Failure(); //_customerService.From(id, Credentials, request);

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
        [Authorize]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
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
    }
}
