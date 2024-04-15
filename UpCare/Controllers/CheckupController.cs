using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UpCareEntities;
using Microsoft.AspNetCore.Mvc;
using Service;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class CheckupController : BaseApiController
    {
        private readonly ICheckupService _checkupService;

        public CheckupController(ICheckupService checkupService)
        {
            _checkupService = checkupService;
        }

        [HttpGet("all")] // GET: /api/checkup/all
        public async Task<ActionResult<List<Checkup>>> GetAll()
        {
            var checkupList = await _checkupService.GetAllAsync();

            if(checkupList.Count == 0)
                return NotFound(new ApiResponse(404, "no checkup items founded"));
            
            return Ok(checkupList);
        }

        [HttpGet("{id}")] // GET: /api/checkup/{id}
        public async Task<ActionResult<Checkup>> GetById(int id)
        {
            var checkup = await _checkupService.GetByIdAsync(id);

            if (checkup == null)
                return NotFound(new ApiResponse(404, "no checkup matches this id found"));

            return Ok(checkup);
        }

        [HttpPost("add")] // POST: /api/checkup/add
        public async Task<ActionResult<SucceededToAdd>> AddCheckup([FromBody] Checkup checkup)
        {
            var addedCheckup = await _checkupService.AddAsync(checkup);

            if (addedCheckup.Id == 0) 
                return BadRequest(new ApiResponse(500, "an error occured while adding"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = addedCheckup
            });
        }

        [HttpPost("update")] // POST: /api/checkup/update
        public async Task<ActionResult<SucceededToAdd>> UpdateCheckup([FromBody]Checkup checkup)
        {
            try
            {
                _checkupService.Update(checkup);

                return Ok(new SucceededToAdd
                {
                    Message = "success",
                    Data = await _checkupService.GetByIdAsync(checkup.Id)
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "an error occured during the update of data"));
            }
        }

        [HttpDelete("delete")] // DELETE: /api/checkup/delete?id=
        public async Task<ActionResult<SucceededToAdd>> DeleteCheckup([FromQuery]int id)
        {
            var result = await _checkupService.DeleteAsync(id);

            if (result > 0)
                return Ok(new ApiResponse(200, "checkup deleted successfully"));
            else
                return BadRequest(new ApiResponse(400, "there is no checkup matches id"));
        }
    }
}
