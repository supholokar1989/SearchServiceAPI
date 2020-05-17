using PatientSearchService.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public interface IPatientSearchRepository
    {
        Task IndexDocument(PatientDetailDto PatientDetail);

        Task<List<PatientDetailDto>> Search(SearchPatientDetailsQuery CommonQuery);
    }
}
