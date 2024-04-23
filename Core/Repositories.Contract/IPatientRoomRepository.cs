using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IPatientRoomRepository
    {
        Task<List<PatientBookRoom>> GetAllPatientRoomsAsync();

        Task AddBookingRoomAsync(PatientBookRoom data);
    }
}
