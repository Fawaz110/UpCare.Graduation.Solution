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

        [HttpGet("admin/receive/doctors")] // GET: /api/chat/receive?role={number}&id={string}
        public async Task<ActionResult<List<MessagePackageToReturn>>> GetDoctorsMessages([FromQuery] string id, [FromQuery] MessagerRole? role = MessagerRole.Admin)
        {
            var admin = await _adminManager.FindByIdAsync(id);

            if (admin is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Admin)
                return BadRequest(new ApiResponse(400, "request only allowed for admin"));

            var messages = await _context.Set<Message>().Where(x => (x.SenderId == id || x.ReceiverId == id))
                                                        .Select(x => new MessageToReturnDto
                                                        {
                                                            ReceiverId = x.ReceiverId,
                                                            ReceiverRole = x.ReceiverRole,
                                                            SenderId = x.SenderId,
                                                            SenderRole = x.SenderRole,
                                                            Content = x.Content,
                                                            DateTime = x.DateTime,
                                                            isSent = (x.SenderId == id) ? true : false
                                                        }).OrderByDescending(x => x.DateTime).ToListAsync();

            var groupedList = messages.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId);

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

                itemToAdd.Messages = itemToAdd.Messages.OrderByDescending(x => x.DateTime).ToList();

                mappedToReturn.Add(itemToAdd);
            }

            return Ok(mappedToReturn);

            ///var list = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
            ///                                        || (x.ReceiverId == id && x.ReceiverRole == role)))
            ///                            .Select(x => new MessageToReturnDto
            ///                            {
            ///                                Content = x.Content,
            ///                                DateTime = x.DateTime,
            ///                                ReceiverId = x.ReceiverId,
            ///                                SenderId = x.SenderId,
            ///                                ReceiverRole = x.ReceiverRole,
            ///                                SenderRole = x.SenderRole,
            ///                                isSent = (x.SenderId == id) ? true : false
            ///                            })
            ///                            .OrderByDescending(x => x.DateTime)
            ///                            .Where(x => (x.SenderId == id) ? x.ReceiverRole == MessagerRole.Doctor : x.SenderRole == MessagerRole.Doctor)
            ///                            .ToListAsync();

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

        [HttpGet("admin/receive/doctor/{doctorId}")] // GET: /api/chat/admin/receive/doctor/{doctorId}?id={string}&role={int}
        public async Task<ActionResult<List<MessageToReturnDto>>> GetMessagesForSpecificDoctor(string doctorId, [FromQuery] string id,[FromQuery] MessagerRole? role = MessagerRole.Admin)
        {
            var admin = await _adminManager.FindByIdAsync(id);

            if (admin is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Admin)
                return BadRequest(new ApiResponse(400, "request only allowed for admins"));

            var doctor = await _doctorManager.FindByIdAsync(doctorId);

            if (doctor is null)
                return NotFound(new ApiResponse(404, "no doctor matches found"));

            var messages = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.ReceiverId == doctorId) || (x.ReceiverId == id && x.SenderId == doctorId)))
                                                        .Select(x => new MessageToReturnDto
                                                        {
                                                            Content = x.Content,
                                                            DateTime = x.DateTime,
                                                            ReceiverId = x.ReceiverId,
                                                            ReceiverRole = x.ReceiverRole,
                                                            SenderId = x.SenderId,
                                                            SenderRole = x.SenderRole,
                                                            isSent = (x.SenderId == id) ? true : false
                                                        }).OrderByDescending(x => x.DateTime).ToListAsync();

            if (messages.Count() == 0)
                return NotFound(new ApiResponse(404, "no messages found"));

            return Ok(messages);
        }

        [HttpGet("doctor/receive/admins")] // GET: /api/chat/receive?role={number}&id={string}
        public async Task<ActionResult<List<MessagePackage>>> GetAdminsMessages([FromQuery] string id, [FromQuery] MessagerRole? role = MessagerRole.Doctor)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Doctor)
                return BadRequest(new ApiResponse(400, "request only allowed for doctors"));

            var messages = await _context.Set<Message>().Where(x => (id == x.SenderId || id == x.ReceiverId))
                                                        .Select(x => new MessageToReturnDto
                                                        {
                                                            Content = x.Content,
                                                            SenderId = x.SenderId,
                                                            ReceiverId = x.ReceiverId,
                                                            DateTime = x.DateTime,
                                                            ReceiverRole = x.ReceiverRole,
                                                            SenderRole = x.SenderRole,
                                                            isSent = (id == x.SenderId) ? true : false
                                                        }).OrderByDescending(x => x.DateTime).ToListAsync();

            var groupedList = messages.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId);

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

                itemToAdd.Messages = itemToAdd.Messages.OrderByDescending(x => x.DateTime).ToList();

                mappedToReturn.Add(itemToAdd);
            }

            return Ok(mappedToReturn);

            ///var list = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
            ///                                        || (x.ReceiverId == id && x.ReceiverRole == role)))
            ///                            .Select(x => new MessageToReturnDto
            ///                            {
            ///                                Content = x.Content,
            ///                                DateTime = x.DateTime,
            ///                                ReceiverId = x.ReceiverId,
            ///                                SenderId = x.SenderId,
            ///                                ReceiverRole = x.ReceiverRole,
            ///                                SenderRole = x.SenderRole,
            ///                                isSent = (x.SenderId == id) ? true : false
            ///                            })
            ///                            .OrderByDescending(x => x.DateTime)
            ///                            .Where(x => (x.SenderId == id) ? x.ReceiverRole == MessagerRole.Admin : x.SenderRole == MessagerRole.Admin)
            ///                            .ToListAsync();

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

        [HttpGet("doctor/receive/admin/{adminId}")] // GET: /api/chat/doctor/receive/admin/{adminId}?id={string}&role={int}
        public async Task<ActionResult<List<MessageToReturnDto>>> GetMessagesBetweenAdminAndDoctor([FromQuery]string id, string adminId, [FromQuery] MessagerRole role = MessagerRole.Doctor)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Doctor)
                return BadRequest(new ApiResponse(400, "request only allowed for doctors"));

            var admin = await _adminManager.FindByIdAsync(adminId);

            if (admin is null)
                return NotFound(new ApiResponse(404, "no admin matches found"));

            var messages = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.ReceiverId == adminId) || (x.ReceiverId == id && x.SenderId == adminId)))
                                                        .Select(x => new MessageToReturnDto
                                                        {
                                                            Content = x.Content,
                                                            DateTime = x.DateTime,
                                                            ReceiverId = x.ReceiverId,
                                                            ReceiverRole = x.ReceiverRole,
                                                            SenderId = x.SenderId,
                                                            SenderRole = x.SenderRole,
                                                            isSent = (x.SenderId == id) ? true : false
                                                        }).OrderByDescending(x => x.DateTime).ToListAsync();

            if (messages.Count() == 0)
                return NotFound(new ApiResponse(404, "no messages found"));

            return Ok(messages);
        }

        [HttpGet("doctor/receive/patients")] // GET: /api/chat/receive?role={number}&id={string}
        public async Task<ActionResult<List<MessagePackage>>> GetPatientsMessages([FromQuery] string id, [FromQuery] MessagerRole? role = MessagerRole.Doctor)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Doctor)
                return BadRequest(new ApiResponse(400, "request only allowed for doctors"));

            var messages = await _context.Set<Message>().Where(x => (x.SenderId == id || x.ReceiverId == id))
                                                        .Select(x => new MessageToReturnDto
                                                        {
                                                            Content = x.Content,
                                                            DateTime = x.DateTime,
                                                            ReceiverId = x.ReceiverId,
                                                            SenderId = x.SenderId,
                                                            ReceiverRole = x.ReceiverRole,
                                                            SenderRole = x.SenderRole,
                                                            isSent = (x.SenderRole == role) ? true : false
                                                        }).OrderByDescending(x => x.DateTime).ToListAsync();

            ///var list = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
            ///                                        || (x.ReceiverId == id && x.ReceiverRole == role)))
            ///                            .Select(x => new MessageToReturnDto
            ///                            {
            ///                                Content = x.Content,
            ///                                DateTime = x.DateTime,
            ///                                ReceiverId = x.ReceiverId,
            ///                                SenderId = x.SenderId,
            ///                                ReceiverRole = x.ReceiverRole,
            ///                                SenderRole = x.SenderRole,
            ///                                isSent = (x.SenderId == id) ? true : false
            ///                            })
            ///                            .OrderByDescending(x => x.DateTime)
            ///                            .Where(x => (x.SenderId == id) ? x.ReceiverRole == MessagerRole.Admin : x.SenderRole == MessagerRole.Admin)
            ///                            .ToListAsync();

            var groupedList = messages.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId);

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

                itemToAdd.Messages = itemToAdd.Messages.OrderByDescending(x => x.DateTime).ToList();

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

        [HttpGet("doctor/receive/patient/{patientId}")] // GET: /api/chat/receive?role={number}&id={string}
        public async Task<ActionResult<List<MessageToReturnDto>>> GetSpecificPatientMessages([FromQuery] string id, string patientId, [FromQuery] MessagerRole? role = MessagerRole.Doctor)
       {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Doctor)
                return BadRequest(new ApiResponse(400, "request only allowed for doctors"));

            var patient = await _patientManager.FindByIdAsync(patientId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "there is no patient matches"));

            var messages = await _context.Messages.Where(msg => (msg.SenderId == patientId && msg.ReceiverId == id)
                                                       || (msg.ReceiverId == patientId && msg.SenderId == id))
                                            .Select(msg => new MessageToReturnDto
                                            {
                                                Content = msg.Content,
                                                SenderId = msg.SenderId,
                                                ReceiverId = msg.ReceiverId,
                                                DateTime = msg.DateTime,
                                                ReceiverRole = msg.ReceiverRole,
                                                SenderRole = msg.SenderRole,
                                                isSent = (msg.SenderId == id) ? true : false
                                            }).OrderByDescending(msg => msg.DateTime).ToListAsync();

            return Ok(messages);

            ///var list = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
            ///                                        || (x.ReceiverId == id && x.ReceiverRole == role)))
            ///                            .Select(x => new MessageToReturnDto
            ///                            {
            ///                                Content = x.Content,
            ///                                DateTime = x.DateTime,
            ///                                ReceiverId = x.ReceiverId,
            ///                                SenderId = x.SenderId,
            ///                                ReceiverRole = x.ReceiverRole,
            ///                                SenderRole = x.SenderRole,
            ///                                isSent = (x.SenderId == id) ? true : false
            ///                            })
            ///                            .OrderByDescending(x => x.DateTime)
            ///                            .Where(x => (x.SenderId == id) ? x.ReceiverRole == MessagerRole.Doctor
            ///                                                           : x.SenderRole == MessagerRole.Doctor)
            ///                            .ToListAsync();
            
            /// var groupedList = list.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId).FirstOrDefault();
            ///var mappedToReturn = new List<MessagePackageToReturn>();
            ///foreach (var group in groupedList)
            ///{
            ///    var itemToAdd = new MessagePackageToReturn();
            ///    itemToAdd.ClientId = group.Key;
            ///    ///if (firstInGroup.SenderRole == role || firstInGroup.SenderId == id)
            ///    ///    keyRole = firstInGroup.ReceiverRole;
            ///    ///else
            ///    ///    keyRole = firstInGroup.SenderRole;
            
            ///    foreach (var item in group)
            ///        itemToAdd.Messages.Add(item);
            ///    itemToAdd.Messages.Reverse();
            ///    mappedToReturn.Add(itemToAdd);
            ///}
            ///if (groupedList is null)
            ///    return NotFound(new ApiResponse(404, "no messages found"));

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

        [HttpGet("patient/receive/doctors")] // GET: /api/chat/receive?role={number}&id={string}
        public async Task<ActionResult<List<MessagePackageToReturn>>> GetDoctorMessagesForPatients([FromQuery] string id, [FromQuery] MessagerRole? role = MessagerRole.Patient)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Patient)
                return BadRequest(new ApiResponse(400, "request only allowed for patients"));

            var messages = await _context.Set<Message>().Where(m => (m.SenderId == id || m.ReceiverId == id))
                                                        .Select(x => new MessageToReturnDto
                                                                {
                                                                    Content = x.Content,
                                                                    DateTime = x.DateTime,
                                                                    ReceiverId=x.ReceiverId,
                                                                    SenderId = x.SenderId,
                                                                    isSent = (x.SenderRole == MessagerRole.Patient)? true: false
                                                                }).OrderBy(x => x.DateTime).ToListAsync();

            

            var groupedList = messages.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId);

            var mappedToReturn = new List<MessagePackageToReturn>();

            foreach (var group in groupedList)
            {
                var itemToAdd = new MessagePackageToReturn();

                itemToAdd.ClientId = group.Key;

                foreach (var item in group)
                    itemToAdd.Messages.Add(item);

                itemToAdd.Messages.Reverse();

                mappedToReturn.Add(itemToAdd);
            }

            return Ok(mappedToReturn);

            ///var list = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
            ///                                        || (x.ReceiverId == id && x.ReceiverRole == role)))
            ///                            .Select(x => new MessageToReturnDto
            ///                            {
            ///                                Content = x.Content,
            ///                                DateTime = x.DateTime,
            ///                                ReceiverId = x.ReceiverId,
            ///                                SenderId = x.SenderId,
            ///                                ReceiverRole = x.ReceiverRole,
            ///                                SenderRole = x.SenderRole,
            ///                                isSent = (x.SenderId == id) ? true : false
            ///                            })
            ///                            .OrderByDescending(x => x.DateTime)
            ///                            .Where(x => (x.SenderId == id) ? x.ReceiverRole == MessagerRole.Admin : x.SenderRole == MessagerRole.Admin)
            ///                            .ToListAsync(); 
            
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

        [HttpGet("patient/receive/doctor/{doctorId}")] // GET: /api/chat/patient/receive/doctor/{doctorId}?role=1&id={string}
        public async Task<ActionResult<List<MessageToReturnDto>>> GetSpecificDoctorMessages([FromQuery] string id, string doctorId, [FromQuery] MessagerRole? role = MessagerRole.Doctor)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            if (role != MessagerRole.Patient)
                return BadRequest(new ApiResponse(400, "request only allowed for patients"));

            var doctor = await _doctorManager.FindByIdAsync(doctorId);

            if (doctor is null)
                return NotFound(new ApiResponse(404, "there is no doctor matches"));

            var messages = await _context.Messages.Where(msg => (msg.SenderId == doctorId && msg.ReceiverId == id)
                                                       || (msg.ReceiverId == doctorId && msg.SenderId == id))
                                            .Select(msg => new MessageToReturnDto
                                            {
                                                Content = msg.Content,
                                                SenderId = msg.SenderId,
                                                ReceiverId = msg.ReceiverId,
                                                DateTime = msg.DateTime,
                                                ReceiverRole = msg.ReceiverRole,
                                                SenderRole = msg.SenderRole,
                                                isSent = (msg.SenderId == id) ? true : false
                                            }).OrderByDescending(msg => msg.DateTime).ToListAsync();

            return Ok(messages);

            ///var list = await _context.Set<Message>().Where(x => ((x.SenderId == id && x.SenderRole == role)
            ///                                        || (x.ReceiverId == id && x.ReceiverRole == role)))
            ///                            .Select(x => new MessageToReturnDto
            ///                            {
            ///                                Content = x.Content,
            ///                                DateTime = x.DateTime,
            ///                                ReceiverId = x.ReceiverId,
            ///                                SenderId = x.SenderId,
            ///                                ReceiverRole = x.ReceiverRole,
            ///                                SenderRole = x.SenderRole,
            ///                                isSent = (x.SenderId == id) ? true : false
            ///                            })
            ///                            .OrderByDescending(x => x.DateTime)
            ///                            .Where(x => (x.SenderId == id) ? x.ReceiverRole == MessagerRole.Doctor
            ///                                                           : x.SenderRole == MessagerRole.Doctor)
            ///                            .ToListAsync();

            /// var groupedList = list.GroupBy(x => (id == x.SenderId) ? x.ReceiverId : x.SenderId).FirstOrDefault();
            ///var mappedToReturn = new List<MessagePackageToReturn>();
            ///foreach (var group in groupedList)
            ///{
            ///    var itemToAdd = new MessagePackageToReturn();
            ///    itemToAdd.ClientId = group.Key;
            ///    ///if (firstInGroup.SenderRole == role || firstInGroup.SenderId == id)
            ///    ///    keyRole = firstInGroup.ReceiverRole;
            ///    ///else
            ///    ///    keyRole = firstInGroup.SenderRole;

            ///    foreach (var item in group)
            ///        itemToAdd.Messages.Add(item);
            ///    itemToAdd.Messages.Reverse();
            ///    mappedToReturn.Add(itemToAdd);
            ///}
            ///if (groupedList is null)
            ///    return NotFound(new ApiResponse(404, "no messages found"));

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
