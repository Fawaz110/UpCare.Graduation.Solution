using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> DeleteAsync(PatientConsultation patientConsultation)
        {
            var consultation = await _consultationRepository.GetWithSpec(patientConsultation);

            if(consultation is null) 
                return 0;

            _consultationRepository.DeleteAsync(consultation);

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<List<PatientConsultation>> GetByDoctorIdAsync(string doctorId)
        {
            var consultations = await _consultationRepository.GetByDoctorIdAsync(doctorId);

            if (consultations.Count() == 0) 
                return null;

            return consultations;
        }

        public async Task<List<PatientConsultation>> GetByPatientIdAsync(string patientId)
        {
            var consultations = await _consultationRepository.GetByPatientIdAsync(patientId);

            if (consultations.Count() == 0)
                return null;

            return consultations;
        }

        public Task<List<PatientConsultation>> GetConsultationBetweenPatientAndDoctorAsync(string patientId, string doctorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Doctor> GetFirstAvailableDoctorBySpeciality(string speciality)
        {
            var specialities = _doctorManager.Users.Select(x=>x.Speciality).Distinct().ToList();

            if (!specialities.Contains(speciality))
                return null;

            var doctors = await _doctorManager.Users.Where(d => d.Speciality == speciality).ToListAsync();

            foreach (var doctor in doctors)
                if(await DoctorIsAvailableNow(doctor))
                    return doctor;

            return null;                
        }

        public async Task<PatientConsultation> GetNextConsultationAsync(string patientId, string doctorId)
        {
            var patient = await _patientManager.FindByIdAsync(patientId);

            if (patient is null) 
                return null;

            var doctor = await _doctorManager.FindByIdAsync(doctorId);

            if (doctor is null)
                return null;

            var result = await _consultationRepository.GetNextConsultationAsync(patientId, doctorId);

            return result;
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
        private async Task<bool> DoctorIsAvailableNow(Doctor doctor)
        {
            var consultations = await _consultationRepository.GetByDoctorIdAsync(doctor.Id);

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);

            foreach (var con in consultations.Where(x => x.DateTime > DateTime.UtcNow))
            {
                TimeSpan diff = DateTime.UtcNow - con.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            foreach (var con in appointments.Where(x => x.DateTime > DateTime.UtcNow))
            {
                TimeSpan diff = DateTime.UtcNow - con.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            return true;
        }
    }
}
