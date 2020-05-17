using iPas.Infrastructure.EventBus.Events;
using System;

namespace PatientSearchService.API
{
    public class RegistrationReceivedIntegrationEvent : IntegrationEvent
    {
        public Int64 ClientId { get; set; }
        public string ClientName { get; set; }
        public Int64 FacilityId { get; set; }
        public Int64 PatientId { get; set; }
        public Int64 PatientVisitId { get; set; }
        public string DocumentId { get; set; }


        public RegistrationReceivedIntegrationEvent(Int64 clientId, Int64 facilityId, Int64 patientId, Int64 patientVisitId, string documentId, string clientname)
        {
            ClientId = clientId;
            FacilityId = facilityId;
            DocumentId = documentId;
            PatientId = patientId;
            PatientVisitId = patientVisitId;
            ClientName = clientname;
        }
    }
}

