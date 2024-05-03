using Core.UpCareEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Contract
{
    public interface INurseCareRepository
    {
        Task AddAsync(NurseCare nurseCare);
        Task UpdateAsync(NurseCare nurseCare);
        Task DeleteAsync(NurseCare nurseCare);
        Task<List<NurseCare>> GetAllRecordsAsync();
        Task<List<NurseCare>> GetByNurseIdAsync(string nurseId);
        Task<List<NurseCare>> GetByRoomIdAsync(int roomId);
        Task<List<NurseCare>> GetByPatientIdAsync(string patientId);
        Task<NurseCare> GetSpecificAsync(NurseCare nurseCare);
    }
}
