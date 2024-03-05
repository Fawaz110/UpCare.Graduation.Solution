using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.PharmacyData
{
    public class PharmacyDbContextSeed
    {
        public static async Task PharmacyUserSeed(UserManager<Pharmacy> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new Pharmacy()
                {
                    Name = "P01",
                    PhoneNumber = "01123456789",
                    Location = "Ground floor",
                    Email = "main@pharmacy.com",
                    UserName = "Main"
                };

                var result = await userManager.CreateAsync(user, "Main@123");

                if (result.Succeeded)
                    logger.LogInformation("Pharmacy lab seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding pharmacy lab users data..(*_*)");

            }
            else
            {
                logger.LogWarning("No need for pharmacy seeding values");
            }
        }
    }
}
