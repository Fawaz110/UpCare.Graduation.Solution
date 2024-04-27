using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;

namespace Service
{
    public class OperationService : IOperationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationRepository _operationRepository;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;

        public OperationService(
            IUnitOfWork unitOfWork,
            IOperationRepository operationRepository,
            IConsultationRepository consultationRepository,
            IAppointmentRepository appointmentRepository,
            UserManager<Admin> adminManager,
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager)
        {
            _unitOfWork = unitOfWork;
            _operationRepository = operationRepository;
            _consultationRepository = consultationRepository;
            _appointmentRepository = appointmentRepository;
            _adminManager = adminManager;
            _doctorManager = doctorManager;
            _patientManager = patientManager;
        }
        public async Task<List<DoctorDoOperation>> GetScheduledOperationsAsync()
            => await _operationRepository.GetScheduledOperationsAsync();

        public async Task<DoctorDoOperation> AddToScheduleAsync(DoctorDoOperation data)
        {
            if (!(await TimeIsAvailable(data)))
                return null;

            await _operationRepository.AddAsync(data);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) 
                return null;

            return data;
        }

        // Method to check if doctor available or not
        private async Task<bool> TimeIsAvailable(DoctorDoOperation appointment)
        {
            var consultations = await _consultationRepository.GetByDoctorIdAsync(appointment.FK_DoctorId);

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(appointment.FK_DoctorId);

            var ops = await _operationRepository.GetScheduledOperationsAsync();

            foreach (var con in consultations)
            {
                TimeSpan diff = appointment.Date - con.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            foreach (var app in appointments)
            {
                TimeSpan diff = appointment.Date - app.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            foreach (var op in ops)
            {
                TimeSpan diff = appointment.Date - op.Date;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            return true;
        }
    }
}
