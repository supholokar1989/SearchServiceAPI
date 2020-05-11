using System;
using System.Collections.Generic;
using System.Text;

namespace PatientSearchService.Data
{
    public class CommonSearchQuery
    {
        public CommonSearchQuery()
        {
            this.PageSize = 20;
        }

        public int Skip { get; set; }
        public int PageSize { get; set; }

        public string Query { get; set; }

        public Int64 PatientVisitID { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
