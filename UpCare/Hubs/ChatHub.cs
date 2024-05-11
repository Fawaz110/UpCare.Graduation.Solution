using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using UpCare.DTOs;

namespace UpCare.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<Nurse> _nurseManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ChatHub(
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager,
            UserManager<Admin> adminManager,
            UserManager<Nurse> nurseManager,
            UserManager<IdentityUser> userManager)
        {
            _doctorManager = doctorManager;
            _patientManager = patientManager;
            _adminManager = adminManager;
            _nurseManager = nurseManager;
            _userManager = userManager;
        }
        //public async Task SendMessage(string recipientId, string message)
        //{
        //    var sender = await _userManager.GetUserAsync(Context.User);


        //}
    }
}
