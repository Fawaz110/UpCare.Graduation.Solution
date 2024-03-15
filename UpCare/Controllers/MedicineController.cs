using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UpCare.Controllers
{
    public class MedicineController : BaseApiController
    {
        private readonly IGenericRepository<Medicine> _genericRepository;

        public MedicineController(IGenericRepository<Medicine> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
        {
            var medicineList = await _genericRepository.GetAllAsync();
            return Ok(medicineList);
        }
    }
}
