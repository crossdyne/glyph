using System.Reflection;
using Crossdyne.Security.Abstractions;
using Crossdyne.Security.Cryptography;
using Glyph.Bff.Services;
using Shared.Redis;

namespace Glyph.Bff.Extensions
{
 public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSingleton<IJwtReadService, JwtReadService>();
            services.AddCashService(configuration);
            services.AddSingleton<ICryptoService, CryptoService>();

            return services;
        }
    }
}