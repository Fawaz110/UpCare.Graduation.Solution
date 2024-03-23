using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.PatientData
{
    public static class PatientDbContextSeed
    {
        public static async Task PatientUsersSeed(UserManager<Patient> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new Patient()
                {
                    FirstName = "Mustafa",
                    LastName = "Mohamed",
                    PhoneNumber = "01123456789",
                    Address = "Minufia",
                    Email = "fawaz@admin.com",
                    UserName = "Mustafa",
                };

                var result = await userManager.CreateAsync(user, "Fawaz@123");

                if (result.Succeeded)
                    logger.LogInformation("Patient seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding patient users data..(*_*)");

            }
            else
            {
                logger.LogWarning("No need for patient seeding values");
            }
        }
    }
}
