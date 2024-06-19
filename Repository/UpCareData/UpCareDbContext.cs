using Core.Entities.UpCareEntities;
using Core.UpCareEntities;
using Core.UpCareEntities.BillEntities;
using Core.UpCareEntities.PrescriptionEntities;
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
        public DbSet<PatientCheckup> PatientCheckups { get; set; }
        public DbSet<PatientRadiology> PatientRadiologies { get; set; }
        public DbSet<Message> Messages { get; set; }

        #region Prescription & Related Tables
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<MedicineInPrescription> MedicineInPrescriptions { get; set; }
        public DbSet<CheckupInPrescription> CheckupInPrescriptions { get; set; }
        public DbSet<RadiologyInPrescription> RadiologyInPrescriptions { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        #endregion
        
        #region Bills & Realted Tables
        public DbSet<Bill> Bills { get; set; }
        public DbSet<CheckupInBill> CheckupInBills { get; set; }
        public DbSet<MedicineInBill> MedicineInBills { get; set; } 
        #endregion
    }
}
