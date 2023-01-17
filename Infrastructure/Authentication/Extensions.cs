using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

namespace CarSharingApp.Infrastructure.Authentication
{
    public static class Extensions 
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["JwtBearer:SecretKey"] 
                ?? throw new ArgumentNullException("SecretKey"));

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", opt =>
            {
                opt.Authority = "https://localhost:5001";

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            return services;
        }
    }
}
