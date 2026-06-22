using FileService.Client.Extensions;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Contexts;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glyph.Assets.Infrastructure.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GlyphContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddFileServiceClients(configuration);

            return services;
        }
    }
}