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
        public async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
        {

            var medicineList = await _medicineService.GetAllAsync();

            return Ok(medicineList);
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

        [HttpGet("search")] // GET: /api/medicine/search?searchTerm=
        public async Task<ActionResult<IEnumerable<Medicine>>> Search([FromQuery]string searchTerm)
        {
            var medicineList = await _medicineService.SearchByMedicineName(searchTerm);

            if (medicineList.Count() == 0) 
                return NotFound(new ApiResponse(404, "no medicine matches founded"));

            return Ok(medicineList);
        }

        [HttpGet("categories")] // GET: /api/medicine/categories
        public async Task<ActionResult<List<string>>> GetCategories()
        {
            var categories = await _medicineService.GetCategories();

            if (categories.Count() == 0)
                return NotFound(new ApiResponse(404, "no data exists"));

            return Ok(categories);
        }

        [HttpGet("shortage")] // GET: /api/medicine/shortage
        public async Task<ActionResult<List<Medicine>>> GetShortage(int leastNormalQuantity = 10)
        {
            var medicineList = await _medicineService.GetShortage(leastNormalQuantity);

            if (medicineList.Count == 0) 
                return NotFound(new ApiResponse(404, "there is no shortage"));

            return Ok(medicineList);
        }

        [HttpPost("update")] // POST: /api/medicine/update
        public async Task<ActionResult<SucceededToAdd>> Update([FromBody]Medicine medicine)
        {
            _medicineService.UpdateMedicine(medicine);

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = await _medicineService.GetByIdAsync(medicine.Id)
            });
        }

        [HttpDelete("delete")] // DELETE: /api/medicine/delete?id=
        public async Task<ActionResult<SucceededToAdd>> DeleteMedicine([FromQuery]int id)
        {
            var result = await _medicineService.DeleteAsync(id);

            if (result > 0)
                return Ok(new ApiResponse(200, "medicine deleted successfully"));
            else
                return BadRequest(new ApiResponse(400, "there is no medicine matches id"));
        }

    }
}
