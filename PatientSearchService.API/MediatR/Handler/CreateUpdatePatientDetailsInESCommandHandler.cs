using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class CreateUpdatePatientDetailsInESCommandHandler : INotificationHandler<CreateUpdatePatientDetailsInESCommand>
    {
        private readonly IPatientSearchRepository _elasticSearchService;
        private readonly ILoggerFactory _logger;

        public CreateUpdatePatientDetailsInESCommandHandler(IPatientSearchRepository Patientsearchrepository, ILoggerFactory logger)
        {
            _elasticSearchService = Patientsearchrepository ?? throw new ArgumentNullException(nameof(Patientsearchrepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(CreateUpdatePatientDetailsInESCommand notification, CancellationToken cancellationToken)
        {
            var patientDetails = new PatientDetailDto { AccountNumber = notification.AccountNumber, AdmitDate = notification.AdmitDate, 
                AdmitType = notification.AdmitType, ClientID = notification.ClientID, 
                DOS = notification.DOS, FinancialClass = notification.FinancialClass, 
                FirstName = notification.FirstName, HAR = notification.HAR, LastName = notification.LastName, 
                MRN = notification.MRN, PatientType = notification.PatientType, PatientVisitID = notification.PatientVisitID, 
                PayerCode = notification.PayerCode, Registrar = notification.Registrar, SSN = notification.SSN, 
                Status = notification.Status, Gender = notification.Gender };
            await _elasticSearchService.AddAndSaveToES(patientDetails);
        }
    }
}
