using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<Admin> _userManager;
        private readonly SignInManager<Admin> _signInManager;
        private readonly IAuthServices _authServices;

        public AdminController(
            UserManager<Admin> userManager,
            SignInManager<Admin> signInManager,
            IAuthServices authServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
        }

        [HttpPost("login")] // POST: /api/admin/login
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
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "admin"
            });
        }

        [HttpGet("{id}")] // GET: /api/admin/{id}
        public async Task<ActionResult<Admin>> GetSpecificAdmin(string id)
        {
            var admin = await _userManager.FindByIdAsync(id);

            if (admin is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            return Ok(admin);
        }

        /*
         *      1. Operation (Add, Update, Delete)
         *      2. Add Operation To Doctor And Patient (DoctorDoOperation)
         */
    }
}
