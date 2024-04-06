
using Core.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Service;

namespace UpCare.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));

            services.AddScoped(typeof(SignInManager<>));
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped(typeof(RoleManager<>));

            return services;
        }
    }
}
