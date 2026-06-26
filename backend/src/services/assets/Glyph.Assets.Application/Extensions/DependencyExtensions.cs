using System.Reflection;
using FluentValidation;
using Glyph.Assets.Application.Behaviors;
using Glyph.Assets.Application.Interfaces.Services;
using Glyph.Assets.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Behaviors;

namespace Glyph.Assets.Application.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IFileMetadataDetector, FileMetadataDetector>();

            return services;
        }
    }
}