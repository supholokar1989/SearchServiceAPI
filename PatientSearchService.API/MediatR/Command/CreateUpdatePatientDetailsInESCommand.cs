using MediatR;
using System;

namespace PatientSearchService.API
{
    public class CreateUpdatePatientDetailsInESCommand : INotification
    {
        public CreateUpdatePatientDetailsInESCommand(string _accountNumber, DateTimeOffset _admitDate, string _payerCode,
           string _ssn, string _financialClass, string _patientType, string _mrn, string _firstName,
           string _lastName, string _admitType, string _status, string _har, string _clientID, string _patientVisitID,
           string _registrar, string _dos, string _gender)
        {
            this.AccountNumber = _accountNumber;
            this.AdmitDate = _admitDate;
            this.PayerCode = _payerCode;
            this.SSN = _ssn;
            this.FinancialClass = _financialClass;
            this.PatientType = _patientType;
            this.MRN = _mrn;
            this.FirstName = _firstName;
            this.LastName = _lastName;
            this.AdmitType = _admitType;
            this.Status = _status;
            this.HAR = _har;
            this.ClientID = _clientID;
            this.PatientVisitID = _patientVisitID;
            this.Registrar = _registrar;
            this.DOS = _dos;
            this.Gender = _gender;
        }
        public string AccountNumber { get; set; }

        public DateTimeOffset AdmitDate { get; set; }

        public string PayerCode { get; set; }

        public string SSN { get; set; }

        public string FinancialClass { get; set; }

        public string PatientType { get; set; }

        public string MRN { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AdmitType { get; set; }

        public string Status { get; set; }

        public string HAR { get; set; }

        public string ClientID { get; set; }

        public string PatientVisitID { get; set; }

        public string Registrar { get; set; }

        public string DOS { get; set; }

        public string Gender { get; set; }

        public ESEvent esEvent { get; set; }
    }

    public enum ESEvent
    {
        Default = 0,
        CreateType = 1,
        UpdateType = 2
    }
}
