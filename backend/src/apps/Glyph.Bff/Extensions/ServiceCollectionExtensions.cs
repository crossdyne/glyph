using System.Reflection;
using Glyph.Bff.Infrastructure.Clients;
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

        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var authBaseUrl = configuration["Urls:AuthServicesBase"];
            string authenticationServices = "AuthenticationServices";
            services.AddHttpClient<IAuthClient, AuthClient>(authenticationServices, client => client.BaseAddress = new Uri(authBaseUrl!));

            var assetsBaseUrl = configuration["Urls:AssetsServicesBase"];
            string assetsServices = "AssetsServices";
            services.AddHttpClient<IPersonalCategoriesClient, PersonalCategoriesClient>(assetsServices, client => client.BaseAddress = new Uri(assetsBaseUrl!));

            return services;
        }
    }
}