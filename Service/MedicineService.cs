using AutoMapper;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;

namespace Service
{
    public class MedicineService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;

        public MedicineService(
            IUnitOfWork unitOfWork, 
            IMedicineRepository medicineRepository,
            IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _medicineRepository = medicineRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Medicine>> GetAllAsync()
            => await _unitOfWork.Repository<Medicine>().GetAllAsync();
        
        public async Task<Medicine> GetByIdAsync(int id) 
        {
            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(id);

            if (medicine is null) 
                return null;

            return medicine;
        }

        public async Task<int> AddMedicine(Medicine entity)
        {
            var medicine = await _medicineRepository.GetMedicineByNameAsync(entity.Name);

            if (medicine is null)
                _unitOfWork.Repository<Medicine>().Add(entity);
            else
            {
                medicine.Quantity += entity.Quantity;

                entity.Quantity = medicine.Quantity;

                _unitOfWork.Repository<Medicine>().Update(medicine);
            }

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<Medicine> GetMedicineByName(string name)
            => await _medicineRepository.GetMedicineByNameAsync(name);

        public async Task<List<Medicine>> SearchByMedicineName(string term)
            => await _medicineRepository.SearchByMedicineName(term);

        public async Task<List<string>> GetCategories()
            => await _medicineRepository.GetCategories();

        public async Task<List<Medicine>> GetShortage(int leastNormalQuantity)
            => await _medicineRepository.GetShortage(leastNormalQuantity);

        public async void UpdateMedicine(Medicine entity)
        {
            _medicineRepository.Update(entity);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(id);

            if (medicine is not null)
                _unitOfWork.Repository<Medicine>().Delete(medicine);
            
            
            return await _unitOfWork.CompleteAsync();
        }

    }
}
