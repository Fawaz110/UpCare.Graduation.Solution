using Core.UpCareEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contract
{
    public interface INurseCareService
    {
        Task<List<NurseCare>> GetNurseCareRecordsAsync(string patientId, int roomId);
        Task<NurseCare> AddAsync(NurseCare nurseCare);
        Task<int> DeleteAsync(string patientId, string nurseId, int roomId, DateTime dateTime);
        Task<int> UpdateAsync(NurseCare nurseCare);
    }
}
