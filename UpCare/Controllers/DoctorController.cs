using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpCare.DTOs;
using UpCare.DTOs.PatientDtos;
using UpCare.DTOs.StaffDTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class DoctorController : BaseApiController
    {
        private readonly UserManager<Doctor> _userManager;
        private readonly UserManager<Patient> _patintManager;
        private readonly IAuthServices _authServices;
        private readonly IAppointmentService _appointmentService;
        private readonly IConsultationService _consultationService;
        private readonly SignInManager<Doctor> _signInManager;

        public DoctorController(
            UserManager<Doctor> userManager,
            UserManager<Patient> patintManager,
            IAuthServices authServices,
            IAppointmentService appointmentService,
            IConsultationService consultationService,
            SignInManager<Doctor> signInManager)
        {
            _userManager = userManager;
            _patintManager = patintManager;
            _authServices = authServices;
            _appointmentService = appointmentService;
            _consultationService = consultationService;
            _signInManager = signInManager;
        }

        [HttpPost("login")] // POST: /api/doctor/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            return Ok(new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "doctor"
            });
        }

        [HttpPost("add")]// POST: /api/doctor/add
        public async Task<ActionResult<UserDto>> Register(DoctorRegisterDto model)
        {
            var user = new Doctor()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Speciality = model.Speciality,
                IsSurgeon = model.IsSurgeon,
                ConsultationPrice = model.ConsultationPrice,
                AppointmentPrice = model.AppointmentPrice,
                FK_AdminId = model.AdminId,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded is false)
            {
                var error = new ApiValidationErrorResponse();
                foreach (var item in result.Errors)
                {
                    error.Errors.Add(item.Code);
                }

                return BadRequest(error);
            }

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "doctor"
            });
        }

        [HttpGet("search")] // GET: api/doctor/search?nameSearchTerm=string
        public async Task<ActionResult<List<Doctor>>> Search([FromQuery] string nameSearchTerm)
        {
            var doctors = await _userManager.Users.Where(p => p.FirstName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())
                                                            || p.LastName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower()))
                                                   .AsNoTracking().ToListAsync();
            if (doctors.Count() == 0)
                return BadRequest(new ApiResponse(404, "No doctors matches search term"));

            return Ok(doctors);
        }

        [HttpGet("all")] // GET: /api/doctor/all
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAll()
        {
            var doctors = await _userManager.Users.ToListAsync();

            if (doctors.Count() == 0)
                return NotFound(new ApiResponse(404, "there are no doctors found"));

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetSpecificDoctor(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);

            if (doctor is null)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(doctor);
        }

        [HttpGet("specialities")] // GET: /api/doctor/specialities
        public async Task<ActionResult<List<string>>> GetSpecialities()
        {
            var specialities = await _userManager.Users.Select(d => d.Speciality).Where(s => s != "").Distinct().ToListAsync();

            List<SpecialityDto> result = new List<SpecialityDto>();

            foreach (var speicality in specialities)
            {
                var number = _userManager.Users.Where(x => x.Speciality == speicality).Count();

                result.Add(new SpecialityDto { Speciality = speicality, DoctorWithSPeciality = number });
            }



            if (specialities.Count() == 0)
                return NotFound(new ApiResponse(404, "no data founded"));

            return Ok(result);
        }

        [HttpGet("schedule/{doctorId}")] // GET: /api/doctor/schedule/{doctorId}
        public async Task<ActionResult<List<ConversationDto>>> GetScheduleForDoctor(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "no doctor matches found"));

            var conversations = await GetConversationsByDoctor(doctor);

            if (conversations is null || conversations.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(conversations);
        }

        [HttpGet("schedule/all")] // GET: /api/doctor/schedule/all
        public async Task<ActionResult<List<ScheduleDto>>> GetScheduleForAllDoctors()
        {
            var doctors = await _userManager.Users.ToListAsync();

            if (doctors is null | doctors.Count() == 0)
                return NotFound(new ApiResponse(404, "no doctors recorded found"));

            var schedule = new List<ScheduleDto>();

            foreach (var doctor in doctors)
            {
                var itemToAdd = new ScheduleDto
                {
                    Doctor = doctor,
                    Conversations = await GetConversationsByDoctor(doctor)
                };

                schedule.Add(itemToAdd);
            }

            return Ok(schedule);
        }

        // private methods
        private async Task<List<ReservationDto>> GetConversationsByDoctor(Doctor doctor)
        {
            var appointments = await _appointmentService.GetAllByDoctorIdAsync(doctor.Id);

            var consultations = await _consultationService.GetByDoctorIdAsync(doctor.Id);

            var conversations = new List<ReservationDto>();

            if (appointments != null && appointments.Count() >= 0)
            {
                foreach (var appointment in appointments)
                {
                    var itemToAdd = new ReservationDto
                    {
                        DateTime = appointment.DateTime,
                        Patient = await _patintManager.FindByIdAsync(appointment.FK_PatientId),
                        Passed = (DateTime.UtcNow > appointment.DateTime) ? true : false,
                        Type = appointment.Type.ToString()
                    };

                    conversations.Add(itemToAdd);
                }
            }

            if (consultations != null && consultations.Count() >= 0)
            {
                foreach (var consultation in consultations)
                {
                    var itemToAdd = new ReservationDto
                    {
                        DateTime = consultation.DateTime,
                        Patient = await _patintManager.FindByIdAsync(consultation.FK_PatientId),
                        Passed = (DateTime.UtcNow.AddMinutes(-30) > consultation.DateTime) ? true : false,
                        Type = consultation.Type.ToString()
                    };

                    conversations.Add(itemToAdd);
                }
            }

            return conversations.OrderByDescending(x => x.DateTime).ToList();
        }


        /*
         *      1. Prescription (CRUDS)
         */
    }

    public class SpecialityDto
    {
        public string Speciality { get; set; }
        public int DoctorWithSPeciality { get; set; }
    }
}
