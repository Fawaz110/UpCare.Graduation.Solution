using Core.Services.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.DTOs.StaffDTOs.Pharmacy;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class PharmacyController : BaseApiController
    {
        private readonly IAuthServices _authServices;
        private readonly SignInManager<Pharmacy> _signInManager;
        private readonly UserManager<Pharmacy> _userManager;

        public PharmacyController(
            IAuthServices authServices,
            SignInManager<Pharmacy> signInManager,
            UserManager<Pharmacy> userManager) 
        {
            _authServices = authServices;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")] // POST: /api/pharmacy/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            return Ok(new PharmacyUserDto()
            {
                Id = user.Id,
                UserName = user.Name,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "pharmacy"
            });
        }

        [HttpPost("add")] // POST: /api/pharmacy/add
        public async Task<ActionResult<UserDto>> AddPharmacy(PharmacyRegisterDto model)
        {
            var user = new Pharmacy()
            {
                Name = model.Name,
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

            return Ok(new PharmacyUserDto()
            {
                UserName = user.Name,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserRole = "pharmacy"
            });
        }

        /*
         *      1. GetMedicineForPatient
         *      2. GetRefill
         *      
         */
    }
}
