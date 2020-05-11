using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class ElasticSearchClient
    {
        private readonly IElasticClient _client;
        public ElasticSearchClient(IElasticClient client)
        {
            _client = client;
        }

        public ElasticSearchClient(string connectionstring, string indexName) : this(CreateElasticClient(connectionstring, indexName)) { }

        public IElasticClient GetClient()
        {
            return _client;
        }

        private static ElasticClient CreateElasticClient(string cnString, string indexName)
        {
            var pool = new SingleNodeConnectionPool(new Uri(cnString));
            var setting = new ConnectionSettings(pool).BasicAuthentication("elastic", "RqtaUJo4yRuIuyqXMZSmaFAt");
            setting.DefaultIndex(indexName);
            return new ElasticClient(setting);
        }
    }
}
