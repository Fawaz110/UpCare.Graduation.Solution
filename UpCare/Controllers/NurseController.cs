using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
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
        private readonly UserManager<Nurse> _nurseManager;
        private readonly IAuthServices _authServices;
        private readonly SignInManager<Nurse> _signInManager;
        private readonly UserManager<Patient> _patientManager;
        private readonly INurseCareService _nurseCareService;
        private readonly IUnitOfWork _unitOfWork;

        public NurseController(
            UserManager<Nurse> nurseManager,
            SignInManager<Nurse> signInManager,
            UserManager<Patient> patientManager,
            INurseCareService nurseCareService,
            IAuthServices authServices,
            IUnitOfWork unitOfWork)
        {
            _nurseManager = nurseManager;
            _authServices = authServices;
            _signInManager = signInManager;
            _patientManager = patientManager;
            _nurseCareService = nurseCareService;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("login")] // POST: /api/nurse/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _nurseManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401, "incorrect email or password"));
            return Ok(new UserDto()
            {Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = model.Email,
                Token = await _authServices.CreateTokenAsync(user, _nurseManager),
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

            var result = await _nurseManager.CreateAsync(user, model.Password);

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
                Token = await _authServices.CreateTokenAsync(user, _nurseManager),
                UserRole = "nurse"
            });
        }
        [HttpGet("search")] // GET: api/nurse/search?nameSearchTerm
        public async Task<ActionResult<List<Nurse>>> Search([FromQuery] string nameSearchTerm)
        {
            var nurses = await _nurseManager.Users.Where(p => p.FirstName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower())
                                                            || p.LastName.Trim().ToLower().Contains(nameSearchTerm.Trim().ToLower()))
                                                   .AsNoTracking().ToListAsync();
            if (nurses.Count() == 0)
                return BadRequest(new ApiResponse(404, "No nurses matches search term"));

            return Ok(nurses);
        }

        [HttpGet("all")] // GET: /api/nurse/all
        public async Task<ActionResult<IEnumerable<Nurse>>> GetAll()
        {
            var nurses = await _nurseManager.Users.ToListAsync();

            if (nurses.Count() == 0)
                return NotFound(new ApiResponse(404, "there are no nurses found"));

            return Ok(nurses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Nurse>> GetSpecificNurse(string id)
        {
            var nurse = await _nurseManager.FindByIdAsync(id);

            if (nurse is null)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(nurse);
        }

        [HttpPost("care/add")] // POST: /api/nurse/care/add
        public async Task<ActionResult<SucceededToAdd>> AddNurseCare(NurseCare model)
        {
            var nurse = await _nurseManager.FindByIdAsync(model.FK_NurseId);

            if (nurse is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            var patient = await _patientManager.FindByIdAsync(model.FK_PatientId);

            if (patient is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(model.FK_RoomId);

            if (room is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            var result = await _nurseCareService.AddAsync(model);

            if (result is null)
                return BadRequest(new ApiResponse(400, "an error occured during adding data"));

            var mapped = new NurseCareDto
            {
                Patient = patient,
                Room = room,
                Nurse = nurse,
                BeatPerMinute = result.BeatPerMinute,
                BloodPresure = result.BloodPresure,
                DateTime = result.DateTime,
                OxygenSaturation = result.OxygenSaturation,
                Suger = result.Suger,
                Notes = result.Notes
            };

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = mapped
            });
        }

        [HttpGet("care/records")] // GET: /api/nurse/care/records?patientId={string}&nurseId={string}&roomId={int}
        public async Task<ActionResult<List<NurseCareDto>>> GetNurseCareRecords(string patientId, int roomId)
        {
            var patient = await _patientManager.FindByIdAsync(patientId);

            if(patient is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync((int) roomId);

            if(room is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            var result = await _nurseCareService.GetNurseCareRecordsAsync(patientId, roomId);

            if (result.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MapToNurseCareDto(result);

            return Ok(mapped.OrderByDescending(x => x.DateTime));
        }

        [HttpDelete("care/delete")] // GET: /api/nurse/care/delete
        public async Task<ActionResult<ApiResponse>> DeleteNurseCareRecord(NurseCare model)
        {
            var nurse = await _nurseManager.FindByIdAsync(model.FK_NurseId);

            if (nurse is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));


            var patient = await _patientManager.FindByIdAsync(model.FK_PatientId);

            if (patient is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(model.FK_RoomId);

            if (room is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));


            var result = await _nurseCareService.DeleteAsync(model.FK_PatientId, model.FK_NurseId, model.FK_RoomId, model.DateTime);

            if (result <= 0)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(new ApiResponse(200, "data deleted successfully"));
        }


        // Private methods to map => NurseCareDto
        private async Task<List<NurseCareDto>> MapToNurseCareDto(List<NurseCare> data)
        {
            var result = new List<NurseCareDto>();

            foreach (var record in data)
            {
                var mapped = await MapToNurseCareDto(record);

                result.Add(mapped);
            }

            return result;
        }

        private async Task<NurseCareDto> MapToNurseCareDto(NurseCare data)
        {
            return new NurseCareDto
            {
                BeatPerMinute = data.BeatPerMinute,
                BloodPresure = data.BloodPresure,
                DateTime = data.DateTime,
                Notes = data.Notes,
                OxygenSaturation = data.OxygenSaturation,
                Suger = data.Suger,
                Room = await _unitOfWork.Repository<Room>().GetByIdAsync(data.FK_RoomId),
                Nurse = await _nurseManager.FindByIdAsync(data.FK_NurseId),
                Patient = await _patientManager.FindByIdAsync(data.FK_PatientId)
            };
        }

        /*
         *      1. NurseCare
         */
    }
}
