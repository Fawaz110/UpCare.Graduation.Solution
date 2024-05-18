using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;
using Service;
using UpCare.DTOs;
using UpCare.DTOs.MessageDtos;
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

        [HttpPost("send")]
        public async Task<ActionResult<SucceededToAdd>> SendMessage([FromBody] MessageDto model)
        {
            var message = new Message();

            if (model.MessageType == MessageType.FromPatientToDoctor)
            {
                var patient = await _patientManager.FindByIdAsync(model.SenderId);

                if (patient is null) return Unauthorized(new ApiResponse(401, "unauthorized access"));

                var doctor = await _doctorManager.FindByIdAsync(model.ReceiverId);

                if (doctor is null) return BadRequest(new ApiResponse(400));

                message.ReceiverRole = MessagerRole.Doctor;
                message.SenderRole = MessagerRole.Patient;
            }
            else if (model.MessageType == MessageType.FromDoctorToPatient)
            {
                var doctor = await _doctorManager.FindByIdAsync(model.SenderId);

                if (doctor is null) return Unauthorized(new ApiResponse(401, "unauthorized access"));

                var patient = await _patientManager.FindByIdAsync(model.ReceiverId);

                if (patient is null) return BadRequest(new ApiResponse(400));

                message.ReceiverRole = MessagerRole.Patient;
                message.SenderRole = MessagerRole.Doctor;
            }
            else if (model.MessageType == MessageType.FromAdminToDoctor)
            {
                var admin = await _adminManager.FindByIdAsync(model.SenderId);

                if (admin is null) return Unauthorized(new ApiResponse(401, "unauthorized access"));

                var doctor = await _doctorManager.FindByIdAsync(model.ReceiverId);

                if (doctor is null) return BadRequest(new ApiResponse(400));

                message.ReceiverRole = MessagerRole.Doctor;
                message.SenderRole = MessagerRole.Admin;
            }
            else if (model.MessageType == MessageType.FromDoctorToAdmin)
            {
                var doctor = await _doctorManager.FindByIdAsync(model.SenderId);

                if (doctor is null) return Unauthorized(new ApiResponse(401, "unauthorized access"));

                var admin = await _adminManager.FindByIdAsync(model.ReceiverId);

                if (admin is null) return BadRequest(new ApiResponse(400));

                message.ReceiverRole = MessagerRole.Admin;
                message.SenderRole = MessagerRole.Doctor;
            }


            message.Content = model.Content;
            message.SenderId = model.SenderId;
            message.ReceiverId = model.ReceiverId;

            await _context.Set<Message>().AddAsync(message);

            var result = await _context.SaveChangesAsync();

            if (result < 0) return BadRequest(new ApiResponse(400, "error occured during sending message")); 

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = message
            });
        }


        [HttpGet("receive")] // GET: /api/chat/receive?role={number}&id={string}
        public async Task<ActionResult<List<MessagePackage>>> GetMessages([FromQuery] MessagerRole role, [FromQuery] string id)
        {
            var list = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
                                                               || (x.ReceiverId == id && x.ReceiverRole == role)))
                                                    .Select(x => new MessageToReturnDto
                                                    {
                                                        Content = x.Content,
                                                        DateTime = x.DateTime,
                                                        ReceiverId = x.ReceiverId,
                                                        SenderId = x.SenderId,
                                                        ReceiverRole = x.ReceiverRole,
                                                        SenderRole = x.SenderRole,
                                                        isSent = (x.SenderId == id) ? true : false
                                                    })
                                                    .OrderByDescending(x => x.DateTime).ToListAsync();

            var groupedList = list.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId);

            var mappedToReturn = new List<MessagePackageToReturn>();

            foreach (var group in groupedList)
            {
                var itemToAdd = new MessagePackageToReturn();

                itemToAdd.ClientId = group.Key;

                ///if (firstInGroup.SenderRole == role || firstInGroup.SenderId == id)
                ///    keyRole = firstInGroup.ReceiverRole;
                ///else
                ///    keyRole = firstInGroup.SenderRole;

                foreach (var item in group)
                    itemToAdd.Messages.Add(item);

                mappedToReturn.Add(itemToAdd);
            }

            return Ok(mappedToReturn);

            ///var result = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
            ///                                           || (x.ReceiverId == id && x.ReceiverRole == role)))
            ///                                  .OrderByDescending(x => x.DateTime).ToListAsync();
            ///if (result.Count() == 0)
            ///    return NotFound(new ApiResponse(404, "no data found"));
            ///var grouped = result.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId);
            ///var mapped = new List<MessagePackage>();
            ///foreach (var group in grouped)
            ///{
            ///    var package = new MessagePackage { Id = group.Key };
            ///    foreach (var item in group)
            ///        package.Messages.Add(item);
            ///}
            ///return Ok(mapped); 
        }

        //[HttpPost("from-admin/to-doctor")] // POST: /api/chat/from-admin/to-doctor
        //public async Task<ActionResult<SucceededToAdd>> SendFromAdminToDoctor([FromBody] Message model)
        //{
        //    var admin = await _adminManager.FindByIdAsync(model.SenderId);

        //    if (admin is null)
        //        return Unauthorized(new ApiResponse(401, "unauthorized access"));

        //    var doctor = await _doctorManager.FindByIdAsync(model.ReceiverId);

        //    if (doctor is null)
        //        return BadRequest(new ApiResponse(400, "bad request"));

        //    await _hubContext.Clients.All.SendAsync("newMessage", admin.FirstName, model.Content);

        //    return Ok(new SucceededToAdd
        //    {
        //        Message = "success",
        //        Data = new MessageDto
        //        {
        //            Content = model.Content,
        //            DateTime = model.DateTime,
        //            Id = model.Id,
        //            Receiver = await _doctorManager.FindByIdAsync(model.ReceiverId),
        //            Sender = await _adminManager.FindByIdAsync(model.SenderId)
        //        }
        //    });
        //}
    }
}
