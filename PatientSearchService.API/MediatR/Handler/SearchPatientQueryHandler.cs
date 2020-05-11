using MediatR;
using Microsoft.Extensions.Logging;
using PatientSearchService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class SearchPatientQueryHandler : IRequestHandler<SearchPatientDetailsQuery, SearchPatientDetailsResult>
    {
        private readonly IPatientSearchRepository _elasticSearchService;
        private readonly ILoggerFactory _logger;

        public SearchPatientQueryHandler(IPatientSearchRepository Patientsearchrepository, ILoggerFactory logger)
        {
            _elasticSearchService = Patientsearchrepository ?? throw new ArgumentNullException(nameof(Patientsearchrepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<SearchPatientDetailsResult> Handle(SearchPatientDetailsQuery request, CancellationToken cancellationToken)
        {
            var searchResults = await _elasticSearchService.Search(request);

            return FindPatientQueryResult(searchResults);
        }

        private SearchPatientDetailsResult FindPatientQueryResult(List<PatientDetailDto> searchResults)
        {
            return new SearchPatientDetailsResult
            {
                SearchResult = searchResults.Select(p => new PatientSearchDto
                {
                    AccountNumber = p.AccountNumber,
                    DOS = p.DOS,
                    FinancialClass = p.FinancialClass,
                    PatientName = p.FirstName + " " + p.LastName,
                    MRN = p.MRN,
                    PatientType = p.PatientType,
                    Gender = p.Gender,
                    Registrar = p.Registrar,
                    PatientVisitID = p.PatientVisitID
                })
                .ToList()
            };
        }
    }
}
