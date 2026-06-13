
using Glyph.Bff.Infrastructure.Clients;

namespace Glyph.Bff.Extensions
{
    public static class HttpClientsServiceCollectionExtensions
    {
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