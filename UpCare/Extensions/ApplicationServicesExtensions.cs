﻿using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.SecondaryData.AdminData;
using Repository.SecondaryData.DoctorData;
using Repository.SecondaryData.LabsData.CheckupLabData;
using Repository.SecondaryData.LabsData.RadiologyLabData;
using Repository.SecondaryData.NurseData;
using Repository.SecondaryData.PatientData;
using Repository.SecondaryData.ReceptionistData;

namespace UpCare.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public async static Task ApplyMigrateAsync(this IServiceProvider service)
        {
            var scope = service.CreateScope();

            var services = scope.ServiceProvider;

            var _loggerFactory = services.GetRequiredService<ILoggerFactory>();

            var logger = _loggerFactory.CreateLogger<Program>();

            var _adminDbContext = services.GetRequiredService<AdminDbContext>();

            var _receptionistDbContext = services.GetRequiredService<ReceptionistDbContext>();

            var _doctorDbContext = services.GetRequiredService<DoctorDbContext>();

            var _nurseDbContext = services.GetRequiredService<NurseDbContext>();

            var _checkupLabDbContext = services.GetRequiredService<CheckupLabDbContext>();

            var _radiologyLabDbContext = services.GetRequiredService<RadiologyDbContext>();

            var _patientDbContext = services.GetRequiredService<PatientDbContext>();


            try
            {
                var patientUserManager = services.GetRequiredService<UserManager<Patient>>();
                await PatientDbContextSeed.PatientUsersSeed(patientUserManager, logger);
                await _patientDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured during excuting PatientDbContext pending migrations..(*_*)");
            }

            try
            {
                var radiologyUserManager = services.GetRequiredService<UserManager<RadiologyLab>>();
                await RadiologyDbContextSeed.RadiologyUserSeed(radiologyUserManager, logger);
                await _radiologyLabDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured during excuting RadiologyDbContext pending migrations..(*_*)");
            }

            try
            {
                var checkupLabUserManager = services.GetRequiredService<UserManager<CheckupLab>>();
                await CheckupLabDbContextSeed.CheckupLabUserSeed(checkupLabUserManager, logger);
                await _checkupLabDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured during excuting CheckupLabDbContext pending migrations..(*_*)");
            }

            try
            {
                var nurseUserManager = services.GetRequiredService<UserManager<Nurse>>();
                await NurseDbContextSeed.NurseUsersSeed(nurseUserManager, logger);
                await _nurseDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured during excuting NurseDbContext pending migrations..(*_*)");
            }

            try
            {
                var doctorUserManager = services.GetRequiredService<UserManager<Doctor>>();
                await DoctorDbContextSeed.DoctorUsersSeed(doctorUserManager, logger);
                await _doctorDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured during excuting DoctorDbContext pending migrations..(*_*)");
            }

            try
            {
                var receptionistUserManager = services.GetRequiredService<UserManager<Receptionist>>();
                await ReceptionistDbContextSeed.ReceptionistUsersSeed(receptionistUserManager, logger);
                await _receptionistDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured during excuting ReceptionistDbContext pending migrations..(*_*)");
            }

            try
            {
                var adminUserManager = services.GetRequiredService<UserManager<Admin>>();
                await AdminDbContextSeed.AdminUsersSeed(adminUserManager, logger);
                await _adminDbContext.Database.MigrateAsync();
            }
            catch(Exception ex) 
            {
                logger.LogError(ex, "Error occured during excuting AdminDbContext pending migrations..(*_*)");
            }


        }
    }
}
