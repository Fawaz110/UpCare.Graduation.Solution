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
            IPrescriptionService prescriptionService,
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager, 
            IUnitOfWork unitOfWork)
        {
            _prescriptionService = prescriptionService;
            _patientManager = patientManager;
            _doctorManager = doctorManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("all")] // GET: /api/prescription/all?doctorId={string || null}&patientId={string || null}
        public async Task<ActionResult<List<PrescriptionDto>>> GetAllPrscription([FromQuery]string doctorId = null, [FromQuery]string patientId = null)
        {
            var data = await _prescriptionService.GetAllPrescriptionAsync(doctorId, patientId);

            if (data.Count() == 0)
                return NotFound(new ApiResponse(404, "no data found"));

            var mapped = await MapToPrescriptionDto(data);

            return Ok(mapped);
        }

        // methods to map => Dtos
        private async Task<List<PrescriptionDto>> MapToPrescriptionDto(List<Prescription> data)
        {
            var mapped = new List<PrescriptionDto>();

            foreach (var item in data)
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

                var mappedItem = new PrescriptionDto
                {
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

                mapped.Add(mappedItem);
            }

            return mapped;
        }

    }
}
