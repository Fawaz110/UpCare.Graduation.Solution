using Core.UnitOfWork.Contract;
using Core.UpCareEntities.BillEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("all")] // GET: /api/bill/all?patientId={string}
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

            var mapped = await MapToBillDto(result.ToList());

            return Ok(mapped);
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
                Data = await MapToBillDto(model)
            });
        }

        [HttpGet] // GET: /api/bill?searchTerm=
        public async Task<ActionResult<List<BillDto>>> Search([FromQuery]string? searchTerm)
        {
            var bills = await _unitOfWork.Repository<Bill>().GetAllAsync();

            var selectedBills = new List<BillDto>();

            if(searchTerm != null)
            {
                var payorsIds = (await _patientManager.Users.Where(p => p.FirstName.Trim().ToLower().Contains(searchTerm.Trim().ToLower())
                                                                     || p.LastName.Trim().ToLower().Contains(searchTerm.Trim().ToLower())).ToListAsync()).Select(x=> x.Id).ToList();

                foreach (var bill in bills)
                    if (payorsIds.Contains(bill.FK_Payor))
                        selectedBills.Add(await MapToBillDto(bill));
            }
            else
            {
                selectedBills = await MapToBillDto(bills.ToList());
            }

            // elmafrood 7aga tehsal hena mesh 3aref heya eah

            if (selectedBills.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(selectedBills);
        }

        // private methods to map => BillDto
        private async Task<List<BillDto>> MapToBillDto(List<Bill> data)
        {
            var mapped = new List<BillDto>();

            foreach (var record in data)
            {
                var mappedItem = new BillDto
                {
                    DateTime = record.DateTime,
                    DeliveredService = record.DeliveredService,
                    PaidMoney = record.PaidMoney,
                    Payor = await _patientManager.FindByIdAsync(record.FK_Payor)
                };
                mapped.Add(mappedItem);
            }

            return mapped;
        }

        private async Task<BillDto> MapToBillDto(Bill data)
        {
            return new BillDto
            {
                DateTime = data.DateTime,
                DeliveredService = data.DeliveredService,
                PaidMoney = data.PaidMoney,
                Payor = await _patientManager.FindByIdAsync(data.FK_Payor)
            };
        }
    }
}
