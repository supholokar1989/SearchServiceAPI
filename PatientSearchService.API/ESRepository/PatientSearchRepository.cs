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


        public async Task AddAndSaveToES(PatientDetailDto PatientDetail)
        {
            try
            {
                if (IndexExist())
                {
                   var resultresponse = await ElasticSearchClient.GetClient().IndexDocumentAsync(PatientDetail);
                    Console.WriteLine(resultresponse.IsValid.ToString());
                }
                else
                {
                    var result = await CreateIndexIfNotExist();
                    if (result)
                    {
                      var  resultresponse = await ElasticSearchClient.GetClient().IndexDocumentAsync(PatientDetail);
                        Console.WriteLine(resultresponse.IsValid.ToString());
                    }

                }

            }catch(Exception ex)
            {
                Console.WriteLine("Error while adding document to elastic search");
                Console.WriteLine(ex.Message.ToString());
            }
           

        }

        public async Task<bool> CreateIndexIfNotExist()
        {
            if (IndexExist()) return true;
            var response = await CreateIndexAsync();

            if (!response.IsValid)
            {
                return false;
                throw new Exception(response.ServerError.ToString(), response.OriginalException);
            }

            return true;
        }

        protected async Task<IResponse> CreateIndexAsync()
        {
            var indexDescriptor = new CreateIndexDescriptor(IndexName).Map(m => m.AutoMap());
            var response = await ElasticSearchClient.GetClient().Indices.CreateAsync(IndexName,
                    index => index.Map<PatientDetailDto>(
                        x => x.AutoMap()
                    ));
            return response;
        }

        protected bool IndexExist()
        {
            return ElasticSearchClient.GetClient().Indices.Exists(IndexName).Exists;
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
            var filters = new List<Func<QueryContainerDescriptor<PatientDetailDto>, QueryContainer>>();
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
                .Index("patientdetails")
                .Query((q => q
                 .Bool(bq => bq.Filter(filters)))));

            return results.Documents.ToList();

        }
    }
}
