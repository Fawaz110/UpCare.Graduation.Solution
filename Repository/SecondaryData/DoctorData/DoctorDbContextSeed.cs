using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.DoctorData
{
    public static class DoctorDbContextSeed
    {
        public static async Task DoctorUsersSeed(UserManager<Doctor> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new Doctor()
                {
                    FirstName = "Mustafa",
                    LastName = "Mohamed",
                    PhoneNumber = "01123456789",
                    Address = "Minufia",
                    Email = "fawaz@doctor.com",
                    UserName = "Mustafa"
                };

                var result = await userManager.CreateAsync(user, "Fawaz@123");

                if (result.Succeeded)
                    logger.LogInformation("Doctor seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding doctor users data..(*_*)");
            }
            else
            {
                logger.LogWarning("No need for doctor seeding values");
            }
        }
    }
}
