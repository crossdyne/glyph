using System.Reflection;
using Crossdyne.Glyph.Application.Behaviors;
using Crossdyne.Glyph.Application.Interfaces.Services;
using Crossdyne.Glyph.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Crossdyne.Glyph.Application.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IFileMetadataDetector, FileMetadataDetector>();

            return services;
        }
    }
}