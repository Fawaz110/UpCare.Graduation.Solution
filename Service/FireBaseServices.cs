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

        public FireBaseServices() {
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

        public async Task<List<(float temperature, float humidity)>> GetAllTemperatureAndHumidityAsync()
        {
            try
            {
                var temperatureAndHumidityData = new List<(float temperature, float humidity)>();

                var dataSnapshot = await _firebaseClient
                    .Child("temperature_and_humidity")
                    .OnceAsync<Dictionary<string, float>>();

                if (dataSnapshot == null)
                {
                    throw new Exception("No data found in Firebase.");
                }

                foreach (var data in dataSnapshot)
                {
                    if (data.Object != null && data.Object.ContainsKey("temp") && data.Object.ContainsKey("hum"))
                    {
                        var temperature = data.Object["temp"];
                        var humidity = data.Object["hum"];

                        temperatureAndHumidityData.Add((temperature, humidity));
                    }
                }

                if (temperatureAndHumidityData.Count == 0)
                {
                    throw new Exception("No temperature and humidity data found in Firebase.");
                }

                return temperatureAndHumidityData;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve temperature and humidity data from Firebase.", ex);
            }



        }
    }
    }
