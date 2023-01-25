using CarSharingApp.Domain.Entities;
using CarSharingApp.Infrastructure.Authentication.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarSharingApp.Infrastructure.Authentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string Generate(Customer customer)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, customer.Credentials.Login),
                new Claim(JwtRegisteredClaimNames.Email, customer.Credentials.Email),
                new Claim(ClaimTypes.Role, "Customer")
            };

            return ConfigureJWToken(claims);
        }

        public string Generate(Administrator administrator)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, administrator.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, administrator.Login),
                new Claim(JwtRegisteredClaimNames.Email, administrator.Email),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            return ConfigureJWToken(claims);
        }

        private string ConfigureJWToken(Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
