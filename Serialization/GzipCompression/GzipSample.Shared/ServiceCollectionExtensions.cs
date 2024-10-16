using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Storage;

namespace GzipSample.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGzipSerializer(this IServiceCollection services, string internalSerializerServiceKey)
    {
        services.AddSingleton<IGrainStorageSerializer, GzipSerializer>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var serializer = sp.GetRequiredKeyedService<IGrainStorageSerializer>(internalSerializerServiceKey);

            return new GzipSerializer(serializer, loggerFactory.CreateLogger<GzipSerializer>());
        });

        return services;
    }
}