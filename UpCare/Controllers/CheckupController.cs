using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;
using Service;
using UpCare.DTOs;
using UpCare.Errors;
using UpCare.Helpers;

namespace UpCare.Controllers
{
    public class CheckupController : BaseApiController
    {
        private readonly ICheckupService _checkupService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Patient> _patientManager;
        private readonly IConfiguration _configuration;
        private readonly UpCareDbContext _context;

        public CheckupController(
            ICheckupService checkupService,
            IUnitOfWork unitOfWork,
            UserManager<Patient> patientManager,
            IConfiguration configuration,
            UpCareDbContext context)
        {
            _checkupService = checkupService;
            _unitOfWork = unitOfWork;
            _patientManager = patientManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("all")] // GET: /api/checkup/all
        public async Task<ActionResult<List<Checkup>>> GetAll()
        {
            var checkupList = await _checkupService.GetAllAsync();

            if(checkupList.Count == 0)
                return NotFound(new ApiResponse(404, "no checkup items founded"));
            
            return Ok(checkupList);
        }

        [HttpGet("{id}")] // GET: /api/checkup/{id}
        public async Task<ActionResult<Checkup>> GetById(int id)
        {
            var checkup = await _checkupService.GetByIdAsync(id);

            if (checkup == null)
                return NotFound(new ApiResponse(404, "no checkup matches this id found"));

            return Ok(checkup);
        }

        [HttpPost("add")] // POST: /api/checkup/add
        public async Task<ActionResult<SucceededToAdd>> AddCheckup([FromBody] Checkup checkup)
        {
            var addedCheckup = await _checkupService.AddAsync(checkup);

            if (addedCheckup.Id == 0) 
                return BadRequest(new ApiResponse(500, "an error occured while adding"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = addedCheckup
            });
        }

        [HttpPost("add/result")] // POST: /api/checkup/add/result
        public async Task<ActionResult<SucceededToAdd>> AddCheckupResult([FromForm] PatientCheckupToAddDto model)
        {
            var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(model.CheckupId);

            if (checkup is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var patient = await _patientManager.FindByIdAsync(model.PatientId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var checkupResult = new PatientCheckup
            {
                FK_ChecupId = model.CheckupId,
                FK_PatientId = model.PatientId,
                Result = DocumentSettings.UploadFile(model.Result, "Checkups")
            };

            var result = await _checkupService.AddCheckupResult(checkupResult);

            if (result <= 0)
                return BadRequest(new ApiResponse(400, "an error occured during recording result"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = await MaptoPatientCheckupDto(checkupResult)
            });
        }

        [HttpGet("results")]
        public async Task<ActionResult<List<PatientCheckupToReturnDto>>> GetAllPatientsResults()
        {
            var result = await _checkupService.GetAllResults();

            if (result.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MaptoPatientCheckupDto(result);

            return Ok(mapped);
        }

        [HttpGet("paid/to/do")] // GET: /api/checup/paid
        public async Task<ActionResult<List<PatientCheckupToReturnDto>>> GetCheckupsToDo()
        {
            var checkupsInBills = await _context.Set<CheckupInBill>().ToListAsync();

            var groupedByBillId = checkupsInBills.GroupBy(x => x.FK_BillId);

            if (groupedByBillId.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = new List<CheckupToDoDto>();
            foreach (var group in groupedByBillId)
            {
                var bill = await _unitOfWork.Repository<Bill>().GetByIdAsync(group.Key);

                var patient = await _patientManager.FindByIdAsync(bill.FK_PayorId);

                var checkups = new List<Checkup>();

                foreach (var checkupInBill in group)
                {
                    var done = (await _checkupService.GetAllResults()).Any(x => (x.FK_PatientId == patient.Id
                                                                                   && x.FK_ChecupId == checkupInBill.FK_CheckupId
                                                                                   && x.DateTime > bill.DateTime));
                    if(!done)
                    {
                        var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(checkupInBill.FK_CheckupId);


                        if(checkup != null)
                            checkups.Add(checkup);
                    }
                }

                var itemToAdd = new CheckupToDoDto
                {
                    Patient = patient,
                    Checkups = checkups
                };

                if (itemToAdd.Checkups.Count() != 0)
                    mapped.Add(itemToAdd);
            }

            if (mapped.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(mapped);
        }

        [HttpGet("results/{patientId}")]
        public async Task<ActionResult<List<CheckupResultsToReturnDto>>> GetResultsForSpecificPatient(string patientId)
        {
            var patient = await _patientManager.FindByIdAsync(patientId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var results = (await _checkupService.GetAllResults()).Where(x => x.FK_PatientId == patientId);

            var mapped = new List<CheckupResultsToReturnDto>();

            foreach (var item in results)
            {
                var itemToAdd = new CheckupResultsToReturnDto
                {
                    Checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_ChecupId),
                    DateTime = item.DateTime,
                    ResultUrl = Path.Combine(_configuration["AssetsBaseUrl"], "Uploads\\Checkups", item.Result)
                };

                mapped.Add(itemToAdd);
            }

            if (mapped.Count() == 0)
                return NotFound(new ApiResponse(404, "no data matches found"));

            return Ok(mapped);
        }

        [HttpPost("update")] // POST: /api/checkup/update
        public async Task<ActionResult<SucceededToAdd>> UpdateCheckup([FromBody]Checkup checkup)
        {
            try
            {
                await _checkupService.Update(checkup);

                return Ok(new SucceededToAdd
                {
                    Message = "success",
                    Data = await _checkupService.GetByIdAsync(checkup.Id)
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "an error occured during the update of data"));
            }
        }

        [HttpDelete("delete")] // DELETE: /api/checkup/delete?id=
        public async Task<ActionResult<SucceededToAdd>> DeleteCheckup([FromQuery]int id)
        {
            var result = await _checkupService.DeleteAsync(id);

            if (result > 0)
                return Ok(new ApiResponse(200, "checkup deleted successfully"));
            else
                return BadRequest(new ApiResponse(400, "there is no checkup matches id"));
        }

        // private methods to map
        private async Task<PatientCheckupToReturnDto> MaptoPatientCheckupDto(PatientCheckup data)
            => new PatientCheckupToReturnDto
            {
                Checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(data.FK_ChecupId),
                DateTime = data.DateTime,
                ResultUrl = Path.Combine(_configuration["AssetsBaseUrl"], "Uploads\\Checkups", data.Result),
                Patient = await _patientManager.FindByIdAsync(data.FK_PatientId)
            };
        private async Task<List<PatientCheckupToReturnDto>> MaptoPatientCheckupDto(List<PatientCheckup> data)
        {
            var mapped = new List<PatientCheckupToReturnDto>();

            foreach (var item in data)
                mapped.Add(await MaptoPatientCheckupDto(item));

            return mapped.OrderByDescending(x => x.DateTime).ToList();
        }


        /*
         * end point we may need it 
         *      1. get checkup list by patient id (want to look at database 3shan ana me4 fahem 7aga)
         *      2. get operations on checkup (payment & patient do checkup)
         */
    }
}
