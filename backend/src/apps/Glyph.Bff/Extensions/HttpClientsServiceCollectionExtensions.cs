using FileService.Client.Extensions;
using Glyph.Bff.Handlers;
using Glyph.Bff.Infrastructure.Clients;
using Glyph.Bff.Interfaces.Clients;

namespace Glyph.Bff.Extensions
{
    public static class HttpClientsServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var authBaseUrl = configuration["Urls:AuthServices"];
            string authenticationServices = "AuthenticationServices";
            services.AddHttpClient<IAuthClient, AuthClient>(authenticationServices, client => client.BaseAddress = new Uri(authBaseUrl!));

            var assetsBaseUrl = configuration["Urls:AssetsServices"];
            string assetsServices = "AssetsServices";
            services.AddHttpClient(assetsServices, client => client.BaseAddress = new Uri(assetsBaseUrl!)).AddHttpMessageHandler<AccessTokenHandler>();
            services.AddHttpClient<IGlobalCategoriesClient, GlobalCategoriesClient>(assetsServices);
            services.AddHttpClient<IPersonalCategoriesClient, PersonalCategoriesClient>(assetsServices);
            services.AddHttpClient<IGlobalAssetClient, GlobalAssetClient>(assetsServices);
            services.AddHttpClient<IPersonalAssetClient, PersonalAssetClient>(assetsServices);
            services.AddHttpClient<IProjectClient, ProjectClient>(assetsServices);

            var userManagementBaseUrl = configuration["Urls:UserManagementServices"];
            string usermanagementServices = "UserManagementServices";
            services.AddHttpClient<IUserManagementClient, UserManagementClient>(usermanagementServices, client => client.BaseAddress = new Uri(userManagementBaseUrl!)).AddHttpMessageHandler<AccessTokenHandler>();

            services.AddFileServiceReadOnlyClients(configuration, config => config.AddHttpMessageHandler<AccessTokenHandler>());

            return services;
        }
    }
}