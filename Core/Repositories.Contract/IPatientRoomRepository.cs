using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IPatientRoomRepository
    {
        Task<List<PatientBookRoom>> GetAllPatientRoomsAsync();
        Task AddBookingRoomAsync(PatientBookRoom data);
        void UpdatePatintBookRoom(PatientBookRoom data);
        Task<PatientBookRoom> GetSpecificRecord(PatientBookRoom data);
        Task<PatientBookRoom> GetSpecificBookingAsync(string patientId, int roomId);
    }
}
