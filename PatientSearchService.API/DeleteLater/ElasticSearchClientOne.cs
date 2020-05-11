using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class ElasticSearchClientOne
    {
        private readonly IElasticClient _client;

        public ElasticSearchClientOne(IElasticClient client)
        {
            _client = client;
        }

        public ElasticSearchClientOne(string uri, string indexName) : this(CreateElasticClient(uri, indexName)) { }

        public IElasticClient GetClient()
        {
            return _client;
        }

        private static ElasticClient CreateElasticClient(string uri, string indexName)
        {
            var node = new Uri(uri);
            var pool = new SingleNodeConnectionPool(new Uri(uri));
            var setting = new ConnectionSettings(pool).BasicAuthentication("elastic", "RqtaUJo4yRuIuyqXMZSmaFAt");
            setting.DefaultIndex(indexName);
            return new ElasticClient(setting);
        }
    }
}
