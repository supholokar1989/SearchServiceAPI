using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class ElasticSearchService : IElasticSearchService
    {
        protected readonly ElasticSearchClientOne ElasticSearchClient;
        protected readonly string IndexName;

        public ElasticSearchService(ElasticSearchClientOne elasticSearchClient, string indexName)
        {
            ElasticSearchClient = elasticSearchClient;
            IndexName = indexName;
        }

        public virtual void Init()
        {
            CheckIndex();
        }

        public void CheckIndex()
        {
            if (IndexExist()) return;
            var response = CreateIndex();

            if (!response.IsValid)
            {
                throw new Exception(response.ServerError.ToString(), response.OriginalException);
            }
        }

        public void BulkInsert(List<PatientDetailDto> objects)
        {
            var response = ElasticSearchClient.GetClient().IndexMany(objects, IndexName);
            if (!response.IsValid)
            {
                throw new Exception(response.ServerError.ToString(), response.OriginalException);
            }
        }

        public virtual IEnumerable<PatientDetailDto> Search(string query)
        {
            var results = ElasticSearchClient.GetClient().Search<PatientDetailDto>(c => c.From(0).Size(10).Query(q => q.Prefix("_all", query)));

            return results.Documents;
        }

        protected virtual IResponse CreateIndex()
        {
            var indexDescriptor = new CreateIndexDescriptor(IndexName).Map(m => m.AutoMap());
            var response = ElasticSearchClient.GetClient().Indices.Create(IndexName,
                    index => index.Map<PatientDetailDto>(
                        x => x.AutoMap()
                    ));
            return response;
        }

        protected bool IndexExist()
        {
            return ElasticSearchClient.GetClient().Indices.Exists(IndexName).Exists;
        }
    }
    public class Person
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
