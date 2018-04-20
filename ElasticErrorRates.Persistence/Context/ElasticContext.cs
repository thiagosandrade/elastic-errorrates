﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Microsoft.EntityFrameworkCore;
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
            ElasticClient = new ElasticClient();
            ConnSettings = new ConnectionSettings(new Uri(configuration.GetConnectionString("ElasticConnection")));

        }

        public void SetupIndex<T>(string defaultIndex) where T : class
        {
            if (!ElasticClient.IndexExists(defaultIndex).Exists)
            {
                var settings = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = 1,
                        NumberOfShards = 5,
                    }
                };

                ElasticClient.CreateIndex(defaultIndex, i => i
                    .Mappings(m => m
                        .Map<T>(ms => ms.AutoMap())
                    ).InitializeUsing(settings)
                );
            }

            ConnSettings.DefaultIndex(defaultIndex);
        }

    }
}
