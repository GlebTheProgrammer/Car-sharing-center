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

        [HttpGet("NewCustomerRequestTemplate")]
        [AllowAnonymous]
        public IActionResult GetCreateNewCustomerRequestTemplate()
        {
            var createCustomerRequestTemplate = new CreateCustomerRequest(
                FirstName: string.Empty,
                LastName: string.Empty,
                StreetAddress: string.Empty,
                AptSuiteEtc: string.Empty,
                City: string.Empty,
                Country: string.Empty,
                ZipPostCode: string.Empty,
                PhoneNumber: string.Empty,
                DriverLicenseIdentifier: string.Empty,
                HasAcceptedNewsSharing: false,
                Login: string.Empty,
                Email: string.Empty,
                Password: string.Empty,
                ConfirmPassword: string.Empty,
                HasAcceptedTermsAndConditions: false);

            return Ok(createCustomerRequestTemplate);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            return getCustomerResult.Match(
                customer => Ok(MapCustomerResponse(customer)),
                errors => Problem(errors));
        }
        
        [HttpPut("Info/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerInfo(Guid id, UpdateCustomerInfoRequest request)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                return Problem(getCustomerResult.Errors);
            }

            Customer notUpdatedCustomerYet = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(notUpdatedCustomerYet, request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            ErrorOr<Updated> updateCustomerInfoResult = await _customerService.UpdateCustomerInfoAsync(customer);

            return updateCustomerInfoResult.Match(
                updated => NoContent(),
                errors => Problem(errors));
        }

        [HttpPut("Credentials/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerCredentials(Guid id, UpdateCustomerCredentialsRequest request)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                return Problem(getCustomerResult.Errors);
            }

            Customer notUpdatedCustomerYet = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(notUpdatedCustomerYet, request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            ErrorOr<Updated> updateCustomerResult = await _customerService.UpdateCustomerPasswordAsync(customer);

            return updateCustomerResult.Match(
                updated => NoContent(),
                errors => Problem(errors));
        }

        [HttpPut("Password/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerPassword(Guid id, UpdateCustomerPasswordRequest request)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                return Problem(getCustomerResult.Errors);
            }

            Customer customerWithOldPassword = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(customerWithOldPassword, request);
            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customerWithUpdatedPassword = requestToCustomerResult.Value;

            ErrorOr<Success> checkoutIfOldPasswordMatchResult = await _customerService.CompareCustomerOldPasswordWithExistingOne(id, request.currentPassword);
            if (checkoutIfOldPasswordMatchResult.IsError)
            {
                return Problem(checkoutIfOldPasswordMatchResult.Errors);
            }

            ErrorOr<Updated> updateCustomerResult = await _customerService.UpdateCustomerPasswordAsync(customerWithUpdatedPassword);

            return updateCustomerResult.Match(
                updated => NoContent(),
                errors => Problem(errors));
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
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
                customer.Address,
                customer.PhoneNumber,
                customer.DriverLicenseIdentifier,
                customer.Profile,
                customer.Statistics,
                customer.HasAcceptedNewsSharing,
                customer.Credentials);
        }
    }
}
