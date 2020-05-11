using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatientSearchService.Data
{
    public class SearchPatientDetailsQuery : IRequest<SearchPatientDetailsResult>
    {
        public SearchPatientDetailsQuery()
        {
            this.PageSize = 100;
        }

        public int Skip { get; set; }
        public int PageSize { get; set; }

        public string Query { get; set; }

        public string PatientVisitID { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string AccountNumber { get; set; }

        public string PayerCode { get; set; }

        public string FinancialClass { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

