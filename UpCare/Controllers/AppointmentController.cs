using Core.Services.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;
using UpCare.DTOs;
using UpCare.DTOs.PatientDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class AppointmentController : BaseApiController
    {
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager,
            IAppointmentService appointmentService)
        {
            _doctorManager = doctorManager;
            _patientManager = patientManager;
            _appointmentService = appointmentService;
        }

        [HttpPost("book")] // POST: /api/appointment/book
        public async Task<ActionResult<SucceededToAdd>> BookAppointment([FromBody] PatientAppointment patientAppointment)
        {
            var result = await _appointmentService.AddAppointmentAsync(patientAppointment);

            if (result is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            var added = new AppointmentDto
            {
                DateTime = result.DateTime,
                Type = result.Type,
                ClientSecret = result.ClientSecret,
                PaymentIntentId = result.PaymentIntentId,
                Doctor = await _doctorManager.FindByIdAsync(patientAppointment.FK_DoctorId),
                Patient = await _patientManager.FindByIdAsync(patientAppointment.FK_PatientId)
            };

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = added
            });
        }

        [HttpGet("dr/passed/{id}")] // GET: /api/appointment/dr/passed/{id}
        public async Task<ActionResult<List<AppointmentDto>>> GetPassedAppointments(string id)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return BadRequest(new ApiResponse(404, "no doctor matches found"));

            var result = await _appointmentService.GetAllByDoctorIdAsync(id);

            if (result is null)
                return NotFound(new ApiResponse(404, "no data found"));

            var appointments = result?.Where(c => c.DateTime < DateTime.UtcNow).ToList();

            if (appointments.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<AppointmentDto>();

            foreach (var consultation in appointments)
            {
                var con = new AppointmentDto
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

        [HttpGet("dr/next/{id}")] // GET: /api/appointment/dr/passed/{id}
        public async Task<ActionResult<List<AppointmentDto>>> GetComingAppointments(string id)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return BadRequest(new ApiResponse(404, "no doctor matches found"));

            var result = await _appointmentService.GetAllByDoctorIdAsync(id);

            if (result is null)
                return NotFound(new ApiResponse(404, "no data found"));

            var appointments = result?.Where(c => c.DateTime > DateTime.UtcNow).ToList();

            if (appointments.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<AppointmentDto>();

            foreach (var consultation in appointments)
            {
                var con = new AppointmentDto
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

        [HttpGet("pt/passed/{id}")] // GET: /api/appointment/dr/passed/{id}
        public async Task<ActionResult<List<AppointmentDto>>> GetPatientPassedAppointments(string id)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return BadRequest(new ApiResponse(404, "no patient matches found"));

            var result = await _appointmentService.GetAllByPatientIdAsync(id);

            if (result is null)
                return NotFound(new ApiResponse(404, "no data found"));

            var appointments = result?.Where(c => c.DateTime < DateTime.UtcNow).ToList();

            if (appointments.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<AppointmentDto>();

            foreach (var consultation in appointments)
            {
                var con = new AppointmentDto
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

        [HttpGet("pt/next/{id}")] // GET: /api/appointment/dr/passed/{id}
        public async Task<ActionResult<List<AppointmentDto>>> GetPatientComingAppointments(string id)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return BadRequest(new ApiResponse(404, "no patient matches found"));

            var result = await _appointmentService.GetAllByPatientIdAsync(id);

            if (result is null)
                return NotFound(new ApiResponse(404, "no data found"));

            var appointments = result?.Where(c => c.DateTime > DateTime.UtcNow).ToList();

            if (appointments.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<AppointmentDto>();

            foreach (var consultation in appointments)
            {
                var con = new AppointmentDto
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

        [HttpPost("cancel")] // DELETE: /api/appintment/cancel
        public async Task<ActionResult<SucceededToAdd>> Cancel([FromBody]PatientAppointment patientAppointment)
        {
            var rowsAffected = await _appointmentService.DeleteAsync(patientAppointment);

            if (rowsAffected == 0)
                return NotFound(new ApiResponse(404, "no data matches found"));

            if (rowsAffected < 0)
                return BadRequest(new ApiResponse(400, "error occured during cancel process"));

            return Ok(new ApiResponse(200, "appointment canceled successfully"));
        }

    }
}
