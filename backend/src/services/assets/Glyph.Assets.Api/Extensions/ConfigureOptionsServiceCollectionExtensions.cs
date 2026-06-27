using System.Text.Json;
using Shared.Web.Extensions;

namespace Glyph.Assets.Api.Extensions
{
    public static class ConfigureOptionsServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureCustomOptions(this IServiceCollection services)
        {
            services.Configure<JsonSerializerOptions>(opt => opt.AddCrossdyneDefaults());

            return services;
        }
    }
}