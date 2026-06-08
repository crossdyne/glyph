using Crossdyne.Glyph.Application.Interfaces;
using Crossdyne.Glyph.Application.Interfaces.Clients;
using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Glyph.Infrastructure.Clients;
using Crossdyne.Glyph.Infrastructure.Persistence;
using Crossdyne.Glyph.Infrastructure.Persistence.Contexts;
using Crossdyne.Glyph.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crossdyne.Glyph.Infrastructure.Extensions
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

            var fileServiceUrl = configuration["HttpClients:FileService"];
            services.AddHttpClient<IFileStorageClient, FileStorageClient>(http => http.BaseAddress = new Uri(fileServiceUrl!));

            return services;
        }
    }
}