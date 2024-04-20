using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Service;

namespace UpCare.Controllers
{
    public class CareController : BaseApiController
    {
        private readonly FireBaseServices _fireBaseServices;
        public CareController(
            FireBaseServices fireBaseServices)
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
                var (latestTemperature, latestHumidity, latestTime, latestDate, patient) = await _fireBaseServices.GetLatestData();

                // Construct a JSON response
                var responseData = new
                {
                    Temperature = latestTemperature,
                    Humidity = latestHumidity,
                    Time = latestTime,
                    Date = latestDate,
                    Patient = patient
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



       


    }
}
