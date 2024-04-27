using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IOperationRepository
    {
        Task AddAsync(DoctorDoOperation doctorDoOperation);
        Task<List<DoctorDoOperation>> GetScheduledOperationsAsync(); 
    }
}
