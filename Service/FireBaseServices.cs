using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Microsoft.Data.SqlClient;
using Core.UpCareUsers;

namespace Service
{
    public class FireBaseServices
    {
        private readonly FirebaseClient _firebaseClient;
        public FireBaseServices()
        {
            _firebaseClient = new FirebaseClient("https://nursecare-4613f-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("YKSySoRUzTn5ih8ZGd4Y1WXgreNEYJpsHZSFu4Zv")
                });
        }



        
        public async Task<Dictionary<string, float>> GetCurrentTemperatureAsync()
        {
            var temperatureData = new Dictionary<string, float>();
            var humidityData = new Dictionary<string, float>();
            try
            {
                temperatureData = await _firebaseClient
                    .Child("temp")
                    .OnceSingleAsync<Dictionary<string, float>>();
            }
            catch (Exception ex)
            {
                // Handle errors properly
                Console.WriteLine($"Error: Failed to retrieve current temperature data from Firebase. {ex}");
                throw;
            }

            return temperatureData;
        }


        public async Task<List<Dictionary<string, object>>> GetCurrentDataAsync()
        {
            var temperatureData = await GetDataForChildAsync("temp");
            var humidityData = await GetDataForChildAsync("hum");
            var timeData = await GetDataForChildAsync("time");
            var dateData = await GetDataForChildAsync("date");

            var currentDataList = new List<Dictionary<string, object>>();

            // foreach (var timestamp in temperatureData.Keys)
            //{
            //  if (humidityData.ContainsKey(timestamp) &&
            //    timeData.ContainsKey(timestamp) &&
            //  dateData.ContainsKey(timestamp))
            //{
            //  var temperature = temperatureData[timestamp];
            //  var humidity = humidityData[timestamp];
            //  var time = timeData[timestamp];
            //  var date = dateData[timestamp];

            var data = new Dictionary<string, object>();
            data.Add("temp", temperatureData);
            data.Add("hum", humidityData);
            data.Add("time", timeData);
            data.Add("date", dateData);
            currentDataList.Add(data);
            //    }
            //}

            return currentDataList;
        }

        private async Task<Dictionary<string, object>> GetDataForChildAsync(string childNode)
        {
            var data = new Dictionary<string, object>();

            try
            {
                data = await _firebaseClient
                    .Child(childNode)
                    .OnceSingleAsync<Dictionary<string, object>>();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error: Failed to retrieve {childNode} data from Firebase. {ex}");
                throw;
            }

            return data;
        }


        public async Task<(double, double, DateTime, DateTime)> GetLatestData()
        {
            // Retrieve the latest data from each dictionary
            var latestTemperature = await GetLatestEntry("temp");
            var latestHumidity = await GetLatestEntry("hum");
            var latestTime = await GetLatestDateTime("time");
            var latestDate = await GetLatestDateTime("date");

            return (latestTemperature, latestHumidity, latestTime, latestDate);
        }

        private async Task<double> GetLatestEntry(string nodeName)
        {
            try
            {
                // Query Firebase database for the specified node
                var firebaseData = await _firebaseClient.Child(nodeName).OnceAsync<double>();

                // Find the latest entry
                var latestEntry = firebaseData.OrderByDescending(x => DateTime.Parse(x.Key)).FirstOrDefault();

                if (latestEntry != null)
                {
                    return latestEntry.Object;
                }
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error retrieving latest entry from Firebase for {nodeName}: {ex.Message}");
            }

            // Return default value if no data found or error occurred
            return default;
        }

        private async Task<DateTime> GetLatestDateTime(string nodeName)
        {
            try
            {
                // Query Firebase database for the specified node
                var firebaseData = await _firebaseClient.Child(nodeName).OnceAsync<DateTime>();

                // Find the latest entry
                var latestEntry = firebaseData.OrderByDescending(x => x.Object).FirstOrDefault();

                if (latestEntry != null)
                {
                    return latestEntry.Object;
                }
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error retrieving latest datetime from Firebase for {nodeName}: {ex.Message}");
            }

            // Return default value if no data found or error occurred
            return default;
        }



    }
}
        



    

