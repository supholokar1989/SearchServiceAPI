using Nest;
using PatientSearchService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class PatientSearchRepository : IPatientSearchRepository
    {

        protected readonly ElasticSearchClient ElasticSearchClient;
        protected readonly string IndexName;


        public PatientSearchRepository(ElasticSearchClient elasticClient, string indexName)
        {
            this.ElasticSearchClient = elasticClient;
            this.IndexName = indexName;
        }


        public async Task IndexDocument(PatientDetailDto PatientDetail)
        {
            var ClientIndex = PatientDetail.ClientName.Replace(" ", string.Empty).ToLower();
            try
            {
                var result = await CreateIndexIfNotExist(ClientIndex);
                if (result)
                {
                   //var resultresponse = ElasticSearchClient.GetClient().Index(PatientDetail, i => i
                   //                                                           .Index(ClientIndex)
                   //                                                           .Id(PatientDetail.PatientVisitID));
                    var resultresponse = await ElasticSearchClient.GetClient().IndexAsync<PatientDetailDto>(PatientDetail, i => i
                                                                                       .Index(ClientIndex)
                                                                                       .Id(PatientDetail.PatientVisitID)
                                                                                       .Refresh(Elasticsearch.Net.Refresh.True));
                    Console.WriteLine(resultresponse.IsValid.ToString());
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Error while adding document to elastic search");
                Console.WriteLine(ex.Message.ToString());
            }
           

        }

        public async Task<bool> CreateIndexIfNotExist(string index)
        {
            if (!ElasticSearchClient.GetClient().Indices.Exists(index).Exists) {
                var response = await CreateIndexAsync(index);
                if (!response.IsValid)
                {
                    return false;
                }
            }
            return true;
        }

        protected async Task<IResponse> CreateIndexAsync(string index)
        {
            var response = await ElasticSearchClient.GetClient().Indices.CreateAsync(index,
                    index => index.Map<PatientDetailDto>(
                        x => x.AutoMap()
                    ));
            return response;
        }

        public async Task<List<PatientDetailDto>> FindPatientDetails(SearchPatientDetailsQuery searchRequest)
        {
            var result = await ElasticSearchClient.GetClient()
                .SearchAsync<PatientDetailDto>(
                    s =>
                        s.From(0)
                        .Size(10)
                        .Query(q =>
                            q.Bool(b =>
                            b.Must(m =>
                            m.Match(mm =>
                            mm.Query(searchRequest.Query)
                            .Field(f => f.PatientVisitID))))));

            return result.Documents.ToList();
        }

        // New Search Query

        public async Task<List<PatientDetailDto>> Search(SearchPatientDetailsQuery searchRequest)
        {
            var ClientIndex = searchRequest.ClientName.Replace(" ", string.Empty).ToLower();
            var filters = new List<Func<QueryContainerDescriptor<PatientDetailDto>, QueryContainer>>();
            if (!string.IsNullOrEmpty(searchRequest.Facility))
            {
                filters.Add(fq => fq.Match(t => t.Field(f => f.Facility).Query(searchRequest.Facility)));
            }
            if (!string.IsNullOrEmpty(searchRequest.FirstName))
            {
                filters.Add(fq => fq.Match(t => t.Field(f => f.FirstName).Query(searchRequest.FirstName)));
            }

            if (!string.IsNullOrEmpty(searchRequest.LastName))
            {
                filters.Add(fq => fq.Match(t => t.Field(f => f.LastName).Query(searchRequest.LastName)));
            }

            if (!string.IsNullOrEmpty(searchRequest.FinancialClass))
            {
                filters.Add(fq => fq.Match(t => t.Field(f => f.FinancialClass).Query(searchRequest.FinancialClass)));
            }

            if (!string.IsNullOrEmpty(searchRequest.PayerCode))
            {
                filters.Add(fq => fq.Match(t => t.Field(f => f.PayerCode).Query(searchRequest.PayerCode)));
            }

            if (!string.IsNullOrEmpty(searchRequest.AccountNumber))
            {
                filters.Add(fq => fq.Terms(t => t.Field(f => f.AccountNumber).Terms(searchRequest.AccountNumber)));
            }
            if (searchRequest.DateFrom != null || searchRequest.DateTo != null)
            {
                if (searchRequest.DateFrom != null)
                {
                    filters.Add(fq => fq.DateRange(d => d.Field(f => f.AdmitDate).GreaterThanOrEquals(searchRequest.DateFrom)));
                }

                if (searchRequest.DateTo != null)
                {
                    filters.Add(fq => fq.DateRange(d => d.Field(f => f.AdmitDate).LessThanOrEquals(searchRequest.DateTo)));
                }
            }
        
            var results = await ElasticSearchClient.GetClient().SearchAsync<PatientDetailDto>(s => s
                .From(searchRequest.Skip)
                .Size(searchRequest.PageSize)
                .Index(ClientIndex)
                .Query((q => q
                 .Bool(bq => bq.Filter(filters)))));

            return results.Documents.ToList();

        }
    }
}
