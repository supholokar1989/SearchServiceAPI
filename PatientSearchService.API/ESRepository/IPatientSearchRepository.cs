using PatientSearchService.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public interface IPatientSearchRepository
    {
        Task<bool> CreateIndexIfNotExist();
        Task AddAndSaveToES(PatientDetailDto PatientDetail);

        Task<List<PatientDetailDto>> Search(SearchPatientDetailsQuery CommonQuery);

        Task<List<PatientDetailDto>> FindPatientDetails(SearchPatientDetailsQuery CommonQuery);
    }
}
