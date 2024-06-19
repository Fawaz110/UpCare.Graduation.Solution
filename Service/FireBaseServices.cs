using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Microsoft.Data.SqlClient;
using Core.UpCareUsers;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class FireBaseServices
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly string _recipientEmailAddress;
        private readonly UserManager<Patient> _userManager;

        public FireBaseServices(UserManager<Patient> userManager)
        {
            _firebaseClient = new FirebaseClient("https://nursecare-4613f-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("YKSySoRUzTn5ih8ZGd4Y1WXgreNEYJpsHZSFu4Zv")
                });
            _recipientEmailAddress = "mariam.sameh.duk@gmail.com";
            _userManager = userManager;
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

            var user = await _userManager.Users.FirstOrDefaultAsync();
            data.Add("patient", user);


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


        
        public async Task<List<Dictionary<string, object>>> GetCurrentHealthDataAsync()
        {
            var healthData = await _firebaseClient.Child("HealthCare").OnceAsync<Dictionary<string, object>>();
            var currentDataList = new List<Dictionary<string, object>>();

            foreach (var entry in healthData)
            {
                var data = entry.Object;
                data["id"] = entry.Key; // Include the unique identifier in the data
                currentDataList.Add(data);
            }

            return currentDataList;
        }

        public async Task<List<Dictionary<string, object>>> GetTemperatureDataAsync()
        {
            try
            {
                var healthData = await _firebaseClient.Child("HealthCare").OnceAsync<Dictionary<string, object>>();
                var temperatureDataList = new List<Dictionary<string, object>>();

                foreach (var entry in healthData)
                {
                    var data = entry.Object;
                    if (data.ContainsKey("temperature"))
                    {
                        var temperatureEntry = new Dictionary<string, object>
                        {
                            { "id", entry.Key },
                            { "temperature", data["temperature"] }
                        };
                        temperatureDataList.Add(temperatureEntry);
                    }
                }

                return temperatureDataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving temperature data: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Dictionary<string, object>>> GetSpo2DataAsync()
        {
            try
            {
                var healthData = await _firebaseClient.Child("HealthCare").OnceAsync<Dictionary<string, object>>();
                var spo2DataList = new List<Dictionary<string, object>>();

                foreach (var entry in healthData)
                {
                    var data = entry.Object;
                    if (data.ContainsKey("spo2"))
                    {
                        var spo2Entry = new Dictionary<string, object>
                        {
                            { "id", entry.Key },
                            { "spo2", data["spo2"] }
                        };
                        spo2DataList.Add(spo2Entry);
                    }
                }

                return spo2DataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Spo2 data: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Dictionary<string, object>>> GetHeartRateDataAsync()
        {
            try
            {
                var healthData = await _firebaseClient.Child("HealthCare").OnceAsync<Dictionary<string, object>>();
                var heartRateDataList = new List<Dictionary<string, object>>();

                foreach (var entry in healthData)
                {
                    var data = entry.Object;
                    if (data.ContainsKey("heart_rate"))
                    {
                        var heartRateEntry = new Dictionary<string, object>
                        {
                            { "id", entry.Key },
                            { "heart_rate", data["heart_rate"] }
                        };
                        heartRateDataList.Add(heartRateEntry);
                    }
                }

                return heartRateDataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Heart Rate data: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Dictionary<string, object>>> GetHumidityDataAsync()
        {
            try
            {
                var healthData = await _firebaseClient.Child("HealthCare").OnceAsync<Dictionary<string, object>>();
                var humidityDataList = new List<Dictionary<string, object>>();

                foreach (var entry in healthData)
                {
                    var data = entry.Object;
                    if (data.ContainsKey("humidity"))
                    {
                        var humidityEntry = new Dictionary<string, object>
                        {
                            { "id", entry.Key },
                            { "humidity", data["humidity"] }
                        };
                        humidityDataList.Add(humidityEntry);
                    }
                }

                return humidityDataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Humidity data: {ex.Message}");
                throw;
            }
        }
        public async Task<Dictionary<string, object>> GetLatestReadingAsync()
        {
            try
            {
                var healthData = await _firebaseClient.Child("HealthCare").OnceAsync<Dictionary<string, object>>();

                var latestReading = new Dictionary<string, object>();

                // Get the latest entry for each parameter
                var latestTemperature = GetLatestParameterEntry(healthData, "temperature");
                var latestHumidity = GetLatestParameterEntry(healthData, "humidity");
                var latestHeartRate = GetLatestParameterEntry(healthData, "heart_rate");
                var latestSpo2 = GetLatestParameterEntry(healthData, "spo2");

                // Add latest readings to the dictionary
                latestReading.Add("temperature", latestTemperature);
                latestReading.Add("humidity", latestHumidity);
                latestReading.Add("heart_rate", latestHeartRate);
                latestReading.Add("spo2", latestSpo2);

                if (latestHeartRate.ContainsKey("heart_rate") && double.TryParse(latestHeartRate["heart_rate"].ToString(), out double heartRate))
                {
                    if (heartRate < 90)
                    {
                        string message = $"Alert: Heart rate is below 90! Current heart rate: {heartRate}";
                        await SendEmailNotification(message);
                    }
                }

                return latestReading;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving latest reading: {ex.Message}");
                throw;
            }
        }

        private Dictionary<string, object> GetLatestParameterEntry(IEnumerable<FirebaseObject<Dictionary<string, object>>> healthData, string parameter)
        {
            var latestEntry = healthData.OrderByDescending(x => x.Object["timestamp"]).FirstOrDefault(x => x.Object.ContainsKey(parameter));

            if (latestEntry != null)
            {
                var parameterEntry = new Dictionary<string, object>
                {
                    { "id", latestEntry.Key },
                    { parameter, latestEntry.Object[parameter] }
                };
                return parameterEntry;
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }
        private async Task SendEmailNotification(string message)
        {
            try
            {
                // Create and configure the SMTP client
                using (var smtpClient = new SmtpClient("bulk.smtp.mailtrap.io", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("api", "be03d5972bfd0863bcc85baab6b81934");

                    
                    var patient = await _userManager.Users.FirstOrDefaultAsync();

                    // Create and send the email message
                    var mailMessage = new MailMessage("mailtrap@demomailtrap.com", _recipientEmailAddress)
                    {
                        Subject = "Heart Rate Alert",
                        Body = $"Patient information:\n\tPatient Name: {patient.FirstName} {patient.LastName}\n\tEmail: {patient.Email}\n\tAddress: {patient.Address}\n\tPhone Number: {patient.PhoneNumber}\n\tGender: {patient.Gender.ToString()}\n\n {message}"
                    };

                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email notification sent successfully.");
                }
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error sending email notification: {ex.Message}");
            }
        }


    }
}






