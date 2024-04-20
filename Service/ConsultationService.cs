using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;

namespace Service
{
    public class ConsultationService : IConsultationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Doctor> _doctorManager;

        public ConsultationService(
            IUnitOfWork unitOfWork,
            IConsultationRepository consultationRepository,
            IAppointmentRepository appointmentRepository,
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager)
        {
            _unitOfWork = unitOfWork;
            _consultationRepository = consultationRepository;
            _appointmentRepository = appointmentRepository;
            _patientManager = patientManager;
            _doctorManager = doctorManager;
        }
        public async Task<PatientConsultation> AddConsultationAsync(PatientConsultation patientConsultation)
        {
            var patient = await _patientManager.FindByIdAsync(patientConsultation.FK_PatientId);
            var doctor = await _doctorManager.FindByIdAsync(patientConsultation.FK_DoctorId);

            if (patient is null || doctor is null) 
                return null;

            if (patientConsultation.DateTime < DateTime.UtcNow) 
                return null;

            if (!(await ConsultationTimeIsAvailable(patientConsultation)))
                return null;

            await _consultationRepository.AddConsultationAsync(patientConsultation);
            await _unitOfWork.CompleteAsync();

            return patientConsultation;
        }

        public Task<int> DeleteAsync(PatientConsultation patientConsultation)
        {
            throw new NotImplementedException();
        }

        public Task<List<PatientConsultation>> GetByDoctorIdAsync(string doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PatientConsultation>> GetByPatientIdAsync(string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PatientConsultation>> GetConsultationBetweenPatientAndDoctorAsync(string patientId, string doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<PatientConsultation> GetNextConsultationAsync(string patientId, string doctorId)
        {
            throw new NotImplementedException();
        }

        // method to check booking is available or not
        private async Task<bool> ConsultationTimeIsAvailable(PatientConsultation consultation)
        {
            var consultations = await _consultationRepository.GetByDoctorIdAsync(consultation.FK_DoctorId);

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(consultation.FK_DoctorId);

            foreach (var con in consultations.Where(x => x.DateTime > DateTime.UtcNow))
            {
                TimeSpan diff = consultation.DateTime - con.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            foreach (var con in appointments.Where(x => x.DateTime > DateTime.UtcNow))
            {
                TimeSpan diff = consultation.DateTime - con.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            return true;
        }
    }
}
