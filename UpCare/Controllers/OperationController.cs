using Core.Entities.UpCareEntities;
using Core.UnitOfWork.Contract;
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
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;

        public OperationController(
            IUnitOfWork unitOfWork,
            UserManager<Admin> adminManager,
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager)
        {
            _unitOfWork = unitOfWork;
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
    }
}
