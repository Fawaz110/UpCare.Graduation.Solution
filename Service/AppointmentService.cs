using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Repository
{
    public class AppointmentService : IAppointmentService
    {
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IConfiguration _configuration;

        public AppointmentService(
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager,
            IConsultationRepository consultationRepository,
            IUnitOfWork unitOfWork,
            IAppointmentRepository appointmentRepository,
            IOperationRepository operationRepository,
            IConfiguration configuration)
        {
            this._patientManager = patientManager;
            this._doctorManager = doctorManager;
            _consultationRepository = consultationRepository;
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
            _operationRepository = operationRepository;
            _configuration = configuration;
        }

        public async Task<PatientAppointment> AddAppointmentAsync(PatientAppointment appointment)
        {
            var patient = await _patientManager.FindByIdAsync(appointment.FK_PatientId);
            
            if (patient is null)
                return null;

            var doctor = await _doctorManager.FindByIdAsync(appointment.FK_DoctorId);

            if (doctor is null)
                return null;

            if (!(await ConsultationTimeIsAvailable(appointment)))
                return null;

            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(appointment.PaymentIntentId)) // Create PaymentIntent
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(doctor.AppointmentPrice * 100),
                    PaymentMethodTypes = new List<string>() { "card" },
                    Currency = "usd"
                };
                if (options.Amount > 0)
                {
                    paymentIntent = await paymentIntentService.CreateAsync(options);
                    appointment.PaymentIntentId = paymentIntent.Id;
                    appointment.ClientSecret = paymentIntent.ClientSecret;
                }
            }
            else // Update PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(doctor.AppointmentPrice * 100)
                };

                if (options.Amount > 0)
                    paymentIntent = await paymentIntentService.UpdateAsync(appointment.PaymentIntentId, options);
                
            }

            await _appointmentRepository.AddAppointmentAsync(appointment);

            var result = await _unitOfWork.CompleteAsync();

            return appointment;
        }

        public async Task<int> DeleteAsync(PatientAppointment appointment)
        {
            var app = await _appointmentRepository.GetWithSpec(appointment);

            if (app is null)
                return 0;

            _appointmentRepository.DeleteAsync(app);

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<List<PatientAppointment>> GetAllByDoctorIdAsync(string id)
        {
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(id);

            if (appointments.Count() == 0)
                return null;

            return appointments;
        }

        public async Task<List<PatientAppointment>> GetAllByPatientIdAsync(string id)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(id);

            if (appointments.Count() == 0)
                return null;

            return appointments;
        }

        // method to check booking is available or not
        private async Task<bool> ConsultationTimeIsAvailable(PatientAppointment appointment)
        {
            var consultations = await _consultationRepository.GetByDoctorIdAsync(appointment.FK_DoctorId);

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(appointment.FK_DoctorId);

            var ops = await _operationRepository.GetScheduledOperationsAsync();

            foreach (var con in consultations)
            {
                TimeSpan diff = appointment.DateTime - con.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            foreach (var con in appointments)
            {
                TimeSpan diff = appointment.DateTime - con.DateTime;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            foreach (var op in ops)
            {
                TimeSpan diff = appointment.DateTime - op.Date;
                if (diff.Duration() < TimeSpan.FromMinutes(30))
                    return false;
            }

            return true;
        }

        //private async Task<bool> DoctorIsAvailableNow(Doctor doctor)
        //{
        //    var consultations = await _consultationRepository.GetByDoctorIdAsync(doctor.Id);

        //    var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);

        //    foreach (var con in consultations)
        //    {
        //        TimeSpan diff = DateTime.UtcNow - con.DateTime;
        //        if (diff.Duration() < TimeSpan.FromMinutes(30))
        //            return false;
        //    }

        //    foreach (var con in appointments)
        //    {
        //        TimeSpan diff = DateTime.UtcNow - con.DateTime;
        //        if (diff.Duration() < TimeSpan.FromMinutes(30))
        //            return false;
        //    }

        //    return true;
        //}
    }
}
