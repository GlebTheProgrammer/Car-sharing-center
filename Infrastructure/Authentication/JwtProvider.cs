using CarSharingApp.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarSharingApp.Domain.Constants;

namespace CarSharingApp.Infrastructure.Authentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        public string Generate(Customer customer)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, customer.Credentials.Login),
                new Claim(JwtRegisteredClaimNames.Email, customer.Credentials.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtAuthorizationConstants.SECRET_KEY));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                JwtAuthorizationConstants.ISSUER,
                JwtAuthorizationConstants.AUDIENCE,
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
