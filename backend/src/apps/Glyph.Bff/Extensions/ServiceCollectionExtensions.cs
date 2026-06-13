using System.Reflection;
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
            
            return services;
        }
    }
}