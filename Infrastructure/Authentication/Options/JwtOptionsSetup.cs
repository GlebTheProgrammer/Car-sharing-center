using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CarSharingApp.Infrastructure.Authentication.Options
{
    public sealed class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "JwtBearer";

        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configurations)
        {
            _configuration = configurations;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
