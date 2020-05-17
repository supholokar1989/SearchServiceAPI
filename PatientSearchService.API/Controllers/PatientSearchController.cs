using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PatientSearchService.Data;

namespace PatientSearchService.API
{
    [Route("api/patientsearch")]
    [ApiController]
    public class PatientSearchController : ControllerBase
    {
        private readonly ILogger<PatientSearchController> _logger;
        private readonly IPatientSearchRepository _elasticSearchService;
        private IMediator _mediatr;

        //protected readonly IPatientSearchRepository PatientSearchRepository;

        public PatientSearchController(ILogger<PatientSearchController> logger, IPatientSearchRepository elasticSearchService,
            IMediator mediatr)
        {
            _logger = logger;
            _elasticSearchService = elasticSearchService;
            _mediatr = mediatr;
        }

        [HttpGet("CreateClientIndex")]
        public async Task<ActionResult<bool>> CreateClientIndex()
        {
            //await _elasticSearchService.CreateIndexIfNotExist("");

            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PatientSearchDto>>> SearchAsync([FromQuery] QueryStringConstructor Searchquery)
        {
            var command = new SearchPatientDetailsQuery() { Query = Searchquery.Query, FirstName = Searchquery.FirstName, 
                                                            LastName = Searchquery.LastName,
                                                            AccountNumber = Searchquery.AccountNumber, 
                                                            DateFrom = Searchquery.DateFrom, 
                                                            PayerCode = Searchquery.PayerCode,
                                                            DateTo = Searchquery.DateTo, 
                                                            FinancialClass = Searchquery.FinancialClass,
                                                            ClientName = Searchquery.ClientName, Facility = Searchquery.FacilityCode};
            _logger.LogInformation("-----Sending command: RegistrationCommand");
            var result = await _mediatr.Send(command);
            return new JsonResult(result.SearchResult);
        }
        //public string Get()
        //{
        //    PatientSearchRepository.AddAndSaveToES(new PatientDetailDto() { ClientID = "123", FirstName = "Supholo" });
        //    return "Hello";
        //}
    }

   
}
