using Core.UpCareEntities;

namespace Core.Services.Contract
{
    public interface IOperationService 
    {
        Task<List<DoctorDoOperation>> GetScheduledOperationsAsync();
        Task<DoctorDoOperation> AddToScheduleAsync(DoctorDoOperation data);
    }
}
