using ElasticErrorRates.Core.Persistence;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticErrorRates.Persistence.Context
{
    public class ElasticContext : IElasticContext
    {
        public ConnectionSettings ConnSettings;
        public IElasticClient ElasticClient { get; }

        public ElasticContext(IConfiguration configuration)
        {
            ConnSettings = new ConnectionSettings(
                new Uri(configuration.GetConnectionString("ElasticConnection") 
                ?? throw new Exception("ElasticConnection not found ")));
            ElasticClient = new ElasticClient(ConnSettings);
        }

        public void SetupIndex<T>(string defaultIndex) where T : class
        {
            if (!ElasticClient.Indices.Exists(defaultIndex).Exists)
            {
                var settings = new CreateIndexRequest(defaultIndex)
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = 1,
                        NumberOfShards = 5,
                        Analysis = new Analysis()
                        {
                            Analyzers = new Analyzers(),
                            Tokenizers = new Tokenizers()
                        }
                    }
                };

                ElasticClient.Indices.Create(defaultIndex, i => i
                    .Map(m => m.AutoMap())
                    .InitializeUsing(settings)
                );
            }

            ConnSettings.DefaultIndex(defaultIndex);
        }

    }
}
