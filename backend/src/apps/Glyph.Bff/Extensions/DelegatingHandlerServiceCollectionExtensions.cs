using Glyph.Bff.Handlers;

namespace Glyph.Bff.Extensions
{
    public static class DelegatingHandlerServiceCollectionExtensions
    {
        public static IServiceCollection AddDelegationsHandlers(this IServiceCollection services)
        {
            services.AddTransient<AccessTokenHandler>();

            return services;
        }
    }
}