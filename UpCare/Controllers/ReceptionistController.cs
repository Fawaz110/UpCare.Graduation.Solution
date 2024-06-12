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
    public class ReceptionistController : BaseApiController
    {
        private readonly UserManager<Receptionist> _userManager;
        private readonly IAuthServices _authServices;
        private readonly SignInManager<Receptionist> _signInManager;

        public ReceptionistController(
            UserManager<Receptionist> userManager,
            IAuthServices authServices,
            SignInManager<Receptionist> signInManager)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
        }
        [HttpPost("login")] // POST: /api/receptionist/login
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
                UserRole = "receptionist"
            });
        }
        [HttpPost("add")]// POST: /api/receptionist/add
        public async Task<ActionResult<UserDto>> Register(ReceptionistRegisterDto model)
        {
            var user = new Receptionist()
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
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "receptionist"
            });
        }
        [HttpGet("search")] // GET: api/receptionist/search?nameSearchTerm
        public async Task<ActionResult<List<Receptionist>>> Search([FromQuery] string nameSearchTerm)
        {
            var receptionists = await _userManager.Users.Where(p => p.FirstName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())
                                                            || p.LastName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower()))
                                                   .AsNoTracking().ToListAsync();
            if (receptionists.Count() == 0)
                return BadRequest(new ApiResponse(404, "No receptionists matches search term"));

            return Ok(receptionists);
        }

        [HttpGet("all")] // GET: /api/receptionist/all
        public async Task<ActionResult<IEnumerable<Receptionist>>> GetAll()
        {
            var receptionists = await _userManager.Users.ToListAsync();

            if (receptionists.Count() == 0)
                return NotFound(new ApiResponse(404, "there are no receptionists found"));

            return Ok(receptionists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Receptionist>> GetSpecificDoctor(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);

            if (doctor is null)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(doctor);
        }

        /*
         *      1. Rooms
         *      2. RoomsForPatients
         */
    }
}
