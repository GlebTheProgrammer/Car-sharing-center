using CarSharingApp.Infrastructure.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace CarSharingApp.PublicApi.Primitives
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.All(e => e.Type == ErrorType.Validation))
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (Error error in errors)
                {
                    modelStateDictionary.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem(modelStateDictionary);
            }

            if (errors.Any(e => e.Type == ErrorType.Unexpected))
            {
                return Problem();
            }

            var firstError = errors[0];

            if (firstError.NumericType is 403)
                return Problem(
                 statusCode: StatusCodes.Status403Forbidden,
                 title: firstError.Description);


            var statusCode = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(
                statusCode: statusCode,
                title: firstError.Description);
        }

        protected JwtClaims? GetJwtClaims()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var userClaims = identity.Claims;

                var jwtClaims = new JwtClaims
                {
                    Id = userClaims.FirstOrDefault(c => c.Properties.Values.Contains(JwtRegisteredClaimNames.Sub))?.Value ?? "",
                    Login = userClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? "",
                    Email = userClaims.FirstOrDefault(c => c.Properties.Values.Contains(JwtRegisteredClaimNames.Email))?.Value ?? "",
                    Role = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? ""
                };

                if (jwtClaims.GetType()
                        .GetProperties()
                        .Select(p => p.GetValue(jwtClaims))
                        .Any(value => value == null || value.ToString()?.Length == 0))
                {
                    return null;
                }
                else
                {
                    return jwtClaims;
                }
            }

            return null;
        }
    }
}
