using Core.UpCareEntities;

namespace Core.Services.Contract
{
    public interface IAppointmentService
    {
        Task<PatientAppointment> AddAppointmentAsync(PatientAppointment appointment);
        Task<int> DeleteAsync(PatientAppointment appointment);
        Task<List<PatientAppointment>> GetAllByDoctorIdAsync(string Id);
        Task<List<PatientAppointment>> GetAllByPatientIdAsync(string Id);
    }
}
