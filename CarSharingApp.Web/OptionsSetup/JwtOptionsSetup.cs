using CarSharingApp.Login.Authentication;
using Microsoft.Extensions.Options;

namespace CarSharingApp.OptionsSetup
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "Jwt";

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
