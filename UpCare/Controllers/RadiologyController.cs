using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UpCareEntities;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.Errors;
using UpCare.Helpers;
using Core.UnitOfWork.Contract;
using Microsoft.AspNetCore.Identity;
using Core.UpCareUsers;
using Repository.UpCareData;
using Core.UpCareEntities.BillEntities;
using Microsoft.EntityFrameworkCore;

namespace UpCare.Controllers
{
    public class RadiologyController : BaseApiController
    {
        private readonly IRadiologyService _radiologyService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Patient> _patientManager;
        private readonly IConfiguration _configuration;
        private readonly UpCareDbContext _context;

        public RadiologyController(
            IRadiologyService radiologyService,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            UserManager<Patient> patientManager,
            UpCareDbContext context)
        {
            _radiologyService = radiologyService;
            _unitOfWork = unitOfWork;
            _patientManager = patientManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("all")] // GET: /api/radiology/all
        public async Task<ActionResult<List<Radiology>>> GetAll()
        {
            var radiologyList = await _radiologyService.GetAllAsync();

            if (radiologyList.Count() == 0) 
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(radiologyList);
        }

        [HttpGet("{id}")] // GET: /api/radiology/1
        public async Task<ActionResult<Radiology>> GetById(int id)
        {
            var radiology = await _radiologyService.GetByIdAsync(id);

            if (radiology is null) 
                return NotFound(new ApiResponse(404, "no radiology matches given id found"));

            return Ok(radiology);
        }

        [HttpPost("add")] // POST: /api/radiology/add
        public async Task<ActionResult<SucceededToAdd>> AddRadiology([FromBody]Radiology model)
        {
            var radiology = await _radiologyService.AddAsync(model);

            if (radiology is null)
                return BadRequest(new ApiResponse(400, "error occured during adding the radiology"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = radiology
            });
        }

        [HttpPost("add/result")] // POST: /api/radiology/add/result
        public async Task<ActionResult<SucceededToAdd>> AddCheckupResult([FromForm] PatientRadiologyToAddDto model)
        {
            var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(model.RadiologyId);

            if (radiology is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var patient = await _patientManager.FindByIdAsync(model.PatientId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var radiologyResult = new PatientRadiology
            {
                FK_RadiologyId = model.RadiologyId,
                FK_PatientId = model.PatientId,
                Result = DocumentSettings.UploadFile(model.Result, "Radiologies")
            };

            var result = await _radiologyService.AddRadiologyResult(radiologyResult);

            if (result <= 0)
                return BadRequest(new ApiResponse(400, "an error occured during recording result"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = await MapToPatientRadiologyToReturn(radiologyResult)
            });
        }

        [HttpGet("results")]
        public async Task<ActionResult<List<PatientRadiologyToReturnDto>>> GetAllPatientsResults()
        {
            var result = await _radiologyService.GetAllResults();

            if (result.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MapToPatientRadiologyToReturn(result);

            return Ok(mapped);
        }

        [HttpGet("paid/to/do")] // GET: /api/checup/paid
        public async Task<ActionResult<List<PatientCheckupToReturnDto>>> GetRadiologiesToDo()
        {
            var radiologiesInBill = await _context.Set<RadiologyInBill>().ToListAsync();

            var groupedByBillId = radiologiesInBill.GroupBy(x => x.FK_BillId);

            if (groupedByBillId.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = new List<RadiologyToDoDto>();
            foreach (var group in groupedByBillId)
            {
                var bill = await _unitOfWork.Repository<Bill>().GetByIdAsync(group.Key);

                var patient = await _patientManager.FindByIdAsync(bill.FK_PayorId);

                var radiologies = new List<Radiology>();

                foreach (var checkupInBill in group)
                {
                    var done = (await _radiologyService.GetAllResults()).Any(x => (x.FK_PatientId == patient.Id
                                                                                   && x.FK_RadiologyId == checkupInBill.FK_RadiologyId
                                                                                   && x.DateTime > bill.DateTime));
                    if (!done)
                    {
                        var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(checkupInBill.FK_RadiologyId);

                        radiologies.Add(radiology);
                    }
                }

                var itemToAdd = new RadiologyToDoDto
                {
                    Patient = patient,
                    Radiologies = radiologies
                };

                if (itemToAdd.Radiologies.Count() != 0)
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

            var results = (await _radiologyService.GetAllResults()).Where(x => x.FK_PatientId == patientId);

            var mapped = new List<PatientRadiologyToReturnDto>();

            foreach (var item in results)
            {
                var itemToAdd = new PatientRadiologyToReturnDto
                {
                    Radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId),
                    DateTime = item.DateTime,
                    ResultUrl = Path.Combine(_configuration["AssetsBaseUrl"], "Uploads\\Checkups", item.Result),
                    Patient = await _patientManager.FindByIdAsync(item.FK_PatientId)
                };

                mapped.Add(itemToAdd);
            }

            if (mapped.Count() == 0)
                return NotFound(new ApiResponse(404, "no data matches found"));

            return Ok(mapped);
        }

        [HttpPost("update")] // POST: /api/radiology/update
        public async Task<ActionResult<SucceededToAdd>> Update([FromBody] Radiology model)
        {
            try
            {
                await _radiologyService.Update(model);

                return Ok(new SucceededToAdd
                {
                    Message = "success",
                    Data = await _radiologyService.GetByIdAsync(model.Id)
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "an error occured during the update of data"));
            }
        }

        [HttpDelete("delete")] // DELETE: /api/radiology/delete?id=
        public async Task<ActionResult<ApiResponse>> Delete([FromQuery] int id)
        {
            var result = await _radiologyService.DeleteAsync(id);

            if (result > 0)
                return Ok(new ApiResponse(200, "radiology deleted successfully"));
            else
                return BadRequest(new ApiResponse(400, "there is no radiology matches id"));
        }

        // private methods for mapping
        private async Task<PatientRadiologyToReturnDto> MapToPatientRadiologyToReturn(PatientRadiology data)
            => new PatientRadiologyToReturnDto
            {
                ResultUrl = Path.Combine(_configuration["AssetsBaseUrl"], "Uploads\\Radiologies", data.Result),
                DateTime = data.DateTime,
                Patient = await _patientManager.FindByIdAsync(data.FK_PatientId),
                Radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(data.FK_RadiologyId)
            };

        private async Task<List<PatientRadiologyToReturnDto>> MapToPatientRadiologyToReturn(List<PatientRadiology> data)
        {
            var mapped = new List<PatientRadiologyToReturnDto>();

            foreach (var item in data)
                mapped.Add(await MapToPatientRadiologyToReturn(item));
            
            return mapped;
        }
        /*
         * end point we may need it 
         *      1. get radiology list by patient id (want to look at database 3shan ana me4 fahem 7aga)
         *      2. get operations on radiology (payment & patient do radiology)
         */
    }
}
