
using Core.Services.Contract;
using Service;

namespace UpCare.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));
            
            return services;
        }
    }
}
