using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public class QueryStringConstructor
    {

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
