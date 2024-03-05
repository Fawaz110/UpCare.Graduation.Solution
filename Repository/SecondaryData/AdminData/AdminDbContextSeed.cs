using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Repository.SecondaryData.AdminData
{
    public static class AdminDbContextSeed
    {
        public static async Task AdminUsersSeed(UserManager<Admin> userManager, ILogger logger)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new Admin()
                {
                    FirstName = "Mustafa",
                    LastName = "Mohamed",
                    PhoneNumber = "01123456789",
                    Address = "Minufia",
                    Email = "fawaz@admin.com",
                    UserName = "Mustafa"
                };

                var result = await userManager.CreateAsync(user, "Fawaz@123");

                if (result.Succeeded)
                    logger.LogInformation("Admin seeding added successfully <3");
                else
                    logger.LogError("Error ocuured during seeding admin users data..(*_*)");

            }
            else
            {
                logger.LogWarning("No need for admin seeding values");
            }
        }
    }
}
