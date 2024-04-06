
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;

namespace Core.Services.Contract
{
    public interface IAuthServices
    {
        Task<string> CreateTokenAsync(Patient patient, UserManager<Patient> userManager);
        Task<string> CreateTokenAsync(Doctor doctor, UserManager<Doctor> userManager);
    }
}
