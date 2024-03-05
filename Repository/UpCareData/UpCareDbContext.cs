﻿using Core.Entities.UpCareEntities;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Repository.UpCareData
{
    public class UpCareDbContext : DbContext
    {
        public UpCareDbContext(DbContextOptions<UpCareDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Checkup> Checkups { get; set; }
        public DbSet<Radiology> Radiologies { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<NurseCare> NurseCares { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<PatientBookRoom> PatientBookRooms { get; set; }
        public DbSet<DoctorDoOperation> DoctorDoOperations { get; set; }
        public DbSet<PatientAppointment> PatientAppointments { get; set; }
        public DbSet<PatientConsultation> PatientConsultations { get; set; }
    }
}