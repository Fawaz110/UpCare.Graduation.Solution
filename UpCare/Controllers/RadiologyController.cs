using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using UpCare.DTOs;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class RadiologyController : BaseApiController
    {
        private readonly IRadiologyService _radiologyService;

        public RadiologyController(
            IRadiologyService radiologyService)
        {
            _radiologyService = radiologyService;
        }

        [HttpGet("all")] // GET: /api/radiology/all
        public async Task<ActionResult<List<Radiology>>> GetAll()
        {
            var radiologyList = await _radiologyService.GetAllAsync();

            if (radiologyList.Count() == 0) 
                return NotFound(new ApiResponse(404, "no data found"));

            return Ok(radiologyList);
        }

        [HttpGet("{id}")] // GET: /api/radiology/1
        public async Task<ActionResult<Radiology>> GetById(int id)
        {
            var radiology = await _radiologyService.GetByIdAsync(id);

            if (radiology is null) 
                return NotFound(new ApiResponse(404, "no radiology matches given id found"));

            return Ok(radiology);
        }

        [HttpPost("add")] // POST: /api/radiology/add
        public async Task<ActionResult<SucceededToAdd>> AddRadiology([FromBody]Radiology model)
        {
            var radiology = await _radiologyService.AddAsync(model);

            if (radiology is null)
                return BadRequest(new ApiResponse(400, "error occured during adding the radiology"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = radiology
            });
        }

        [HttpPost("update")] // POST: /api/radiology/update
        public async Task<ActionResult<SucceededToAdd>> Update([FromBody] Radiology model)
        {
            try
            {
                _radiologyService.Update(model);

                return Ok(new SucceededToAdd
                {
                    Message = "success",
                    Data = await _radiologyService.GetByIdAsync(model.Id)
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "an error occured during the update of data"));
            }
        }

        [HttpDelete("delete")] // DELETE: /api/radiology/delete?id=
        public async Task<ActionResult<ApiResponse>> Delete([FromQuery] int id)
        {
            var result = await _radiologyService.DeleteAsync(id);

            if (result > 0)
                return Ok(new ApiResponse(200, "radiology deleted successfully"));
            else
                return BadRequest(new ApiResponse(400, "there is no radiology matches id"));
        }

        /*
         * end point we may need it 
         *      1. get radiology list by patient id (want to look at database 3shan ana me4 fahem 7aga)
         *      2. get operations on radiology (payment & patient do radiology)
         */
    }
}
