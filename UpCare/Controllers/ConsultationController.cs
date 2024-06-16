using Core.Services.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.DTOs.PatientDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Doctor> _doctorManager;
        private List<string> _specialities;

        public ConsultationController(
            IConsultationService consultationService, 
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager)
        {
            _consultationService = consultationService;
            _patientManager = patientManager;
            _doctorManager = doctorManager;
            _specialities = _doctorManager.Users.Select(d => d.Speciality).Where(s => s != "").Distinct().ToList();
        }

        [HttpPost("book")] // POST: /api/consultation/book
        public async Task<ActionResult<SucceededToAdd>> BookConsultation([FromBody] PatientConsultation consultation)
        {
            var result = await _consultationService.AddConsultationAsync(consultation);

            if (result is null) 
                return BadRequest(new ApiResponse(400, "you may entered in valid data or time no available"));

            var objectToReturn = new ConsultationDto
            {
                DateTime = consultation.DateTime,
                Type = result.Type,
                PaymentIntentId = result.PaymentIntentId,
                ClientSecret = result.ClientSecret,
                Doctor = await _doctorManager.FindByIdAsync(consultation.FK_DoctorId),
                Patient = await _patientManager.FindByIdAsync(consultation.FK_PatientId)
            };

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = objectToReturn
            });
        }

        [HttpGet("dr/passed/{id}")] // GET: /api/consultation/passed/{id}
        public async Task<ActionResult<List<ConsultationDto>>> GetPassedConsultations(string id)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null) 
                return BadRequest(new ApiResponse(404, "no doctor matches found"));

            var result = await _consultationService.GetByDoctorIdAsync(id);

            var consultations = result.Where(c => c.DateTime < DateTime.UtcNow).ToList();

            if (consultations.Count() == 0) 
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<ConsultationDto>();

            foreach (var consultation in consultations)
            {
                var con = new ConsultationDto
                {
                    Doctor = await _doctorManager.FindByIdAsync(consultation.FK_DoctorId),
                    Patient = await _patientManager.FindByIdAsync(consultation.FK_PatientId),
                    DateTime = consultation.DateTime,
                    Type = consultation.Type
                };

                listToReturn.Add(con);
            }

            return Ok(listToReturn.OrderByDescending(x => x.DateTime));
        }

        [HttpGet("dr/next/{id}")] // GET: /api/consultation/next/{id}
        public async Task<ActionResult<List<ConsultationDto>>> GetComingConsultations(string id)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return BadRequest(new ApiResponse(404, "no doctor matches found"));

            var result = await _consultationService.GetByDoctorIdAsync(id);

            var consultations = result.Where(c => c.DateTime > DateTime.UtcNow).ToList();

            if (consultations.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<ConsultationDto>();

            foreach (var consultation in consultations)
            {
                var con = new ConsultationDto
                {
                    Doctor = await _doctorManager.FindByIdAsync(consultation.FK_DoctorId),
                    Patient = await _patientManager.FindByIdAsync(consultation.FK_PatientId),
                    DateTime = consultation.DateTime,
                    Type = consultation.Type
                };

                listToReturn.Add(con);
            }

            return Ok(listToReturn.OrderBy(x => x.DateTime));
        }

        [HttpGet("pt/passed/{id}")] // GET: /api/consultation/pt/passed/{id}
        public async Task<ActionResult<List<ConsultationDto>>> GetPassedForPatient(string id)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return BadRequest(new ApiResponse(404, "no patient matches found"));

            var result = await _consultationService.GetByPatientIdAsync(id);

            var consultations = result.Where( x => x.DateTime < DateTime.UtcNow ).ToList();

            if (consultations.Count() == 0) 
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<ConsultationDto>();

            foreach (var item in consultations)
            {
                var con = new ConsultationDto
                {
                    DateTime = item.DateTime,
                    Doctor = await _doctorManager.FindByIdAsync(item.FK_DoctorId),
                    Patient = await _patientManager.FindByIdAsync(item.FK_PatientId),
                    Type = item.Type
                };

                listToReturn.Add(con);
            }

            return Ok(listToReturn.OrderByDescending(x => x.DateTime));
        }

        [HttpGet("pt/next/{id}")] // GET: /api/consultation/pt/next/{id}
        public async Task<ActionResult<List<ConsultationDto>>> GetComingForPatient(string id)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return BadRequest(new ApiResponse(404, "no patient matches found"));

            var result = await _consultationService.GetByPatientIdAsync(id);

            if (result is null | result?.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var consultations = result.Where(x => x.DateTime > DateTime.UtcNow).ToList();

            if (consultations.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<ConsultationDto>();

            foreach (var item in consultations)
            {
                var con = new ConsultationDto
                {
                    DateTime = item.DateTime,
                    Doctor = await _doctorManager.FindByIdAsync(item.FK_DoctorId),
                    Patient = await _patientManager.FindByIdAsync(item.FK_PatientId),
                    Type = item.Type
                };

                listToReturn.Add(con);
            }

            return Ok(listToReturn.OrderBy(x => x.DateTime));
        }

        [HttpGet("next")] // GET: /api/consultation/next?patientId={value}&doctorId={value}
        public async Task<ActionResult<ConsultationDto>> GetNextForDoctorWithPatient([FromQuery]string patientId, [FromQuery]string doctorId)
        {
            var doctor = _doctorManager.FindByIdAsync(doctorId);

            if (doctor is null) 
                return BadRequest(new ApiResponse(400));

            var patient = _patientManager.FindByIdAsync(patientId);

            if (patient is null)
                return BadRequest(new ApiResponse(400));

            var result = await _consultationService.GetNextConsultationAsync(patientId, doctorId);

            if (result is null) 
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(new ConsultationDto
            {
                DateTime = result.DateTime,
                Type = result.Type,
                Doctor = await _doctorManager.FindByEmailAsync(doctorId),
                Patient = await _patientManager.FindByIdAsync(patientId)
            });
        }

        [HttpGet("emergency/all")] // POST: /api/consultation/emergency/all
        public async Task<ActionResult<List<ConsultationDto>>> GetAllEmergency()
        {
            var emergencyList = await _consultationService.GetAllConsultationsAsync();

            var result = new List<ConsultationDto>();

            foreach (var emergency in emergencyList)
            {
                if(emergency.Type == ConsultationType.OfflineEmergency || emergency.Type == ConsultationType.OnlineEmergency)
                {
                    var item = new ConsultationDto
                    {
                        Type = emergency.Type,
                        DateTime = emergency.DateTime,
                        Doctor = await _doctorManager.FindByIdAsync(emergency.FK_DoctorId),
                        Patient = await _patientManager.FindByIdAsync(emergency.FK_PatientId)
                    };
                    result.Add(item);
                }    
            }

            if (result.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(result);
        }

        [HttpPost("emergency")] // POST: /api/consultation/emergency
        public async Task<ActionResult<SucceededToAdd>> AddEmergency([FromBody]EmergencyDto model)
        {
            if (!_specialities.Contains(model.Speciality))
                return BadRequest(new ApiResponse(400));

            var doctor = await _consultationService.GetFirstAvailableDoctorBySpeciality(model.Speciality);

            if (doctor is null)
                return NotFound(new ApiResponse(404, "no doctor available found"));

            var patient = await _patientManager.FindByIdAsync(model.PatientId);

            if (patient is null)
                return BadRequest(new ApiResponse(400));

            var emergency = new PatientConsultation
            {
                DateTime = DateTime.UtcNow,
                Type = model.Type,
                FK_PatientId = patient.Id,
                FK_DoctorId = doctor.Id
            };

            var added = await _consultationService.AddConsultationAsync(emergency);

            if (added is null)
                return BadRequest(new ApiResponse(400, "an error occured during adding data"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new ConsultationDto
                {
                    PaymentIntentId = added.PaymentIntentId,
                    ClientSecret = added.ClientSecret,
                    Doctor = await _doctorManager.FindByIdAsync(added.FK_DoctorId),
                    Patient = await _patientManager.FindByIdAsync(added.FK_PatientId),
                    DateTime = added.DateTime,
                    Type = added.Type
                }
            }) ;
        }

        [HttpPost("cancel")] // POST: /api/consultation/cancel
        public async Task<ActionResult<ApiResponse>> CancelConsultation([FromBody]PatientConsultation consultation)
        {
            var rowsAffected = await _consultationService.DeleteAsync(consultation);

            if (rowsAffected == 0) 
                return NotFound(new ApiResponse(404, "no data matches found"));

            if (rowsAffected < 0)
                return BadRequest(new ApiResponse(400, "error occured during cancel process"));

            return Ok(new ApiResponse(200, "consultation canceled successfully"));
        }

        /*
         *      1. Consultation (add, cancel) [DONE]
         *      2. Appointment
         *      3. Medicine Refill
         *      4. CollectPatientHistory
         */
    }
}
