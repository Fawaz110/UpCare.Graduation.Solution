using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpCare.DTOs;
using UpCare.DTOs.StaffDTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class DoctorController : BaseApiController
    {
        private readonly UserManager<Doctor> _userManager;
        private readonly IAuthServices _authServices;
        private readonly SignInManager<Doctor> _signInManager;

        public DoctorController(
            UserManager<Doctor> userManager, 
            IAuthServices authServices, 
            SignInManager<Doctor> signInManager)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
        }

        [HttpPost("login")] // POST: /api/doctor/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "doctor"
            });
        }

        [HttpPost("add")]// POST: /api/doctor/add
        public async Task<ActionResult<UserDto>> Register(DoctorRegisterDto model)
        {
            var user = new Doctor()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Speciality = model.Speciality,
                IsSurgeon = model.IsSurgeon,
                ConsultationPrice = model.ConsultationPrice,
                AppointmentPrice = model.AppointmentPrice,
                FK_AdminId = model.AdminId, 
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded is false)
            {
                var error = new ApiValidationErrorResponse();
                foreach (var item in result.Errors)
                {
                    error.Errors.Add(item.Code);
                }

                return BadRequest(error);
            }

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "doctor"
            });
        }
        [HttpGet("search")] // GET: api/doctor/search?nameSearchTerm=string
        public async Task<ActionResult<List<Doctor>>> Search([FromQuery] string nameSearchTerm)
        {
            var doctors = await _userManager.Users.Where(p => p.FirstName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())
                                                            || p.LastName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower()))
                                                   .AsNoTracking().ToListAsync();
            if (doctors.Count() == 0)
                return BadRequest(new ApiResponse(404, "No doctors matches search term"));

            return Ok(doctors);
        }
    }
}
