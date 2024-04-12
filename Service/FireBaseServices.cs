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

     

        public async Task<(double, double)> GetTemperatureAndHumidityAsync()
        {
            var temperatureNode = "temp"; 
            var humidityNode = "hum"; 

            var temperature = await _firebaseClient
                .Child(temperatureNode)
                .OnceSingleAsync<double>();

            var humidity = await _firebaseClient
                .Child(humidityNode)
                .OnceSingleAsync<double>();

            return (temperature, humidity);
        }

    }
}
