using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.LabsData.CheckupLabData
{
    public static class CheckupLabDbContextSeed
    {
        public static async Task CheckupLabUserSeed(UserManager<CheckupLab> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new CheckupLab()
                {
                    Name = "A01",
                    PhoneNumber = "01123456789",
                    Location = "Ground floor",
                    Email = "main@checkup.com",
                    UserName = "Main"
                };

                var result = await userManager.CreateAsync(user, "Main@123");

                if (result.Succeeded)
                    logger.LogInformation("Checkup lab seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding checkup lab users data..(*_*)");

            }
            else
            {
                logger.LogWarning("No need for check lab seeding values");
            }
        }
    }
}
