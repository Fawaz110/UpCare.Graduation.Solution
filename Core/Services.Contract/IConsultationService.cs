using Core.UpCareEntities;

namespace Core.Services.Contract
{
    public interface IConsultationService
    {
        Task<List<PatientConsultation>> GetByPatientIdAsync(string patientId);
        Task<List<PatientConsultation>> GetByDoctorIdAsync(string doctorId);
        Task<List<PatientConsultation>> GetConsultationBetweenPatientAndDoctorAsync(string patientId, string doctorId);
        Task<PatientConsultation> GetNextConsultationAsync(string patientId, string doctorId);
        Task<PatientConsultation> AddConsultationAsync(PatientConsultation patientConsultation);
        Task<int> DeleteAsync(PatientConsultation patientConsultation);
    }
}
