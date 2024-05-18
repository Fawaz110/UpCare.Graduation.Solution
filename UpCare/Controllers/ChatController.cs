using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repository.UpCareData;
using Service;
using UpCare.DTOs;
using UpCare.Errors;
using UpCare.Hubs;

namespace UpCare.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly UpCareDbContext _context;
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(
            UpCareDbContext context, 
            UserManager<Admin> adminManager,
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager,
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _adminManager = adminManager;
            _doctorManager = doctorManager;
            _patientManager = patientManager;
            _hubContext = hubContext;
        }

        [HttpPost("from-admin/to-doctor")] // POST: /api/chat/from-admin/to-doctor
        public async Task<ActionResult<SucceededToAdd>> SendFromAdminToDoctor([FromBody] Message model)
        {
            var admin = await _adminManager.FindByIdAsync(model.SenderId);

            if (admin is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            var doctor = await _doctorManager.FindByIdAsync(model.ReceiverId);

            if (doctor is null)
                return BadRequest(new ApiResponse(400, "bad request"));

            await _hubContext.Clients.User(model.ReceiverId).SendAsync("receiveMessage", model.SenderId, model.Content);

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new MessageDto
                {
                    Content = model.Content,
                    DateTime = model.DateTime,
                    Id = model.Id,
                    Receiver = await _doctorManager.FindByIdAsync(model.ReceiverId),
                    Sender = await _adminManager.FindByIdAsync(model.SenderId)
                }
            }); ;
        }
    }
}
