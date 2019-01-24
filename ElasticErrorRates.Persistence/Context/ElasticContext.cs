using System;
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
            ConnSettings = new ConnectionSettings(new Uri(configuration.GetConnectionString("ElasticConnection")));
            ElasticClient = new ElasticClient(ConnSettings);
        }

        public void SetupIndex<T>(string defaultIndex) where T : class
        {
            if (!ElasticClient.IndexExists(defaultIndex).Exists)
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

                ElasticClient.CreateIndex(defaultIndex, i => i
                    .Mappings(m => m
                        .Map<T>(ms => ms.AutoMap()))
                    .InitializeUsing(settings)
                );
            }

            ConnSettings.DefaultIndex(defaultIndex);
        }

    }
}
