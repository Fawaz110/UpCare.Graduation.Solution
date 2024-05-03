using Core.UnitOfWork.Contract;
using Core.UpCareEntities.BillEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class BillController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Patient> _patientManager;

        public BillController(
            IUnitOfWork unitOfWork,
            UserManager<Patient> patientManager)
        {
            _unitOfWork = unitOfWork;
            _patientManager = patientManager;
        }

        [HttpGet("all")] // GET: /api/bill/all
        public async Task<ActionResult<List<Bill>>> GetAllBills(string? patientId)
        {
            var result = await _unitOfWork.Repository<Bill>().GetAllAsync();

            if(patientId != null)
            {
                var patient = await _patientManager.FindByIdAsync(patientId);

                if (patient is null)
                    return BadRequest(new ApiResponse(400, "invalid data entered"));

                result = result.Where(x=>x.FK_Payor == patientId).ToList();
            }

            if(result.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(result);
        }

        [HttpPost("add")] // POST: /api/bill/add
        public async Task<ActionResult<SucceededToAdd>> AddBill(Bill model)
        {
            var patient = _patientManager.FindByIdAsync(model.FK_Payor);

            if (patient is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            await _unitOfWork.Repository<Bill>().Add(model);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return BadRequest(new ApiResponse(400, "an error occured during adding data"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = model
            });
        }
    }
}
