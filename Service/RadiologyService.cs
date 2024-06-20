using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RadiologyService : IRadiologyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRadiologyRepository _radiologyRepository;

        public RadiologyService(
            IUnitOfWork unitOfWork,
            IRadiologyRepository radiologyRepository)
        {
            _unitOfWork = unitOfWork;
            _radiologyRepository = radiologyRepository;
        }

        public async Task<Radiology> AddAsync(Radiology entity)
        {
            var radiology = await _radiologyRepository.GetByNameAsync(entity.Name);

            if (radiology is null)
            {
                await _unitOfWork.Repository<Radiology>().Add(entity);

                await _unitOfWork.CompleteAsync();

                return entity;
            }
            else
            {
                radiology.Price = entity.Price;

                _unitOfWork.Repository<Radiology>().Update(radiology);

                await _unitOfWork.CompleteAsync();

                return radiology;
            }

        }

        public async Task<int> AddRadiologyResult(PatientRadiology patientRadiology)
        {
            await _radiologyRepository.AddPatientResult(patientRadiology);

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var radiology = await _radiologyRepository.GetByIdAsync(id);

            if (radiology is null)
                return 0;

            _radiologyRepository.Delete(radiology);

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Radiology>> GetAllAsync()
            => await _unitOfWork.Repository<Radiology>().GetAllAsync();

        public async Task<List<PatientRadiology>> GetAllResults()
            => await _radiologyRepository.GetAllResults();

        public async Task<Radiology> GetByIdAsync(int id)
            => await _unitOfWork.Repository<Radiology>().GetByIdAsync(id);

        public async Task<Radiology> GetByName(string name)
            => await _radiologyRepository.GetByNameAsync(name);

        public async Task Update(Radiology entity)
        {
            _radiologyRepository.Update(entity);

            await _unitOfWork.CompleteAsync();
        }
    }
}
