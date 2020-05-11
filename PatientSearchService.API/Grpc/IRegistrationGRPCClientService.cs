using System;
using System.Threading.Tasks;

namespace PatientSearchService.API
{
    public interface IRegistrationGRPCClientService
    {
        Task<Adt> SearchRegistrationDataById(string documentId, Int64 clientId);
    }
}
