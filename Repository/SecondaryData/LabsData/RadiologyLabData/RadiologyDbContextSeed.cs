using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.LabsData.RadiologyLabData
{
    public static class RadiologyDbContextSeed
    {
        public static async Task RadiologyUserSeed(UserManager<RadiologyLab> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new RadiologyLab()
                {
                    Name = "R01",
                    PhoneNumber = "01123456789",
                    Location = "Ground floor",
                    Email = "main@rad.com",
                    UserName = "Main"
                };

                var result = await userManager.CreateAsync(user, "Main@123");

                if (result.Succeeded)
                    logger.LogInformation("Radiology lab seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding radiology lab users data..(*_*)");

            }
            else
            {
                logger.LogWarning("No need for check radiology lab seeding values");
            }
        }
    }
}
