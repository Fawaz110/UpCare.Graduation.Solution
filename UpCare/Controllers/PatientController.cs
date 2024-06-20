using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.PrescriptionEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api.Models;
using Service;
using UpCare.DTOs;
using UpCare.DTOs.Patient;
using UpCare.DTOs.PatientDtos;
using UpCare.DTOs.PrescriptionDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class PatientController : BaseApiController
    {
        private readonly IAuthServices _authServices;
        private readonly UserManager<Patient> _patientManager;
        private readonly SignInManager<Patient> _signInManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentService _appointmentService;
        private readonly IConsultationService _consultationService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly ICheckupService _checkupService;
        private readonly IRadiologyService _radiologyService;
        private readonly IConfiguration _configuration;

        public PatientController(
            UserManager<Patient> patientManager,
            SignInManager<Patient> signInManager,
            UserManager<Doctor> doctorManager,
            IAuthServices authServices,
            IUnitOfWork unitOfWork,
            IAppointmentService appointmentService,
            IConsultationService consultationService,
            IPrescriptionService prescriptionService,
            ICheckupService checkupService,
            IRadiologyService radiologyService,
            IConfiguration configuration)
        {
            _authServices = authServices;
            _patientManager = patientManager;
            _signInManager = signInManager;
            _doctorManager = doctorManager;
            _unitOfWork = unitOfWork;
            _appointmentService = appointmentService;
            _consultationService = consultationService;
            _prescriptionService = prescriptionService;
            _checkupService = checkupService;
            _radiologyService = radiologyService;
            _configuration = configuration;
        }
        [HttpPost("login")] // POST: /api/patient/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _patientManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _patientManager),
                UserRole = "patient"
            });
        }
        [HttpPost("register")]// POST: /api/patient/register
        public async Task<ActionResult<UserDto>> Register(PatientRegisterDto model)
        {
            var user = new Patient()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                BloodType = model.BloodType,
                Gender = (model.Gender == "male") ? Gender.Male : Gender.Female,
                FK_ReceptionistId = model.ReceptionistId,
                DateOfBirth = model.DateOfBirth,
            };

            var result = await _patientManager.CreateAsync(user, model.Password);

            if (result.Succeeded is false)
            {
                var error = new ApiValidationErrorResponse();
                foreach (var item in result.Errors)
                    error.Errors.Add(item.Code);
                
                return BadRequest(error);
            }

            return Ok(new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _patientManager),
                UserRole = "patient",
                UserName = user.UserName
            });
        }

        [HttpGet("all")] // GET: /api/patient/all
        public async Task<ActionResult<List<Patient>>> GetAll()
        {
            var patients = await _patientManager.Users.AsNoTracking().ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")] // GET: /api/patient/{patientId}
        public async Task<ActionResult<Patient>> GetByPatientId(string id)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return BadRequest(new ApiResponse(404, "Patient not found"));

            return Ok(patient);
        }

        [HttpGet("search")] // GET: api/patient/search?nameSearchTerm
        public async Task<ActionResult<List<Patient>>> Search([FromQuery]string nameSearchTerm)
        {
            var patients = await _patientManager.Users.Where(p => p.FirstName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())
                                                            || p.LastName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower()))
                                                   .AsNoTracking().ToListAsync();
            if (patients.Count() == 0)
                return BadRequest(new ApiResponse(404, "No patients matches search term"));
            
            return Ok(patients);
        }

        [HttpGet("history/{patientId}")] // GET: /api/patient/history/{patientId}
        public async Task<ActionResult<PatientHistoryDto>> GetPatientHistory(string patientId)
        {
            var patient = await _patientManager.FindByIdAsync(patientId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MapToPatientHistoryDto(patient);

            return Ok(mapped);
        }


        // private methods
        private async Task<PatientHistoryDto> MapToPatientHistoryDto(Patient patient)
        {

            #region Get All Conversations
            var consultations = await _consultationService.GetByPatientIdAsync(patient.Id);
            var appointments = await _appointmentService.GetAllByPatientIdAsync(patient.Id);

            var mappedAppointments = new List<ConversationDto>();
            if (appointments != null && appointments.Count() > 0)
                foreach (var appointment in appointments)
                {
                    var conversation = new ConversationDto
                    {
                        Doctor = await _doctorManager.FindByIdAsync(appointment.FK_DoctorId),
                        DateTime = appointment.DateTime,
                        Passed = (DateTime.UtcNow.AddMinutes(-30) > appointment.DateTime) ? true : false,
                        Type = appointment.Type.ToString()
                    };

                    mappedAppointments.Add(conversation);
                }

            var mappedConsultation = new List<ConversationDto>();
            if (consultations != null && consultations.Count() > 0)
                foreach (var consultation in consultations)
                {
                    var conversation = new ConversationDto
                    {
                        Doctor = await _doctorManager.FindByIdAsync(consultation.FK_DoctorId),
                        DateTime = consultation.DateTime,
                        Passed = (DateTime.UtcNow.AddMinutes(-30) > consultation.DateTime) ? true : false,
                        Type = consultation.Type.ToString()
                    };

                    mappedConsultation.Add(conversation);
                }

            var conversations = mappedAppointments.Concat(mappedConsultation).OrderByDescending(x => x.DateTime).ToList(); 
            #endregion

            var medicineHistory = new List<MedicineHistory>();
            var checkupHistory = new List<CheckupHistory>();
            var radiologyHistory = new List<RadiologyHistory>();

            var checkupResults = (await _checkupService.GetAllResults()).Where(x => x.FK_PatientId == patient.Id).ToList();

            foreach (var item in checkupResults)
            {
                var itemToAdd = new CheckupHistory
                {
                    Checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_ChecupId),
                    DateTime = item.DateTime,
                    Result = Path.Combine(_configuration["AssetsBaseUrl"], "Uploads\\Checkups", item.Result)
                };

                checkupHistory.Add(itemToAdd);
            }

            var radiologyResults = (await _radiologyService.GetAllResults()).Where(x => x.FK_PatientId == patient.Id);

            foreach (var item in radiologyResults)
            {
                var itemToAdd = new RadiologyHistory
                {
                    Radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId),
                    DateTime = item.DateTime,
                    Result = Path.Combine(_configuration["AssetsBaseUrl"], "Uploads\\Radiologies", item.Result)
                };

                radiologyHistory.Add(itemToAdd);
            }

            var prescriptions = await _unitOfWork.Repository<Prescription>().GetAllAsync();

            var patientPrescriptions = prescriptions.Where(p =>p.FK_PatientId == patient.Id && ((!string.IsNullOrEmpty(p.PrescriptionPaymentIntentId)) || !(string.IsNullOrEmpty(p.MedicinePaymentIntentId)))).ToList();

            var mappedPatientPrescription = await MapToPrescriptionDto(patientPrescriptions);

            foreach (var pp in mappedPatientPrescription)
            {
                var medicineList = pp.Medicines.Select(m => new MedicineHistory
                {
                    Medicine = m,
                    DateTime = pp.DateTime
                }).ToList();

                medicineHistory.AddRange(medicineList);

                //var checkupList = pp.Checkups.Select(c => new CheckupHistory
                //{
                //    Checkup = c,
                //    DateTime = pp.DateTime
                //});

                //checkupHistory.AddRange(checkupList);

                //var radiologyList = pp.Radiologies.Select(r => new RadiologyHistory
                //{
                //    DateTime = pp.DateTime,
                //    Radiology = r
                //}).ToList();

                //radiologyHistory.AddRange(radiologyList);
            }

            return new PatientHistoryDto
            {
                PatientInfo = patient,
                Conversations = conversations,
                Radiologies = radiologyHistory.OrderByDescending(m => m.DateTime).ToList(),
                Checkups = checkupHistory.OrderByDescending(m => m.DateTime).ToList(),
                Medicines = medicineHistory.OrderByDescending(m => m.DateTime).ToList(),
            };
        }

        private async Task<List<PrescriptionDto>> MapToPrescriptionDto(List<Prescription> data)
        {
            var mapped = new List<PrescriptionDto>();

            foreach (var item in data)
            {
                var mappedItem = await MapToPrescriptionDto(item);

                mapped.Add(mappedItem);
            }

            return mapped;
        }

        private async Task<PrescriptionDto> MapToPrescriptionDto(Prescription item)
        {
            var medicineRecords = await _prescriptionService.GetMedicineByPrescriptionIdAsync(item.Id);

            var mappedMedicine = new List<Medicine>(); // 1

            foreach (var med in medicineRecords)
            {
                var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(med.FK_MedicineId);
                mappedMedicine.Add(medicine);
            }

            var checkupRecords = await _prescriptionService.GetCheckupByPrescriptionIdAsync(item.Id);

            var mappedCheckup = new List<Checkup>(); // 2

            foreach (var ch in checkupRecords)
            {
                var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(ch.FK_CheckupId);
                mappedCheckup.Add(checkup);
            }

            var radiologyRecords = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(item.Id);

            var mappedRadiology = new List<Radiology>(); // 3

            foreach (var rad in radiologyRecords)
            {
                var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(rad.FK_RadiologyId);

                mappedRadiology.Add(radiology);
            }

            return new PrescriptionDto
            {
                Id = item.Id,
                Advice = item.Advice,
                DateTime = item.DateTime,
                Details = item.Details,
                Diagnosis = item.Diagnosis,
                Checkups = mappedCheckup,
                Medicines = mappedMedicine,
                Radiologies = mappedRadiology,
                Doctor = await _doctorManager.FindByIdAsync(item.FK_DoctorId),
                Patient = await _patientManager.FindByIdAsync(item.FK_PatientId)
            };

        }

    }
}