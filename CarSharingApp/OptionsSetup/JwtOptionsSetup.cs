using CarSharingApp.Login.Authentication;
using Microsoft.Extensions.Options;

namespace CarSharingApp.OptionsSetup
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "Jwt";

        private readonly IConfiguration _configurations;

        public JwtOptionsSetup(IConfiguration configurations)
        {
            _configurations = configurations;
        }

        public void Configure(JwtOptions options)
        {
            _configurations.GetSection(SectionName).Bind(options);
        }
    }
}
