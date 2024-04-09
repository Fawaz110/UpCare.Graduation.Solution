using AutoMapper;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class MedicineController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IMedicineService _medicineService;

        //private readonly IGenericRepository<Medicine> _genericRepository;

        public MedicineController(/* IGenericRepository<Medicine> genericRepository */
            IMapper mapper, 
            IMedicineService medicineService)
        {
            _mapper = mapper;
            _medicineService = medicineService;
            //_genericRepository = genericRepository;
        }
        [HttpGet("all")] // GET: /api/medcine/all
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetAll()
        {

            var medicineList = await _medicineService.GetAllAsync();

            var mapped = _mapper.Map<IEnumerable<MedicineDto>>(medicineList);

            return Ok(mapped);
        }

        [HttpGet("{id}")] // GET: /api/medicine/{id}
        public async Task<ActionResult<MedicineDto>> GetMedicineById(int id)
        {
            var medicine = await _medicineService.GetByIdAsync(id);

            if (medicine is null)
                return NotFound(new ApiResponse(404, "no medicine matches this id found"));
            
            var mapped = _mapper.Map<MedicineDto>(medicine);

            return Ok(mapped);
        }
        
        [HttpPost("add")] // POST: /api/medicine/add
        public async Task<ActionResult<SucceededToAdd>> AddMedicine(MedicineDto model)
        {
            var mapped = _mapper.Map<Medicine>(model);

            var result = await _medicineService.AddMedicine(mapped);

            if (result < 1)
                return BadRequest(new ApiResponse(500, "an error occured while adding"));


            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = await _medicineService.GetMedicineByName(model.Name)
            });

        }

    }
}
