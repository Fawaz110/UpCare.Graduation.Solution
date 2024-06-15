using Core.Entities.UpCareEntities;
using Core.UpCareEntities;

namespace Core.Services.Contract
{
    public interface IRoomService
    {
        Task<Room> AddRoomAsync(Room room);
        Task<PatientBookRoom> BookRoomAsync(PatientBookRoom patientBookRoom);
        Task<List<PatientBookRoom>> GetAllPatientBookingAsync();
        Task<PatientBookRoom> EndPatientRoomBooking(PatientBookRoom patientBookRoom);
        Task<PatientBookRoom> GetSpecificBookingAsync(string patientId, int roomId);
    }
}
