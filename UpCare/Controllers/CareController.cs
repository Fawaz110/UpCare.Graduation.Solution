using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace UpCare.Controllers
{
    public class CareController : BaseApiController
    {
        private readonly FireBaseServices _fireBaseServices;

        public CareController(FireBaseServices fireBaseServices )
        {
            _fireBaseServices = fireBaseServices;
        }

        [HttpGet("Data")] //api.care.data
        public async Task<IActionResult> GetData()
        {
            try
            {
                var (temperature, humidity) = await _fireBaseServices.GetTemperatureAndHumidityAsync();
                var responseData = new { Temperature = temperature, Humidity = humidity };
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        
        [HttpGet("GetCurrentTemp")]
        public async Task<IActionResult> GetCurrentTemperature()
        {
            try
            {
                var temperatureData = await _fireBaseServices.GetCurrentTemperatureAsync();
                return Ok(temperatureData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        [HttpGet("getCurrentData")]
        public async Task<IActionResult> GetCurrentData()
        {
            try
            {
                var currentDataList = await _fireBaseServices.GetCurrentDataAsync();
                return Ok(currentDataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
