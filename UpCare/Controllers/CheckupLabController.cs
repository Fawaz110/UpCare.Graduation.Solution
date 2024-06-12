using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpCare.DTOs;
using UpCare.DTOs.LabDTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class CheckupLabController : BaseApiController
    {
        private readonly UserManager<CheckupLab> _userManager;
        private readonly IAuthServices _authServices;
        private readonly SignInManager<CheckupLab> _signInManager;

        public CheckupLabController(
            UserManager<CheckupLab> userManager,
            IAuthServices authServices,
            SignInManager<CheckupLab> signInManager) 
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
        }
        [HttpPost("login")] // POST: /api/checkupLab/login
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
                FirstName = user.Name,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "checkupLab"
            });
        }
        [HttpPost("add")]// POST: /api/checkupLab/add
        public async Task<ActionResult<UserDto>> Register(CheckupLabRegisterDto model)
        {
            var user = new CheckupLab()
            {
                Name = model.FirstName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Location = model.Location,
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
                FirstName = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "checkupLab"
            });
        }
        [HttpGet("search")] // GET: api/checkupLab/search?nameSearchTerm
        public async Task<ActionResult<List<CheckupLab>>> Search([FromQuery] string nameSearchTerm)
        {
            var checkupLabs = await _userManager.Users.Where(p => p.Name.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())).AsNoTracking().ToListAsync();
            if (checkupLabs.Count() == 0)
                return BadRequest(new ApiResponse(404, "No checkupLabs matches search term"));

            return Ok(checkupLabs);
        }
    }
}
