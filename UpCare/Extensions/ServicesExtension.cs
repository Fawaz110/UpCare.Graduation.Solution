
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Microsoft.AspNetCore.Identity;
using Repository;
using Service;
using UpCare.Helpers;

namespace UpCare.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(map => map.AddProfile(new MappingProfiles()));

            services.AddScoped(typeof(SignInManager<>));
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped(typeof(RoleManager<>));

            return services;
        }
    }
}
