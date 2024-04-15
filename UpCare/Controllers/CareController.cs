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


        [HttpGet("GetAllData")] //api.care.getalldata
        public async Task<IActionResult> GetAllTemperatureAndHumidity()
        {
            try
            {
                var temperatureAndHumidityData = await _fireBaseServices.GetAllTemperatureAndHumidityAsync();
          
                return Ok(temperatureAndHumidityData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
