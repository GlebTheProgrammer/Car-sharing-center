using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.PublicApi.Primitives;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    [Route("api/customers")]
    public sealed class CustomersController : ApiController
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, 
                                   ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            ErrorOr<Customer> requestToCustomerResult = _customerService.From(request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            ErrorOr<Customer> createCustomerResult = await _customerService.CreateCustomerAsync(customer);
            
            if (!createCustomerResult.IsError)
                _logger.LogInformation("New customer with ID: {registeredCustomerId} has been registered.", customer.Id);

            return createCustomerResult.Match(
                created => CreatedAtAction(
                        actionName: nameof(GetCustomer),
                        routeValues: new { id = customer.Id },
                        value: MapCustomerResponse(customer)),
                errors => Problem(errors));
        }

        [HttpGet("template")]
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
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            return getCustomerResult.Match(
                customer => Ok(MapCustomerResponse(customer)),
                errors => Problem(errors));
        }

        [HttpGet]
        [Authorize]
        [Route("current/information")]
        public async Task<IActionResult> GetCustomerInformation()
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(Guid.Parse(jwtClaims.Id));

            return getCustomerResult.Match(
                customer => Ok(MapCustomerDataResponse(customer)),
                errors => Problem(errors));
        }
        
        [HttpPut("{id:guid}/information")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerInfo([FromRoute] Guid id, 
                                                            [FromBody] UpdateCustomerInfoRequest request)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                _logger.LogInformation("Failed finding customer with ID: {customerId} when trying to update data.", id);
                return Problem(getCustomerResult.Errors);
            }

            Customer notUpdatedCustomerYet = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(notUpdatedCustomerYet, request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            await _customerService.UpdateCustomerInfoAsync(customer);

            _logger.LogInformation("Customer with ID: {registeredCustomerId} has successfully updated his data.", customer.Id);

            return NoContent();
        }

        [HttpPut("current/information")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentCustomerInfo([FromBody] UpdateCustomerInfoRequest request)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(Guid.Parse(jwtClaims.Id));

            if (getCustomerResult.IsError)
            {
                _logger.LogInformation("Failed finding customer with ID: {customerId} when trying to update data.", jwtClaims.Id);
                return Problem(getCustomerResult.Errors);
            }

            Customer notUpdatedCustomerYet = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(notUpdatedCustomerYet, request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            await _customerService.UpdateCustomerInfoAsync(customer);

            _logger.LogInformation("Customer with ID: {registeredCustomerId} has successfully updated his data.", customer.Id);

            return NoContent();
        }

        [HttpPut("{id:guid}/credentials")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerCredentials([FromRoute] Guid id, 
                                                                   [FromBody] UpdateCustomerCredentialsRequest request)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                _logger.LogInformation("Failed finding customer with ID: {customerId} when trying to update his credentials (email and(or) login).", id);
                return Problem(getCustomerResult.Errors);
            }

            Customer notUpdatedCustomerYet = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(notUpdatedCustomerYet, request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customer = requestToCustomerResult.Value;

            await _customerService.UpdateCustomerPasswordAsync(customer);

            _logger.LogInformation("Customer with ID: {registeredCustomerId} has successfully changed his credentials (email and(or) login).", customer.Id);

            return NoContent();
        }

        [HttpPut("{id:guid}/password")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerPassword([FromRoute] Guid id, 
                                                                [FromBody] UpdateCustomerPasswordRequest request)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                _logger.LogInformation("Failed finding customer with ID: {customerId} when trying to update password.", id);
                return Problem(getCustomerResult.Errors);
            }

            Customer customerWithOldPassword = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(customerWithOldPassword, request);
            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customerWithUpdatedPassword = requestToCustomerResult.Value;

            ErrorOr<string> checkoutIfOldPasswordMatchResult = await _customerService.CompareCustomerOldPasswordWithExistingOne(id, request.currentPassword);
            if (checkoutIfOldPasswordMatchResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {registeredCustomerId} has entered wrong previous password trying to change it.", id);
                return Problem(checkoutIfOldPasswordMatchResult.Errors);
            }

            await _customerService.UpdateCustomerPasswordAsync(customerWithUpdatedPassword);

            _logger.LogInformation("Customer with ID: {registeredCustomerId} has successfully changed his password.", id);

            return NoContent();
        }

        [HttpPut("current/password")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentCustomerPassword([FromBody] UpdateCustomerPasswordRequest request)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(Guid.Parse(jwtClaims.Id));

            if (getCustomerResult.IsError)
            {
                _logger.LogInformation("Failed finding customer with ID: {customerId} when trying to update password.", jwtClaims.Id);
                return Problem(getCustomerResult.Errors);
            }

            Customer customerWithOldPassword = getCustomerResult.Value;

            ErrorOr<Customer> requestToCustomerResult = _customerService.From(customerWithOldPassword, request);
            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            Customer customerWithUpdatedPassword = requestToCustomerResult.Value;

            await _customerService.UpdateCustomerPasswordAsync(customerWithUpdatedPassword);

            _logger.LogInformation("Customer with ID: {registeredCustomerId} has successfully changed his password.", jwtClaims.Id);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(id);

            if (getCustomerResult.IsError)
            {
                _logger.LogInformation("Failed finding customer with ID: {customerId} when deleting.", id);
                return Problem(getCustomerResult.Errors);
            }

            await _customerService.DeleteCustomerAsync(id);

            _logger.LogInformation("Customer with ID: {registeredCustomerId} has successfully been deleted.", id);

            return NoContent();
        }

        #region Response mapping section

        [NonAction]
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

        [NonAction]
        private static CustomerDataResponse MapCustomerDataResponse(Customer customer)
        {
            return new CustomerDataResponse(
                customer.FirstName,
                customer.LastName,
                customer.PhoneNumber,
                customer.DriverLicenseIdentifier,
                customer.Profile.Description,
                customer.Profile.Image,
                customer.Credentials.Login,
                customer.Credentials.Email,
                customer.HasAcceptedNewsSharing);
        }

        #endregion
    }
}
