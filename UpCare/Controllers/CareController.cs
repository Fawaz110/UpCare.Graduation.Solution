using Core.UpCareUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Service;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class CareController : BaseApiController
    {
        private readonly FireBaseServices _fireBaseServices;
        private object _firebaseServices;

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

        //[HttpGet("latest")]
        //public async Task<IActionResult> GetLatestData()
        //{
        //    try
        //    {
        //        // Call the DataService to get the latest data
        //        var (latestTemperature, latestHumidity, latestTime, latestDate, patient) = await _fireBaseServices.GetLatestData();

        //        // Construct a JSON response
        //        var responseData = new
        //        {
        //            Temperature = latestTemperature,
        //            Humidity = latestHumidity,
        //            Time = latestTime,
        //            Date = latestDate,
        //            Patient = patient
        //        };

        //        // Return the JSON response
        //        return Ok(responseData);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error and return a 500 Internal Server Error response
        //        Console.WriteLine($"Error retrieving latest data: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}



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



        //[HttpGet("monitor")]
        //public async Task<ActionResult<string>> StartTemperatureMonitoring()
        //{
        //    try
        //    {
        //        // Start monitoring the temperature
        //        await _fireBaseServices.MonitorTemperature();

        //        return Ok(new ApiResponse(200, "email sent successfully"));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error and return a 500 Internal Server Error response
        //        Console.WriteLine($"Error starting temperature monitoring: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpGet("HealthData")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> Get()
        {
            try
            {
                var healthData = await _fireBaseServices.GetCurrentHealthDataAsync();
                return Ok(healthData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/HealthData/Temperature
        [HttpGet("Temperature")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> GetTemperature()
        {
            try
            {
                var temperatureData = await _fireBaseServices.GetTemperatureDataAsync();
                return Ok(temperatureData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/HealthData/Spo2
        [HttpGet("Spo2")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> GetSpo2()
        {
            try
            {
                var spo2Data = await _fireBaseServices.GetSpo2DataAsync();
                return Ok(spo2Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/HealthData/HeartRate
        [HttpGet("HeartRate")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> GetHeartRate()
        {
            try
            {
                var heartRateData = await _fireBaseServices.GetHeartRateDataAsync();
                return Ok(heartRateData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/HealthData/Humidity
        [HttpGet("Humidity")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> GetHumidity()
        {
            try
            {
                var humidityData = await _fireBaseServices.GetHumidityDataAsync();
                return Ok(humidityData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // GET: api/HealthData/LatestReading
        [HttpGet("LatestReading")]
        public async Task<ActionResult<Dictionary<string, object>>> GetLatestReading()
        {
            try
            {
                var latestReading = await _fireBaseServices.GetLatestReadingAsync();
                return Ok(latestReading);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
    }
}
