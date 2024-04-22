using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Service;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class CareController : BaseApiController
    {
        private readonly FireBaseServices _fireBaseServices;

        public CareController(FireBaseServices fireBaseServices)
        {
            _fireBaseServices = fireBaseServices;
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

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestData()
        {
            try
            {
                // Call the DataService to get the latest data
                var (latestTemperature, latestHumidity, latestTime, latestDate) = await _fireBaseServices.GetLatestData();

                // Construct a JSON response
                var responseData = new
                {
                    Temperature = latestTemperature,
                    Humidity = latestHumidity,
                    Time = latestTime,
                    Date = latestDate
                };

                // Return the JSON response
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error response
                Console.WriteLine($"Error retrieving latest data: {ex.Message}");
                return StatusCode(500, "Internal server error");
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



        [HttpGet("monitor")]
        public async Task<ActionResult<string>> StartTemperatureMonitoring()
        {
            try
            {
                // Start monitoring the temperature
                await _fireBaseServices.MonitorTemperature();

                return Ok(new ApiResponse(200, "email sent successfully"));
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 Internal Server Error response
                Console.WriteLine($"Error starting temperature monitoring: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
