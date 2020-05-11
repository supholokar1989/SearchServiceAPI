using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class CitiesService : ElasticSearchService
    {
        public CitiesService(ElasticSearchClientOne elasticSearchClient, string indexName) : base(elasticSearchClient, indexName) { }

        public override IEnumerable<PatientDetailDto> Search(string query)
        {
            var results = ElasticSearchClient.GetClient().Search<PatientDetailDto>(c => c.From(0).Size(10).Query(q => q.Prefix(p => p.HAR, query) || q.Term("district.name", query)));

            return results.Documents.OrderBy(d => d.FirstName);
        }
    }
}
