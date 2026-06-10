using System.Reflection;
using FluentValidation;
using Glyph.Application.Behaviors;
using Glyph.Application.Interfaces.Services;
using Glyph.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Glyph.Application.Extensions
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