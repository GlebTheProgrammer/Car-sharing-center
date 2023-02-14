using Microsoft.Extensions.Logging;

namespace CarSharingApp.Infrastructure.Logging
{
    public static class Extensions
    {
        public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
            loggingBuilder.AddSeq();

            return loggingBuilder;
        }
    }
}
