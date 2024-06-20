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
        public async Task<ActionResult<List<BillDto>>> GetAllBills(string? patientId)
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

            return Ok(mapped.OrderByDescending(x => x.DateTime));
        }

        [HttpGet("{id}")] // GET: /api/bill/{id}
        public async Task<ActionResult<BillDto>> GetSpecificBill(int id)
        {
            var bill = await _unitOfWork.Repository<Bill>().GetByIdAsync(id);

            if (bill is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var mapped = await MapToBillDto(bill);

            return Ok(mapped);
        }

        //[HttpPost("add")] // POST: /api/bill/add
        //public async Task<ActionResult<SucceededToAdd>> AddPrescriptionBill(BillToAddDto model)
        //{
        //    var patient = _patientManager.FindByIdAsync(model.FK_PayorId);

        //    if (patient is null)
        //        return BadRequest(new ApiResponse(400, "invalid data entered"));

        //    var prescription = await _unitOfWork.Repository<Prescription>().GetByIdAsync(model.PrescriptionId);

        //    if (prescription is null)
        //        return BadRequest(new ApiResponse(400, "error occured with prescription"));

        //    var bill = new Bill
        //    {
        //        DateTime = model.DateTime,
        //        DeliveredService = model.DeliveredService,
        //        FK_PayorId = model.FK_PayorId,
        //        PaymentIntentId = model.PaymentIntentId,
        //        PaidMoney = await CalcPaidMoney(model.PrescriptionId, model.PrescriptionPayment)
        //    };

        //    // Cont.. Form Here
        //    // Get Medicine, Radiologies, Checkups depends on BillToAddDto.Payment
        //    #region Get Related Data To Bill Depends On Payment Sent

        //    var medicineList = new List<MedicineInPrescription>();
        //    var checkupList = new List<CheckupInPrescription>();
        //    var radiologyList = new List<RadiologyInPrescription>();

        //    if (model.PrescriptionPayment == PrescriptionPayment.All || model.PrescriptionPayment == PrescriptionPayment.Medicine)
        //        medicineList = await _prescriptionService.GetMedicineByPrescriptionIdAsync(model.PrescriptionId);

        //    if (model.PrescriptionPayment == PrescriptionPayment.All || model.PrescriptionPayment == PrescriptionPayment.Checkup)
        //        checkupList = await _prescriptionService.GetCheckupByPrescriptionIdAsync(model.PrescriptionId);

        //    if (model.PrescriptionPayment == PrescriptionPayment.All || model.PrescriptionPayment == PrescriptionPayment.Radiology)
        //        radiologyList = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(model.PrescriptionId);

        //    #endregion

        //    var result = await _billService.AddAsync(bill, medicineList, checkupList, radiologyList);

        //    if (result is null)
        //        return BadRequest(new ApiResponse(400, "an error occured during adding data"));

        //    return Ok(new SucceededToAdd
        //    {
        //        Message = "success",
        //        Data = await MapToBillDto(result)
        //    });
        //}

        //[HttpPost("add")] // POST: /api/bill/add
        //public async Task<ActionResult<SucceededToAdd>> AddReservastionBill()
        //{

        //}

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
                var mappedItem = await MapToBillDto(record);

                mapped.Add(mappedItem);
            }
            return mapped;
        }

        private async Task<BillDto> MapToBillDto(Bill data)
            => new BillDto
            {
                DateTime = data.DateTime,
                DeliveredService = data.DeliveredService,
                PaidMoney = data.PaidMoney,
                Payor = await _patientManager.FindByIdAsync(data.FK_PayorId),
                Medicines = await _billService.GetMedicineInBillAsync(data.Id),
                Checkups = await _billService.GetCheckupInBillAsync(data.Id),
                Radiologies = await _billService.GetRadiologiesInBillAsync(data.Id),
                Id = data.Id, 
                ClientSecret = data.ClientSecret,
                PaymentIntentId = data.PaymentIntentId,
            };
        
        private async Task<decimal> CalcPaidMoney(int prescriptionId, PrescriptionPayment? payment)
        {
            var totalPaid = 0m;

            if(payment == PrescriptionPayment.All)
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
            else if (payment == PrescriptionPayment.Medicine)
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
            else if (payment == PrescriptionPayment.Radiology)
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
            else if (payment == PrescriptionPayment.Checkup)
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
