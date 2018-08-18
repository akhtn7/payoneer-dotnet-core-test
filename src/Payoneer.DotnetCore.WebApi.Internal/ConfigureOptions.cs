using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payoneer.DotnetCore.Net.Rest;

namespace Payoneer.DotnetCore.WebApi.Internal
{
    internal static class OptionsConfiguration
    {
        public static IServiceCollection ConfigureOptions(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ExternalPaymentServiceOptions>(configuration.GetSection("ExternalPaymentService"));

            return services;
        }
    }
}