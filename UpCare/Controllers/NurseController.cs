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
    public class NurseController : BaseApiController
    {
        private readonly UserManager<Nurse> _userManager;
        private readonly IAuthServices _authServices;
        private readonly SignInManager<Nurse> _signInManager;

        public NurseController(
            UserManager<Nurse> userManager,
            IAuthServices authServices,
            SignInManager<Nurse> signInManager 
            )
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
        }
        [HttpPost("login")] // POST: /api/nurse/login
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
                UserRole = "nurse"
            });
        }
        [HttpPost("add")]// POST: /api/nurse/add
        public async Task<ActionResult<UserDto>> Register(NurseRegisterDto model)
        {
            var user = new Nurse()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
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
                UserRole = "nurse"
            });
        }
        [HttpGet("search")] // GET: api/nurse/search?nameSearchTerm
        public async Task<ActionResult<List<Nurse>>> Search([FromQuery] string nameSearchTerm)
        {
            var nurses = await _userManager.Users.Where(p => p.FirstName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())
                                                            || p.LastName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower()))
                                                   .AsNoTracking().ToListAsync();
            if (nurses.Count() == 0)
                return BadRequest(new ApiResponse(404, "No nurses matches search term"));

            return Ok(nurses);
        }
    }
}
