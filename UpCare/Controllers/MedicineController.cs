using AutoMapper;
using Core.Repositories.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class MedicineController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IGenericRepository<Medicine> _genericRepository;

        public MedicineController(/* IGenericRepository<Medicine> genericRepository */
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_genericRepository = genericRepository;
        }
        [HttpGet("all")] // GET: /api/medcine/all
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetAll()
        {
            
            var medicineList = await _unitOfWork.Repository<Medicine>().GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<MedicineDto>>(medicineList);
            return Ok(medicineList);
        }

        //[HttpPost("add")] // POST: /api/medicine/add
        //public async Task<ActionResult<SucceededToAdd>> AddMedicine(MedicineDto model)
        //{
            
        //}

    }
}
