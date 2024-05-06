using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpCare.DTOs;
using UpCare.DTOs.BillDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class BillController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IBillService _billService;
        private readonly UserManager<Patient> _patientManager;

        public BillController(
            IUnitOfWork unitOfWork,
            IPrescriptionService prescriptionService,
            IBillService billService,
            UserManager<Patient> patientManager)
        {
            _unitOfWork = unitOfWork;
            _prescriptionService = prescriptionService;
            _billService = billService;
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

                result = result.Where(x=>x.FK_PayorId == patientId).ToList();
            }

            if(result.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MapToBillDto(result.ToList());

            return Ok(mapped);
        }

        [HttpPost("add")] // POST: /api/bill/add
        public async Task<ActionResult<SucceededToAdd>> AddBill(BillToAddDto model)
        {
            var patient = _patientManager.FindByIdAsync(model.FK_PayorId);

            if (patient is null)
                return BadRequest(new ApiResponse(400, "invalid data entered"));

            var prescription = await _unitOfWork.Repository<Prescription>().GetByIdAsync(model.PrescriptionId);

            if (prescription is null)
                return BadRequest(new ApiResponse(400, "error occured with prescription"));

            var bill = new Bill
            {
                DateTime = model.DateTime,
                DeliveredService = model.DeliveredService,
                FK_PayorId = model.FK_PayorId,
                PaymentIntentId = model.PaymentIntentId,
                PaidMoney = await CalcPaidMoney(model.PrescriptionId, model.Payment)
            };

            // Cont.. Form Here

            var result = await _billService.AddAsync(bill);

            if (result is null)
                return BadRequest(new ApiResponse(400, "an error occured during adding data"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = await MapToBillDto(result)
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
                    if (payorsIds.Contains(bill.FK_PayorId))
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
                    Payor = await _patientManager.FindByIdAsync(record.FK_PayorId)
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
                Payor = await _patientManager.FindByIdAsync(data.FK_PayorId)
            };
        }

        private async Task<decimal> CalcPaidMoney(int prescriptionId, Payment payment)
        {
            var totalPaid = 0m;

            if(payment == Payment.All)
            {
                #region Calc Medicine Total Price
                var medicineList = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                foreach (var item in medicineList)
                {
                    var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                    totalPaid += medicine.Price;
                } 
                #endregion

                #region Calc Checkup Total Price
                var checkupList = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                foreach (var item in checkupList)
                {
                    var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_CheckupId);

                    totalPaid += checkup.Price;
                }
                #endregion

                #region Calc Radiology Total Price
                var radiologyList = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                foreach (var item in radiologyList)
                {
                    var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId);

                    totalPaid += radiology.Price;
                } 
                #endregion
            }
            else if (payment == Payment.Medicine)
            {
                #region Calc Medicine Total Price
                var medicineList = await _prescriptionService.GetMedicineByPrescriptionIdAsync(prescriptionId);

                foreach (var item in medicineList)
                {
                    var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.FK_MedicineId);

                    totalPaid += medicine.Price;
                }
                #endregion
            }
            else if (payment == Payment.Radiology)
            {
                #region Calc Radiology Total Price
                var radiologyList = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(prescriptionId);

                foreach (var item in radiologyList)
                {
                    var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(item.FK_RadiologyId);

                    totalPaid += radiology.Price;
                }
                #endregion
            }
            else if (payment == Payment.Checkup)
            {
                #region Calc Checkup Total Price
                var checkupList = await _prescriptionService.GetCheckupByPrescriptionIdAsync(prescriptionId);

                foreach (var item in checkupList)
                {
                    var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(item.FK_CheckupId);

                    totalPaid += checkup.Price;
                }
                #endregion
            }

            return totalPaid;
        }
    }
}
