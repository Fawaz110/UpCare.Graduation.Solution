using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;

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



        public async Task<(float, float)> GetTemperatureAndHumidityAsync()
        {
            var temperatureNode = "temp";
            var humidityNode = "hum";

            var temperature = await _firebaseClient
                .Child(temperatureNode)
                .OnceSingleAsync<float>();

            var humidity = await _firebaseClient
                .Child(humidityNode)
                .OnceSingleAsync<float>();

            return (temperature, humidity);
        }

        //public async Task<List<(float temperature, float humidity)>> GetAllTemperatureAndHumidityAsync()
        //{
        //    try
        //    {
        //        var temperatureAndHumidityData = new List<(float temperature, float humidity)>();

        //        var dataSnapshot = await _firebaseClient
        //            .Child("temperature_and_humidity")
        //            .OnceAsync<Dictionary<string, float>>();

        //        if (dataSnapshot == null)
        //        {
        //            throw new Exception("No data found in Firebase.");
        //        }

        //        foreach (var data in dataSnapshot)
        //        {
        //            if (data.Object != null && data.Object.ContainsKey("temp") && data.Object.ContainsKey("hum"))
        //            {
        //                var temperature = data.Object["temp"];
        //                var humidity = data.Object["hum"];

        //                temperatureAndHumidityData.Add((temperature, humidity));
        //            }
        //        }

        //        if (temperatureAndHumidityData.Count == 0)
        //        {
        //            throw new Exception("No temperature and humidity data found in Firebase.");
        //        }

        //        return temperatureAndHumidityData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to retrieve temperature and humidity data from Firebase.", ex);
        //    }



        //}

        public async Task<(List<float> temperature, List<float> humidity)> GetAllTemperatureAndHumidityAsync()
        {
            try
            {
                var temperature = await _firebaseClient
                        .Child("temp")
                        .OnceSingleAsync<List<float>>();


                var humidity = await _firebaseClient
                    .Child("hum")
                    .OnceSingleAsync<List<float>>();

                return (temperature, humidity);
            }
            catch (Exception ex)
            {
                // Handle errors properly
                throw new Exception("Failed to retrieve temperature and humidity data from Firebase.", ex);
            }
        }

        public async Task<Dictionary<string, float>> GetCurrentTemperatureAsync()
        {
            //var temperatureData = new List<float>();
            var temperatureData = new Dictionary<string, float>();
            var humidityData= new Dictionary<string, float>();
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
                    data.Add("date", dateData );
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



    }
}


    
