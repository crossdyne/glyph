using Confluent.Kafka;
using FileService.Client.Extensions;
using Glyph.Assets.Application.Features.Account.EventHandlers;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Contexts;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Messaging.Abstractions;
using Shared.Contracts.Messaging.Events;

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

            services.Configure<ConsumerConfig>(configuration.GetSection("Kafka:Consumer"));
            services.AddScoped<IIntegrationEventHandler<UserAccountDeletedIntegrationEvent>, UserAccountDeletedIntegrationEventHandler>();
            services.AddKafkaConsumer<UserAccountDeletedIntegrationEvent>("user-management.user.account-delete");

            services.AddFileServiceClients(configuration);

            return services;
        }
    }
}