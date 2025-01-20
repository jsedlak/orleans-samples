using Orleans.Providers;

[assembly: RegisterProvider("MongoDBDatabase", "Clustering", "Silo", typeof(MongoDBClusteringProviderBuilder))]
[assembly: RegisterProvider("MongoDBDatabase", "Clustering", "Client", typeof(MongoDBClusteringProviderBuilder))]

namespace Orleans.Hosting;

internal sealed class MongoDBClusteringProviderBuilder : IProviderBuilder<ISiloBuilder>, IProviderBuilder<IClientBuilder>
{
    public void Configure(ISiloBuilder builder, string name, IConfigurationSection configurationSection)
    {
        builder.UseMongoDBClustering((options) =>
        {
            options.DatabaseName = configurationSection["ServiceKey"];
        });
    }

    public void Configure(IClientBuilder builder, string name, IConfigurationSection configurationSection)
    {
        builder.UseMongoDBClustering((options) =>
        {
            options.DatabaseName = configurationSection["ServiceKey"];
        });
    }
}