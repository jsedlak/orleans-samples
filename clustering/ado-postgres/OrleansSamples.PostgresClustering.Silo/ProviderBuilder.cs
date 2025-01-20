using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Providers;

[assembly: RegisterProvider("PostgresDatabase", "Clustering", "Silo", typeof(PostgresClusteringProviderBuilder))]
[assembly: RegisterProvider("PostgresDatabase", "Clustering", "Client", typeof(PostgresClusteringProviderBuilder))]

[assembly: RegisterProvider("PostgresServer", "Clustering", "Silo", typeof(PostgresClusteringProviderBuilder))]
[assembly: RegisterProvider("PostgresServer", "Clustering", "Client", typeof(PostgresClusteringProviderBuilder))]

namespace Orleans.Hosting;

internal sealed class PostgresClusteringProviderBuilder : IProviderBuilder<ISiloBuilder>, IProviderBuilder<IClientBuilder>
{
    public void Configure(ISiloBuilder builder, string name, IConfigurationSection configurationSection)
    {
        builder.UseAdoNetClustering((OptionsBuilder<AdoNetClusteringSiloOptions> optionsBuilder) => optionsBuilder.Configure<IServiceProvider>((options, services) =>
        {
            options.Invariant = "Npgsql";
            
            var serviceKey = configurationSection["ServiceKey"];
            if (!string.IsNullOrWhiteSpace(serviceKey))
            {
                options.ConnectionString = services.GetRequiredService<IConfiguration>()
                    .GetConnectionString(serviceKey);
            }
        }));
    }

    public void Configure(IClientBuilder builder, string name, IConfigurationSection configurationSection)
    {
        builder.UseAdoNetClustering((OptionsBuilder<AdoNetClusteringClientOptions> optionsBuilder) => optionsBuilder.Configure<IServiceProvider>((options, services) =>
        {
            options.Invariant = "Npgsql";

            var serviceKey = configurationSection["ServiceKey"];
            if (!string.IsNullOrWhiteSpace(serviceKey))
            {
                options.ConnectionString = services.GetRequiredService<IConfiguration>()
                    .GetConnectionString(serviceKey);
            }
        }));
    }
}
