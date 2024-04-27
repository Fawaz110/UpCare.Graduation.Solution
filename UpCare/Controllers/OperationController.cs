using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class OperationController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationService _operationService;
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;

        public OperationController(
            IUnitOfWork unitOfWork,
            IOperationService operationService,
            UserManager<Admin> adminManager,
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager)
        {
            _unitOfWork = unitOfWork;
            _operationService = operationService;
            _adminManager = adminManager;
            _doctorManager = doctorManager;
            _patientManager = patientManager;
        }

        [HttpGet("all")] // GET: /api/operation/all?adminId=edb9bd85-85b7-47fb-b835-600264b8b676
        public async Task<ActionResult<Operation>> GetAll([FromQuery]string adminId)
        {
            var admin = await _adminManager.FindByIdAsync(adminId);

            if(admin is null) 
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            var operations = await _unitOfWork.Repository<Operation>().GetAllAsync();

            if (operations.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var listToReturn = new List<OperationDto>();

            foreach (var operation in operations)
            {
                var item = new OperationDto
                {
                    Id = operation.Id,
                    Name = operation.Name,
                    Price = operation.Price,
                    Admin = await _adminManager.FindByIdAsync(operation.FK_AdminId)
                };

                listToReturn.Add(item);
            }

            return Ok(listToReturn);
        }

        [HttpPost("add")] // POST: /api/operation/add
        public async Task<ActionResult<SucceededToAdd>> AddOperation([FromBody]Operation operation)
        {
            var admin = await _adminManager.FindByIdAsync(operation.FK_AdminId);

            if (admin is null) 
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            await _unitOfWork.Repository<Operation>().Add(operation);

            await _unitOfWork.CompleteAsync();

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new OperationDto
                {
                    Id = operation.Id,
                    Name = operation.Name,
                    Price = operation.Price,
                    Admin = await _adminManager.FindByIdAsync(operation.FK_AdminId)
                }
            });
        }
        
        [HttpPost("update")] // POST: /api/operation/update
        public async Task<ActionResult<SucceededToAdd>> UpdateOperation([FromBody]Operation model)
        {
            var admin = await _adminManager.FindByIdAsync(model.FK_AdminId);

            if (admin is null) 
                return Unauthorized(new ApiResponse(401, "unauthrized access"));

            var op = await _unitOfWork.Repository<Operation>().GetByIdAsync(model.Id);

            if (op is null) 
                return NotFound(new ApiResponse(404, "no data matches found"));

            op.Price = model.Price;
            op.Name = model.Name;
            op.FK_AdminId = model.FK_AdminId;


            _unitOfWork.Repository<Operation>().Update(op);
            await _unitOfWork.CompleteAsync();

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new OperationDto { 
                    Id = op.Id, 
                    Name = op.Name, 
                    Price = op.Price, 
                    Admin = await _adminManager.FindByIdAsync(model.FK_AdminId)
                }
            });
        }

        [HttpDelete("delete/{adminId}")] // DELETE: /api/operation/delete/{AdminId}?operationId={number}
        public async Task<ActionResult<ApiResponse>> DeleteOperation(string adminId, [FromQuery]int operationId)
        {
            var admin = await _adminManager.FindByIdAsync(adminId);

            if (admin is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            var op = await _unitOfWork.Repository<Operation>().GetByIdAsync(operationId);

            if (op is null) 
                return NotFound(new ApiResponse(404, "no data matches found"));

            _unitOfWork.Repository<Operation>().Delete(op);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) 
                return BadRequest(new ApiResponse(400, "an error occured during process"));

            return Ok(new ApiResponse(200, "operation deleted successfully"));
        }

        [HttpGet("all/schedule")] // GET: /api/operation/all/schedule
        public async Task<ActionResult<List<DoctorDoOperationDto>>> GetAllScheduledOperations()
        {
            var data = await _operationService.GetScheduledOperationsAsync();

            if (data.Count() == 0) 
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MapToDoctorDoOperation(data);

            return Ok(mapped);
        }

        [HttpPost("schedule/add")] // POST: /api/operation/schedule/add
        public async Task<ActionResult<SucceededToAdd>> AddToSchedule(DoctorDoOperation model)
        {

            var doctor = await _doctorManager.FindByIdAsync(model.FK_DoctorId);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "invalid users"));

            var admin = await _adminManager.FindByIdAsync(model.FK_AdminId);

            if (admin is null)
                return Unauthorized(new ApiResponse(401, "invalid users"));

            var patient = await _patientManager.FindByIdAsync(model.FK_PatientId);

            if (patient is null)
                return Unauthorized(new ApiResponse(401, "invalid users"));

            var op = await _unitOfWork.Repository<Operation>().GetByIdAsync(model.FK_OperationId);

            if (op is null)
                return NotFound(new ApiResponse(404, "incorrect data entered"));

            var result = await _operationService.AddToScheduleAsync(model);

            if (result is null)
                return BadRequest(new ApiResponse(400, "doctor may be not available"));

            var mapped = new DoctorDoOperationDto
            {
                Admin = admin,
                Doctor = doctor,
                Patient = patient,
                Operation = op,
                Date = result.Date
            };

            return Ok(mapped);
        }

        [HttpGet("doctor-auth/{id}")] // GET: /api/operation/doctor-auth/{doctorId}?desc=1
        public async Task<ActionResult<OperationsForDoctorDto>> GetScheduleForDoctor(string id, [FromQuery]int desc = 0)
        {
            var doctor = await _doctorManager.FindByIdAsync(id);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            var data = await _operationService.GetScheduledOperationsAsync();

            var afterFilter = data.Where(x => x.FK_DoctorId == id).ToList();

            if (afterFilter.Count() == 0) 
                return NotFound(new ApiResponse(404, "no data found"));            

            return Ok(new OperationsForDoctorDto
            {
                Doctor = doctor,
                Data = (desc == 1) ? (await MapToOperationsWithPatientDto(afterFilter)).OrderByDescending(x => x.DateTime).ToList()
                                    : (await MapToOperationsWithPatientDto(afterFilter)).OrderBy(x => x.DateTime).ToList()
            });
        }

        [HttpGet("patient-auth/{id}")] // GET: /api/operation/patient-auth/{patientId}?desc=1
        public async Task<ActionResult<OperationsForDoctorDto>> GetScheduleForPatient(string id, [FromQuery] int desc = 0)
        {
            var patient = await _patientManager.FindByIdAsync(id);

            if (patient is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            var data = await _operationService.GetScheduledOperationsAsync();

            var afterFilter = data.Where(x => x.FK_PatientId == id).ToList();

            if (afterFilter.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(new
            {
                Patient = patient,
                Data = (desc == 1) ? (await MapToOperationsWithPatientDto(afterFilter)).OrderByDescending(x => x.DateTime).ToList()
                                    : (await MapToOperationsWithPatientDto(afterFilter)).OrderBy(x => x.DateTime).ToList()

            });
        }

        // private method to map data => DTO
        private async Task<List<DoctorDoOperationDto>> MapToDoctorDoOperation(List<DoctorDoOperation> data)
        {
            var mapped = new List<DoctorDoOperationDto>();

            foreach (var record in data)
            {
                var admin = await _adminManager.FindByIdAsync(record.FK_AdminId);
                var doctor = await _doctorManager.FindByIdAsync(record.FK_DoctorId);
                var patient = await _patientManager.FindByIdAsync(record.FK_PatientId);
                var op = await _unitOfWork.Repository<Operation>().GetByIdAsync(record.FK_OperationId);

                var item = new DoctorDoOperationDto
                {
                    Admin = admin,
                    Patient = patient,
                    Doctor = doctor,
                    Operation = op,
                    Date = record.Date
                };

                mapped.Add(item);
            }

            return mapped;
        }
        private async Task<List<OperationWithPatientDto>> MapToOperationsWithPatientDto(List<DoctorDoOperation> data)
        {
            var mapped = new List<OperationWithPatientDto>();

            foreach (var record in data)
            {
                var item = new OperationWithPatientDto
                {
                    Operation = await _unitOfWork.Repository<Operation>().GetByIdAsync(record.FK_OperationId),
                    Patient = await _patientManager.FindByIdAsync(record.FK_PatientId),
                    DateTime = record.Date,
                };

                mapped.Add(item);
            }

            return mapped;
        }
    }
}
