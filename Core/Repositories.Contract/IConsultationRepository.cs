using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IConsultationRepository
    {
        Task<List<PatientConsultation>> GetByPatientIdAsync(string patientId);
        Task<List<PatientConsultation>> GetByDoctorIdAsync(string doctorId);
        Task<List<PatientConsultation>> GetConsultationBetweenPatientAndDoctorAsync(string patientId, string doctorId);
        Task<PatientConsultation> GetNextConsultationAsync(string patientId, string doctorId);
        Task AddConsultationAsync(PatientConsultation patientConsultation);
        void DeleteAsync(PatientConsultation patientConsultation);
        Task<PatientConsultation> GetWithSpec(PatientConsultation consultation);
        Task<List<PatientConsultation>> GetAllAsync();
    }
}
