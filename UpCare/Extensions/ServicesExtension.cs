
using Core.Repositories.Contract;
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
            #region Repositories Registeration

            services.AddScoped<IMedicineRepository, MedicineRepository>();
            services.AddScoped<ICheckupRepository, CheckupRepository>();
            services.AddScoped<IRadiologyRepository, RadiologyRepository>();

            #endregion

            #region UnitOfWork Registration

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Services Registrations

            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<ICheckupService, CheckupService>();
            services.AddScoped<IRadiologyService, RadiologyService>();
            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));

            #endregion

            services.AddAutoMapper(map => map.AddProfile(new MappingProfiles()));

            services.AddScoped(typeof(SignInManager<>));
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped(typeof(RoleManager<>));
            services.AddScoped<FireBaseServices>();
            services.AddControllers();


            return services;
        }
    }
}
