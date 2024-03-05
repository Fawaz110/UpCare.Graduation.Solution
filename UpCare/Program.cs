using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.SecondaryData.AdminData;
using Repository.SecondaryData.DoctorData;
using Repository.SecondaryData.LabsData.CheckupLabData;
using Repository.SecondaryData.LabsData.RadiologyLabData;
using Repository.SecondaryData.NurseData;
using Repository.SecondaryData.PatientData;
using Repository.SecondaryData.PharmacyData;
using Repository.SecondaryData.ReceptionistData;
using Repository.UpCareData;
using UpCare.Extensions;

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
            ////////////////////////// 
            ////////////////////////// 
            ////////////////////////// tables pendings (prescription and dependent tables)
            //////////////////////////        pendings (bills and dependent tables)
            ////////////////////////// 
            //////////////////////////
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

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AdminDbContext>();
            // End Of Admin Services

            // Receptionist Connection & Services Configuration
            builder.Services.AddDbContext<ReceptionistDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ReceptionistConnection"));
            });
            builder.Services.AddIdentityCore<Receptionist>(options =>
            {

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ReceptionistDbContext>();
            // End Of Receptionist Services

            // Doctor Connection & Services Configuration
            builder.Services.AddDbContext<DoctorDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DoctorConnection"));
            });
            builder.Services.AddIdentityCore<Doctor>(options =>
            {

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<DoctorDbContext>();
            // End Of Doctor Services

            // Nurse Connection & Services Configuration
            builder.Services.AddDbContext<NurseDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("NurseConnection"));
            });
            builder.Services.AddIdentityCore<Nurse>(options =>
            {

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<NurseDbContext>();
            // End Of Nurse Services

            // CheckupLabs Connection & Services Configuration
            builder.Services.AddDbContext<CheckupLabDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LabConnection"));
            });
            builder.Services.AddIdentityCore<CheckupLab>(options =>
            {

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<CheckupLabDbContext>();
            // End Of Lab Services

            // Patient Connection & Services Configuration
            builder.Services.AddDbContext<PatientDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("PatientConnection"));
            });
            builder.Services.AddIdentityCore<Patient>(options =>
            {

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<PatientDbContext>();
            // End Of Patient Services

            // RadiologyDbContext Connection & Services Configuration
            builder.Services.AddDbContext<RadiologyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("RadioogyConnection"));
            });
            builder.Services.AddIdentityCore<RadiologyLab>(options =>
            {

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<RadiologyDbContext>();
            // End Of Radiology Services

            // PharmacyDbContext Connection & Services Configuration
            builder.Services.AddDbContext<PharmacyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("PharmacyConnection"));
            });
            builder.Services.AddIdentityCore<Pharmacy>(options =>
            {

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<PharmacyDbContext>();
            // End Of PharmacyDbContext Services

            var app = builder.Build();

            await app.Services.ApplyMigrateAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}