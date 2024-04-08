
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;

namespace Core.Services.Contract
{
    public interface IAuthServices
    {
        Task<string> CreateTokenAsync(Patient patient, UserManager<Patient> userManager);
        Task<string> CreateTokenAsync(Doctor doctor, UserManager<Doctor> userManager);
        Task<string> CreateTokenAsync(Nurse user, UserManager<Nurse> userManager);
        Task<string> CreateTokenAsync(Receptionist user, UserManager<Receptionist> userManager);
        Task<string> CreateTokenAsync(RadiologyLab user, UserManager<RadiologyLab> userManager);
        Task<string> CreateTokenAsync(CheckupLab checkupLab, UserManager<CheckupLab> userManager);
        Task<string> CreateTokenAsync(Admin admin, UserManager<Admin> userManager);
        Task<string> CreateTokenAsync(Pharmacy pharmacy, UserManager<Pharmacy> userManager);
    }
}
