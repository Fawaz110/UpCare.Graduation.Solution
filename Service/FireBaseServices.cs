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

namespace Service
{
    public class FireBaseServices
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly string _recipientEmailAddress;

        public FireBaseServices()
        {
            _firebaseClient = new FirebaseClient("https://nursecare-4613f-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("YKSySoRUzTn5ih8ZGd4Y1WXgreNEYJpsHZSFu4Zv")
                });

            _recipientEmailAddress = "mariam.sameh.duk@gmail.com";

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


        public async Task<(float, float, string, string)> GetLatestData()
        {
            // Retrieve the latest data from each dictionary
            var latestTemperature = await GetLatestEntry("temp");
            var latestHumidity = await GetLatestEntry("hum");
            var latestTime = await GetLatestDateTime("time");
            var latestDate = await GetLatestDateTime("date");

            return (latestTemperature, latestHumidity, latestTime, latestDate);
        }

        private async Task<float> GetLatestEntry(string nodeName)
        {
            try
            {
                // Query Firebase database for the specified node
                var firebaseData = await _firebaseClient.Child(nodeName).OnceAsync<float>();

                // Find the latest entry
                var latestEntry = firebaseData.LastOrDefault();

                if (latestEntry != null)
                    return latestEntry.Object;
                
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error retrieving latest entry from Firebase for {nodeName}: {ex.Message}");
            }

            // Return default value if no data found or error occurred
            return default;
        }

        private async Task<string> GetLatestDateTime(string nodeName)
        {
            try
            {
                // Query Firebase database for the specified node
                var firebaseData = await _firebaseClient.Child(nodeName).OnceAsync<string>();

                // Find the latest entry
                var latestEntry = firebaseData.LastOrDefault();

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
        public async Task MonitorTemperature()
        {
            // var latestTemperature = await GetLatestEntry("temp");
            // Continuously monitor temperature data
            while (true)
            {
                try
                {
                    // Retrieve the latest temperature value from Firebase
                    var latestTemperature = await GetLatestEntry("temp");

                    // Check if the temperature equals 25
                    if (latestTemperature < 100)
                    {
                        // Send a notification message
                        SendEmailNotification("Temperature reached 25 degrees Celsius!");

                        break;
                    }
                }
                catch (Exception ex)
                {
                    // Handle error
                    Console.WriteLine($"Error monitoring temperature: {ex.Message}");
                }

                // Wait for a specified interval before checking again
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        
        private void SendEmailNotification(string message)
        {
            try
            {
                // Create and configure the SMTP client
                using (var smtpClient = new SmtpClient("bulk.smtp.mailtrap.io", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("api", "be03d5972bfd0863bcc85baab6b81934");

                    // Create and send the email message
                    var mailMessage = new MailMessage("mailtrap@demomailtrap.com", _recipientEmailAddress)
                    {
                        Subject = "Temperature Notification",
                        Body = message
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
        



    

