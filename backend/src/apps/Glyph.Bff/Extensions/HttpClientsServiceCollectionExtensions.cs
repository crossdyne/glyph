
using Glyph.Bff.Handlers;
using Glyph.Bff.Infrastructure.Clients;
using Glyph.Bff.Interfaces.Clients;

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
            services.AddHttpClient(assetsServices, client => client.BaseAddress = new Uri(assetsBaseUrl!)).AddHttpMessageHandler<AccessTokenHandler>();
            services.AddHttpClient<IPersonalCategoriesClient, PersonalCategoriesClient>(assetsServices);

            return services;
        }
    }
}