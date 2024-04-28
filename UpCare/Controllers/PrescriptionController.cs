using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareEntities.PrescriptionEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;
using UpCare.DTOs;
using UpCare.DTOs.PrescriptionDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class PrescriptionController : BaseApiController
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly IUnitOfWork _unitOfWork;

        public PrescriptionController(
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager,
            IPrescriptionService prescriptionService,
            IUnitOfWork unitOfWork)
        {
            _prescriptionService = prescriptionService;
            _patientManager = patientManager;
            _doctorManager = doctorManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("all")] // GET: /api/prescription/all?doctorId={string || null}&patientId={string || null}
        public async Task<ActionResult<List<PrescriptionDto>>> GetAllPrscription([FromQuery] string? doctorId = null, [FromQuery] string? patientId = null)
        {
            var data = await _prescriptionService.GetAllPrescriptionAsync(doctorId, patientId);

            if (data.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MapToPrescriptionDto(data);

            return Ok(mapped);
        }

        [HttpGet("{id}")] // GET: /api/prescription/{id}
        public async Task<ActionResult<PrescriptionDto>> GetById(int id)
        {
            var prescription = await _prescriptionService.GetByIdAsync(id);

            if (prescription is null)
                return NotFound(new ApiResponse(404, "no data found"));


            return Ok(await MapToPrescriptionDto(prescription));
        }

        [HttpPost("add")] // POST: /api/prescription/add
        public async Task<ActionResult<SucceededToAdd>> AddPrescrition([FromBody] PrescritionToAddDto model)
        {
            var patient = await _patientManager.FindByIdAsync(model.FK_PatientId);

            if (patient is null) 
                return NotFound(new ApiResponse(404, "no data matches found"));

            var doctor = await _doctorManager.FindByIdAsync(model.FK_DoctorId);

            if (doctor is null)
                return Unauthorized(new ApiResponse(401, "unauthorized access"));

            var prescritionToAdd = new Prescription
            {
                Advice = model.Advice,
                DateTime = DateTime.UtcNow,
                Details = model.Details,
                Diagnosis = model.Diagnosis,
                FK_DoctorId = model.FK_DoctorId,
                FK_PatientId = model.FK_PatientId,
            }; // 1

            var medicineList = new List<Medicine>();

            if (model.FK_MedicineIds.Count() > 0)
            {
                foreach (var medicineId in model.FK_MedicineIds)
                {
                    var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(medicineId);

                    if (medicine is null)
                        return BadRequest(new ApiResponse(400, "invalid data entered"));

                    medicineList.Add(medicine);
                }
            } // 2

            var checkupList = new List<Checkup>();

            if (model.FK_CheckupsIds.Count() > 0)
            {
                foreach (var checkupId in model.FK_CheckupsIds)
                {
                    var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(checkupId);

                    if (checkup is null)
                        return BadRequest(new ApiResponse(400, "invalid data entered"));

                    checkupList.Add(checkup);
                }
            } // 3

            var radiologyList = new List<Radiology>();

            if (model.FK_RadiologiesIds.Count() > 0)
            {
                foreach (var radiologyId in model.FK_RadiologiesIds)
                {
                    var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(radiologyId);

                    if (radiology is null)
                        return BadRequest(new ApiResponse(400, "invalid data entered"));

                    radiologyList.Add(radiology);
                }
            } // 4

            var prescription = await _prescriptionService.AddPrescriptionAsync(prescritionToAdd, medicineList, checkupList, radiologyList);

            if (prescription is null) 
                return BadRequest(new ApiResponse(400, "error occured during adding data"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = await MapToPrescriptionDto(prescription)
            });
        }


        // methods to map => Dtos
        private async Task<List<PrescriptionDto>> MapToPrescriptionDto(List<Prescription> data)
        {
            var mapped = new List<PrescriptionDto>();

            foreach (var item in data)
            {
                var mappedItem = await MapToPrescriptionDto(item);

                mapped.Add(mappedItem);
            }

            return mapped;
        }

        private async Task<PrescriptionDto> MapToPrescriptionDto(Prescription item)
        {
            var medicineRecords = await _prescriptionService.GetMedicineByPrescriptionIdAsync(item.Id);

            var mappedMedicine = new List<Medicine>(); // 1

            foreach (var med in medicineRecords)
            {
                var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(med.FK_MedicineId);

                mappedMedicine.Add(medicine);
            }

            var checkupRecords = await _prescriptionService.GetCheckupByPrescriptionIdAsync(item.Id);

            var mappedCheckup = new List<Checkup>(); // 2

            foreach (var ch in checkupRecords)
            {
                var checkup = await _unitOfWork.Repository<Checkup>().GetByIdAsync(ch.FK_CheckupId);

                mappedCheckup.Add(checkup);
            }

            var radiologyRecords = await _prescriptionService.GetRadiologyByPrescriptionIdAsync(item.Id);

            var mappedRadiology = new List<Radiology>(); // 3

            foreach (var rad in radiologyRecords)
            {
                var radiology = await _unitOfWork.Repository<Radiology>().GetByIdAsync(rad.FK_RadiologyId);

                mappedRadiology.Add(radiology);
            }

            return new PrescriptionDto
            {
                Id = item.Id,
                Advice = item.Advice,
                DateTime = item.DateTime,
                Details = item.Details,
                Diagnosis = item.Diagnosis,
                Checkups = mappedCheckup,
                Medicines = mappedMedicine,
                Radiologies = mappedRadiology,
                Doctor = await _doctorManager.FindByIdAsync(item.FK_DoctorId),
                Patient = await _patientManager.FindByIdAsync(item.FK_PatientId)
            };

        }

    }
}
