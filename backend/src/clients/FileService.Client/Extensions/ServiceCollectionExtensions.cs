using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.FileService.Interfaces;

namespace FileService.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string FileServiceClientName = "FileServiceClient";

        public static IServiceCollection AddFileServiceClients(this IServiceCollection services, IConfiguration configuration, Action<IHttpClientBuilder>? configure = null)
        {
            services.AddFileServiceHttpClient(configuration, configure);

            services.AddHttpClient<IFileServiceClient, FileStorageClient>(FileServiceClientName);
            services.AddHttpClient<IFileServiceReadOnlyClient, FileStorageClient>(FileServiceClientName);

            return services;
        }

        public static IServiceCollection AddFileServiceReadOnlyClients(this IServiceCollection services, IConfiguration configuration, Action<IHttpClientBuilder>? configure = null)
        {
            services.AddFileServiceHttpClient(configuration, configure);

            services.AddHttpClient<IFileServiceReadOnlyClient, FileStorageClient>(FileServiceClientName);

            return services;
        }

        private static void AddFileServiceHttpClient(this IServiceCollection services, IConfiguration configuration, Action<IHttpClientBuilder>? configure)
        {
            var fileServiceUrl = configuration["HttpClients:FileService"]!;

            var builder = services.AddHttpClient(FileServiceClientName, client =>
            {
                client.BaseAddress = new Uri(fileServiceUrl);
            });

            configure?.Invoke(builder);
        }
    }
}