using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class NurseCareService : INurseCareService
    {
        private readonly UserManager<Nurse> _nurseManager;
        private readonly UserManager<Patient> _patientManager;
        private readonly INurseCareRepository _nurseCareRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NurseCareService(
            UserManager<Nurse> nurseManager,
            UserManager<Patient> patientManager,
            INurseCareRepository nurseCareRepository,
            IUnitOfWork unitOfWork)
        {
            _nurseManager = nurseManager;
            _patientManager = patientManager;
            _nurseCareRepository = nurseCareRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<NurseCare> AddAsync(NurseCare nurseCare)
        {
            await _nurseCareRepository.AddAsync(nurseCare);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return nurseCare;
        }

        public async Task<List<NurseCare>> GetNurseCareRecordsAsync(string patientId, int roomId)
        {
            var result = await _nurseCareRepository.GetAllRecordsAsync();

            result = result.Where(x => x.FK_PatientId == patientId).ToList();

            result = result.Where(x => x.FK_RoomId == roomId).ToList();

            return result;  
        }

        public async Task<int> DeleteAsync(string patientId, string nurseId, int roomId, DateTime dateTime)
        {
            var list = await _nurseCareRepository.GetByNurseIdAsync(nurseId);

            var item = list.FirstOrDefault(x=>x.FK_PatientId == patientId && x.FK_RoomId == roomId && x.DateTime == dateTime);

            if(item is null)
                return 0;

            await _nurseCareRepository.DeleteAsync(item);

            return await _unitOfWork.CompleteAsync();
        }

        public Task<int> UpdateAsync(NurseCare nurseCare)
        {
            throw new NotImplementedException();
        }
    }
}
