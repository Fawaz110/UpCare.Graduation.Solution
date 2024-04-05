using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class PatientController : BaseApiController
    {
        private readonly UserManager<Patient> _userManager;
        private readonly IAuthServices _authServices;
        private readonly SignInManager<Patient> _signInManager;

        public PatientController(UserManager<Patient> userManager,IAuthServices authServices, SignInManager<Patient> signInManager)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
        }
        [HttpPost("login")] // POST: /api/patient/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user,_userManager)
            });
        }
        [HttpPost("register")]// POST: /api/patient/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new Patient()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                BloodType = model.BloodType,
                FK_ReceptionistId = model.ReceptionistId,
                Gender = (model.Gender == "male") ? Gender.Male : Gender.Female,
                //DateOfBirth = model.DateOfBirth,
            };
            //user.DateOfBirth.Day = model.DateOfBirth.
            var dayMonthYear = model.DateOfBirth.Split("-").Select(int.Parse).ToArray();

            if (dayMonthYear.Length == 3 &&
                dayMonthYear[0] >= 1 && dayMonthYear[0] <= 31 &&
                dayMonthYear[1] >= 1 && dayMonthYear[1] <= 12 && 
                dayMonthYear[2] >= 1900 && dayMonthYear[2] <= DateTime.Now.Year) 
                // Create DateTime object if values are valid
                user.DateOfBirth = new DateTime(dayMonthYear[2], dayMonthYear[1], dayMonthYear[0]);
            
            else
                return BadRequest(new ApiValidationErrorResponse());
            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded is false)
            {
                var error = new ApiValidationErrorResponse();
                foreach(var item in result.Errors)
                {
                    error.Errors.Add(item.Code);
                }

                return BadRequest(error);
            }

            return Ok(new UserDto()
            {
                FirstName=user.FirstName,
                LastName=user.LastName,
                Email=user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager)
            });
        }
    }
}
