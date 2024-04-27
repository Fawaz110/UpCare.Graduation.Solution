using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class FeedbackController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<Patient> _patientManager;

        public FeedbackController(
            IUnitOfWork unitOfWork,
            UserManager<Admin> adminManager,
            UserManager<Patient> patientManager)
        {
            _unitOfWork = unitOfWork;
            _adminManager = adminManager;
            _patientManager = patientManager;
        }

        [HttpGet("all")] // GET: /api/feedback/all?patientNameSearchTerm={string}
        public async Task<ActionResult<List<FeedbackDto>>> GetAll([FromQuery] string? patientNameSearchTerm)
        {
            var feedbacks = await _unitOfWork.Repository<Feedback>().GetAllAsync();

            if (feedbacks.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            List<FeedbackDto> result = new List<FeedbackDto>();

            foreach (var item in feedbacks)
            {
                var feedback = new FeedbackDto
                {
                    Id = item.Id,
                    Comment = item.Comment,
                    DateTime = item.DateTime,
                    Rate = item.Rate,
                    Patient = await _patientManager.FindByIdAsync(item.FK_PatientId)                    
                };
                if (!string.IsNullOrEmpty(patientNameSearchTerm))
                {
                    if ((feedback.Patient.FirstName.Trim().ToLower().Contains(patientNameSearchTerm.Trim().ToLower())
                        | feedback.Patient.LastName.Trim().ToLower().Contains(patientNameSearchTerm.Trim().ToLower())))
                        result.Add(feedback);
                }
                else
                    result.Add(feedback);                
            }

            return Ok(result);
        }

        [HttpGet("{id}")] // GET: /api/feedback/{id}
        public async Task<ActionResult<FeedbackDto>> GetById(int id)
        {
            var feedback = await _unitOfWork.Repository<Feedback>().GetByIdAsync(id);

            if (feedback is null) 
                return NotFound(new ApiResponse(404, "no data matches found"));


            return Ok(new FeedbackDto
            {
                Id = id,
                Comment = feedback.Comment,
                DateTime = feedback.DateTime,
                Rate = feedback.Rate,
                Patient = await _patientManager.FindByIdAsync(feedback.FK_PatientId)
            });
        }

        [HttpPost("add")] // POST: /api/feedback/add
        public async Task<ActionResult<SucceededToAdd>> AddFeedback([FromBody]Feedback feedback)
        {
            var patient = await _patientManager.FindByIdAsync(feedback.FK_PatientId);

            if (patient is null)
                return BadRequest(new ApiResponse(400, "incoreect data entered"));

            await _unitOfWork.Repository<Feedback>().Add(feedback);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return BadRequest(new ApiResponse(400, "error occured during adding data"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new FeedbackDto
                {
                    Id = feedback.Id,
                    Comment = feedback.Comment,
                    Rate = feedback.Rate,
                    DateTime = feedback.DateTime,
                    Patient = await _patientManager.FindByIdAsync(feedback.FK_PatientId)
                }
            });
        }
    }
}
