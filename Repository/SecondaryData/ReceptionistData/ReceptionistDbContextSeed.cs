using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.ReceptionistData
{
    public static class ReceptionistDbContextSeed
    {
        public static async Task ReceptionistUsersSeed(UserManager<Receptionist> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new Receptionist()
                {
                    FirstName = "Mustafa",
                    LastName = "Mohamed",
                    PhoneNumber = "01123456789",
                    Address = "Minufia",
                    Email = "fawaz@receptionist.com",
                    UserName = "Mustafa"
                };

                var result = await userManager.CreateAsync(user, "Fawaz@123");

                if (result.Succeeded)
                    logger.LogInformation("Receptionist seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding receptionist users data..(*_*)");

            }
            else
            {
                logger.LogWarning("No need for receptionist seeding values");
            }
        }
    }
}
