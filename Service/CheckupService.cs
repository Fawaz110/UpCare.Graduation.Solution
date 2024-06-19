using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;

namespace Service
{
    public class CheckupService : ICheckupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICheckupRepository _checkupRepository;

        public CheckupService(IUnitOfWork unitOfWork, ICheckupRepository checkupRepository)
        {
            _unitOfWork = unitOfWork;
            _checkupRepository = checkupRepository;
        }

        public async Task<Checkup> AddAsync(Checkup entity)
        {
            var checkup = await _checkupRepository.GetByName(entity.Name);

            if(checkup == null)
            {
                await _checkupRepository.Add(entity);
                
                await _unitOfWork.CompleteAsync();

                return entity;
            }
            else
            {
                checkup.Price = entity.Price;

                _checkupRepository.Update(checkup);

                await _unitOfWork.CompleteAsync();

                return checkup;
            }
        }

        public Task<int> AddCheckupResult(PatientCheckup patientCheckup)
        {
            _checkupRepository.AddPatientResult(patientCheckup);

            return _unitOfWork.CompleteAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(id);

            if(checkup != null)
            {
                _unitOfWork.Repository<Checkup>().Delete(checkup);

                return await _unitOfWork.CompleteAsync();
            }

            return 0;
        }

        public async Task<ICollection<Checkup>> GetAllAsync()
            => await _unitOfWork.Repository<Checkup>().GetAllAsync();

        public async Task<List<PatientCheckup>> GetAllResults()
            => await _checkupRepository.GetAllResults();

        public async Task<Checkup> GetByIdAsync(int id)
            => await _unitOfWork.Repository<Checkup>().GetByIdAsync(id);

        public async Task<Checkup> GetByName(string name)
            => await _checkupRepository.GetByName(name);

        public async Task Update(Checkup entity)
        {
            var checkup = await _checkupRepository.GetByName(entity.Name);

            if(checkup != null)
            {
                checkup.Price = entity.Price;

                _unitOfWork.Repository<Checkup>().Update(checkup);

                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
