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

        public ConsultationController(
            IConsultationService consultationService, 
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager)
        {
            _consultationService = consultationService;
            _patientManager = patientManager;
            _doctorManager = doctorManager;
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
                Doctor = await _doctorManager.FindByIdAsync(consultation.FK_DoctorId),
                Patient = await _patientManager.FindByIdAsync(consultation.FK_PatientId)
            };

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = objectToReturn
            });
        }

        /*
         *      1. Consultation (add, cancel)
         *      2. Appointment
         *      3. Medicine Refill
         *      4. CollectPatientHistory
         */
    }
}
