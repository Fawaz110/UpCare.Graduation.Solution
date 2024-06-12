
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Microsoft.AspNetCore.Identity;
using Repository;
using Repository.UpCareData;
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
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IPatientRoomRepository, PatientRoomRepository>();
            services.AddScoped<IConsultationRepository, ConsultationRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<INurseCareRepository, NurseCareRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();

            #endregion

            #region UnitOfWork Registration

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Services Registrations

            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<ICheckupService, CheckupService>();
            services.AddScoped<IRadiologyService, RadiologyService>();
            services.AddScoped<IConsultationService, ConsultationService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddScoped<INurseCareService, NurseCareService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));

            #endregion

            services.AddAutoMapper(map => map.AddProfile(new MappingProfiles()));

            services.AddScoped(typeof(SignInManager<>));
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped(typeof(RoleManager<>));
            services.AddScoped<FireBaseServices>();
            services.AddScoped<IMediator, Mediator>();
            //services.AddSingleton<UpCareDbContext>();
            services.AddControllers();

            services.AddSignalR();


            return services;
        }
    }
}
