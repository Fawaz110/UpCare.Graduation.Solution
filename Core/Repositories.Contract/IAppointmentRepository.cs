using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IAppointmentRepository
    {
        Task<List<PatientAppointment>> GetByPatientIdAsync(string patientId);
        Task<List<PatientAppointment>> GetByDoctorIdAsync(string doctorId);
        Task<List<PatientAppointment>> GetAppointmentsBetweenPatientAndDoctorAsync(string patientId, string doctorId);
        Task<PatientAppointment> GetNextAppointmentAsync(string patientId, string doctorId);
        Task AddConsultationAsync(PatientAppointment patientConsultation);
        void DeleteAsync(PatientAppointment patientConsultation);
    }
}
