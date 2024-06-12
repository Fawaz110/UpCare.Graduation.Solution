using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpCare.DTOs;
using UpCare.DTOs.LabDTOs;
using UpCare.DTOs.StaffDTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class RadiologyLabController : BaseApiController
    {
        private readonly UserManager<RadiologyLab> _userManager;
        private readonly IAuthServices _authServices;
        private readonly SignInManager<RadiologyLab> _signInManager;

        public RadiologyLabController(
            UserManager<RadiologyLab> userManager,
            IAuthServices authServices,
            SignInManager<RadiologyLab> signInManager)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
        }
        [HttpPost("login")] // POST: /api/radiologyLab/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            return Ok(new UserDto()
            {Id = user.Id,
                FirstName=user.Name,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "radiologyLab"
            });
        }
        [HttpPost("add")]// POST: /api/radiologyLab/add
        public async Task<ActionResult<UserDto>> Register(RadiologyLabRegisterDto model)
        {
            var user = new RadiologyLab()
            {Name=model.FirstName,
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
                UserRole = "radiologyLab"
            });
        }
        [HttpGet("search")] // GET: api/radiologyLab/search?nameSearchTerm
        public async Task<ActionResult<List<RadiologyLab>>> Search([FromQuery] string nameSearchTerm)
        {
            var radiologLabs = await _userManager.Users.Where(p => p.Name.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())).AsNoTracking().ToListAsync();
            if (radiologLabs.Count() == 0)
                return BadRequest(new ApiResponse(404, "No radiologLabs matches search term"));

            return Ok(radiologLabs);
        }
    }
}
