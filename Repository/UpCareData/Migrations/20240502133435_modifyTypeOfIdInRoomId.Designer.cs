﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.UpCareData;

#nullable disable

namespace Repository.UpCareData.Migrations
{
    [DbContext(typeof(UpCareDbContext))]
    [Migration("20240502133435_modifyTypeOfIdInRoomId")]
    partial class modifyTypeOfIdInRoomId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Entities.UpCareEntities.Checkup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Checkups");
                });

            modelBuilder.Entity("Core.Entities.UpCareEntities.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FK_AdminId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("Core.Entities.UpCareEntities.Radiology", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Radiologies");
                });

            modelBuilder.Entity("Core.Entities.UpCareEntities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AvailableBeds")
                        .HasColumnType("int");

                    b.Property<string>("FK_ReceptionistId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfBeds")
                        .HasColumnType("int");

                    b.Property<decimal>("PricePerNight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Core.UpCareEntities.BillEntities.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveredService")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PaidMoney")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("Core.UpCareEntities.BillEntities.CheckupInBill", b =>
                {
                    b.Property<int>("FK_CheckupId")
                        .HasColumnType("int");

                    b.Property<int>("FK_BillId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("FK_CheckupId", "FK_BillId");

                    b.ToTable("CheckupInBills");
                });

            modelBuilder.Entity("Core.UpCareEntities.BillEntities.MedicineInBill", b =>
                {
                    b.Property<int>("FK_MedicineId")
                        .HasColumnType("int");

                    b.Property<int>("FK_BillId")
                        .HasColumnType("int");

                    b.HasKey("FK_MedicineId", "FK_BillId");

                    b.ToTable("MedicineInBills");
                });

            modelBuilder.Entity("Core.UpCareEntities.BillEntities.RadiologyInBill", b =>
                {
                    b.Property<int>("FK_RadiologyId")
                        .HasColumnType("int");

                    b.Property<int>("FK_BillId")
                        .HasColumnType("int");

                    b.HasKey("FK_RadiologyId", "FK_BillId");

                    b.ToTable("RadiologyInBill");
                });

            modelBuilder.Entity("Core.UpCareEntities.DoctorDoOperation", b =>
                {
                    b.Property<string>("FK_PatientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FK_OperationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("FK_AdminId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FK_DoctorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FK_PatientId", "FK_OperationId", "Date");

                    b.ToTable("DoctorDoOperations");
                });

            modelBuilder.Entity("Core.UpCareEntities.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FK_PatientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Core.UpCareEntities.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FK_PharmacyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Indecations")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ProductionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SideEffects")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("Core.UpCareEntities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Core.UpCareEntities.NurseCare", b =>
                {
                    b.Property<string>("FK_NurseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FK_RoomId")
                        .HasColumnType("int");

                    b.Property<string>("FK_PatientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("BeatPerMinute")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BloodPresure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OxygenSaturation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suger")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FK_NurseId", "FK_RoomId", "FK_PatientId", "DateTime");

                    b.ToTable("NurseCares");
                });

            modelBuilder.Entity("Core.UpCareEntities.PatientAppointment", b =>
                {
                    b.Property<string>("FK_PatientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FK_DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("FK_PatientId", "FK_DoctorId", "DateTime");

                    b.ToTable("PatientAppointments");
                });

            modelBuilder.Entity("Core.UpCareEntities.PatientBookRoom", b =>
                {
                    b.Property<string>("FK_PatientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FK_DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FK_RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfRecievedBeds")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("FK_PatientId", "FK_DoctorId", "FK_RoomId");

                    b.ToTable("PatientBookRooms");
                });

            modelBuilder.Entity("Core.UpCareEntities.PatientConsultation", b =>
                {
                    b.Property<string>("FK_PatientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FK_DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("FK_PatientId", "FK_DoctorId", "DateTime");

                    b.ToTable("PatientConsultations");
                });

            modelBuilder.Entity("Core.UpCareEntities.PrescriptionEntities.CheckupInPrescription", b =>
                {
                    b.Property<int>("FK_CheckupId")
                        .HasColumnType("int");

                    b.Property<int>("FK_PrescriptionId")
                        .HasColumnType("int");

                    b.HasKey("FK_CheckupId", "FK_PrescriptionId");

                    b.ToTable("CheckupInPrescriptions");
                });

            modelBuilder.Entity("Core.UpCareEntities.PrescriptionEntities.MedicineInPrescription", b =>
                {
                    b.Property<int>("FK_MedicineId")
                        .HasColumnType("int");

                    b.Property<int>("FK_PrescriptionId")
                        .HasColumnType("int");

                    b.HasKey("FK_MedicineId", "FK_PrescriptionId");

                    b.ToTable("MedicineInPrescriptions");
                });

            modelBuilder.Entity("Core.UpCareEntities.PrescriptionEntities.Prescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Advice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Diagnosis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FK_DoctorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FK_PatientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("Core.UpCareEntities.PrescriptionEntities.RadiologyInPrescription", b =>
                {
                    b.Property<int>("FK_PrescriptionId")
                        .HasColumnType("int");

                    b.Property<int>("FK_RadiologyId")
                        .HasColumnType("int");

                    b.HasKey("FK_PrescriptionId", "FK_RadiologyId");

                    b.ToTable("RadiologyInPrescriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
