using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.PowerBI.Api.Models;
using Repository.UpCareData;
using UpCare.Helpers;

namespace UpCare.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<Nurse> _nurseManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UpCareDbContext _context;
        private readonly IMediator _mediator;

        public ChatHub(
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager,
            UserManager<Admin> adminManager,
            UserManager<Nurse> nurseManager,
            UserManager<IdentityUser> userManager,
            UpCareDbContext context,
            IMediator mediator)
        {
            _doctorManager = doctorManager;
            _patientManager = patientManager;
            _adminManager = adminManager;
            _nurseManager = nurseManager;
            _userManager = userManager;
            _context = context;
            _mediator = mediator;
        }
        //public async Task sendMessage(string adminId, string doctorId, string content)
        //{
        //    var sender = await _adminManager.FindByIdAsync(adminId);

        //    if (sender is null)
        //        return;

        //    var doctor = await _doctorManager.FindByIdAsync(doctorId);

        //    if (doctor is null)
        //        return;

        //    var message = new Message
        //    {
        //        SenderId = adminId,
        //        ReceiverId = doctorId,
        //        Content = content
        //    };

        //    await _context.Set<Message>().AddAsync(message);

        //    await _context.SaveChangesAsync();

        //    var doctorConnection = _context.Set<UserConnection>().FirstOrDefault(x => x.FK_UserId == doctor.Id);

        //    await Clients.All.SendAsync("newMessage", sender.FirstName, message);
        //}

        //public override Task OnConnectedAsync()
        //{
        //    var newConnection = new UserConnection
        //    {
        //        ConnectionId = Context.ConnectionId,
        //        FK_UserId = Context.UserIdentifier
        //    };

        //    _context.Set<UserConnection>().Add(newConnection);

        //    _context.SaveChanges();
            
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var connectionToRemove = _context.Set<UserConnection>().FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

        //    if(connectionToRemove != null) 
        //        _context.Set<UserConnection>().Remove(connectionToRemove);

        //    _context.SaveChanges();

        //    return base.OnDisconnectedAsync(exception);
        //}
    }
}
