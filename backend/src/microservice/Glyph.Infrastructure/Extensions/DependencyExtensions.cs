using Glyph.Application.Interfaces;
using Glyph.Application.Interfaces.Clients;
using Glyph.Application.Interfaces.Repositories;
using Glyph.Infrastructure.Clients;
using Glyph.Infrastructure.Persistence;
using Glyph.Infrastructure.Persistence.Contexts;
using Glyph.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glyph.Infrastructure.Extensions
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