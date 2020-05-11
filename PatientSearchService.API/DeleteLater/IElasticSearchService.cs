using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public interface IElasticSearchService
    {
        void Init();
        void CheckIndex();
        void BulkInsert(List<PatientDetailDto> objects);
        IEnumerable<PatientDetailDto> Search(string query);
    }

}
