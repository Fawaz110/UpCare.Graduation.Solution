using Core.Repositories.Contract;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.SecondaryData.AdminData;
using Repository.SecondaryData.DoctorData;
using Repository.SecondaryData.LabsData.CheckupLabData;
using Repository.SecondaryData.LabsData.RadiologyLabData;
using Repository.SecondaryData.NurseData;
using Repository.SecondaryData.PatientData;
using Repository.SecondaryData.PharmacyData;
using Repository.SecondaryData.ReceptionistData;
using Repository.UpCareData;
using UpCare.Errors;
using UpCare.Extensions;
using UpCare.Hubs;

namespace UpCare
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Project Moooooooooooooooott (x_x)

            #region Connections
            // UpCare Connection & Services Configuration
            builder.Services.AddDbContext<UpCareDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // End Of UpCare Services

            // Admin Connection & Services Configuration
            builder.Services.AddDbContext<AdminDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdminConnection"));
            });

            builder.Services.AddIdentityCore<Admin>(options =>
            {

            }).AddEntityFrameworkStores<AdminDbContext>();
            // End Of Admin Services

            // Receptionist Connection & Services Configuration
            builder.Services.AddDbContext<ReceptionistDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ReceptionistConnection"));
            });
            builder.Services.AddIdentityCore<Receptionist>(options =>
            {

            }).AddEntityFrameworkStores<ReceptionistDbContext>();
            // End Of Receptionist Services

            // Nurse Connection & Services Configuration
            builder.Services.AddDbContext<NurseDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("NurseConnection"));
            });
            builder.Services.AddIdentityCore<Nurse>(options =>
            {

            }).AddEntityFrameworkStores<NurseDbContext>();
            // End Of Nurse Services

            // CheckupLabs Connection & Services Configuration
            builder.Services.AddDbContext<CheckupLabDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LabConnection"));
            });
            builder.Services.AddIdentityCore<CheckupLab>(options =>
            {

            }).AddEntityFrameworkStores<CheckupLabDbContext>();
            // End Of Lab Services

            // Patient Connection & Services Configuration
            builder.Services.AddDbContext<PatientDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("PatientConnection"));
            });
            builder.Services.AddIdentity<Patient, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<PatientDbContext>()
            .AddDefaultTokenProviders();
            // End Of Patient Services

            // Doctor Connection & Services Configuration
            builder.Services.AddDbContext<DoctorDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DoctorConnection"));
            });

            builder.Services.AddIdentityCore<Doctor>(options =>
            {

            })
            .AddEntityFrameworkStores<DoctorDbContext>()
            .AddDefaultTokenProviders();

            //builder.Services.AddScoped(typeof(IUserRoleStore<Doctor>));
            // End Of Doctor Services

            // RadiologyDbContext Connection & Services Configuration
            builder.Services.AddDbContext<RadiologyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("RadioogyConnection"));
            });
            builder.Services.AddIdentityCore<RadiologyLab>(options =>
            {

            }).AddEntityFrameworkStores<RadiologyDbContext>();
            // End Of Radiology Services

            // PharmacyDbContext Connection & Services Configuration
            builder.Services.AddDbContext<PharmacyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("PharmacyConnection"));
            });
            builder.Services.AddIdentityCore<Pharmacy>(options =>
            {

            }).AddEntityFrameworkStores<PharmacyDbContext>();
            // End Of PharmacyDbContext Services 
            #endregion

            #region ServicesConfigurations
            builder.Services.AddServices();

            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(e => e.ErrorMessage)
                                                         .ToArray();
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors.ToList()
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            #endregion

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options => {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            var app = builder.Build();



            await app.Services.ApplyMigrateAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<ChatHub>("/chat");
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();

        }
    }
}