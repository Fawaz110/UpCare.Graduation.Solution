using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.NurseData
{
    public static class NurseDbContextSeed
    {
        public static async Task NurseUsersSeed(UserManager<Nurse> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new Nurse()
                {
                    FirstName = "Mustafa",
                    LastName = "Mohamed",
                    PhoneNumber = "01123456789",
                    Address = "Minufia",
                    Email = "fawaz@nurse.com",
                    UserName = "Mustafa"
                };

                var result = await userManager.CreateAsync(user, "Fawaz@123");

                if (result.Succeeded)
                    logger.LogInformation("Nurse seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding nurse users data..(*_*)");

            }
            else
            {
                logger.LogWarning("No need for nurse seeding values");
            }
        }
    }
}
