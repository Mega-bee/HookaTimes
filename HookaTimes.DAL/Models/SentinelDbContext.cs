using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class SentinelDbContext : DbContext
    {
        public SentinelDbContext()
        {
        }

        public SentinelDbContext(DbContextOptions<SentinelDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccProfile> AccProfiles { get; set; }
        public virtual DbSet<AccProfileRole> AccProfileRoles { get; set; }
        public virtual DbSet<Anomaly> Anomalies { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MetricLog> MetricLogs { get; set; }
        public virtual DbSet<MetricRange> MetricRanges { get; set; }
        public virtual DbSet<MetricType> MetricTypes { get; set; }
        public virtual DbSet<MetricsNormalValue> MetricsNormalValues { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientDevice> PatientDevices { get; set; }
        public virtual DbSet<PatientMedicine> PatientMedicines { get; set; }
        public virtual DbSet<PriorityLevelEvaluation> PriorityLevelEvaluations { get; set; }
        public virtual DbSet<RawPatientMetric> RawPatientMetrics { get; set; }
        public virtual DbSet<UserDevicetoken> UserDevicetokens { get; set; }
        public virtual DbSet<PhoneOtp> PhoneOtps { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccProfile>(entity =>
            {
                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.AccProfiles)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_ACC_Profiles_Genders");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AccProfiles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ACC_Profiles_ACC_ProfileRoles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccProfiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ACC_Profiles_AspNetUsers");
            });

            modelBuilder.Entity<Anomaly>(entity =>
            {
                entity.HasOne(d => d.MetricType)
                    .WithMany(p => p.Anomalies)
                    .HasForeignKey(d => d.MetricTypeId)
                    .HasConstraintName("FK_Anomaly_MetricType");

                entity.HasOne(d => d.MonitoredByNavigation)
                    .WithMany(p => p.Anomalies)
                    .HasForeignKey(d => d.MonitoredBy)
                    .HasConstraintName("FK_Anomaly_AspNetUsers");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Anomalies)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Anomaly_Patient");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_AspNetUsers_Genders");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Device_Device");
            });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.Property(e => e.Title).IsFixedLength(true);
            });

            modelBuilder.Entity<MetricLog>(entity =>
            {
                entity.HasOne(d => d.MetricType)
                    .WithMany(p => p.MetricLogs)
                    .HasForeignKey(d => d.MetricTypeId)
                    .HasConstraintName("FK_MetricLog_MetricType");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.MetricLogs)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_MetricLog_Patient");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.MetricLogs)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_MetricLog_MetricsNormalValues");
            });

            modelBuilder.Entity<MetricsNormalValue>(entity =>
            {
                entity.HasOne(d => d.MetricType)
                    .WithMany(p => p.MetricsNormalValues)
                    .HasForeignKey(d => d.MetricTypeId)
                    .HasConstraintName("FK_MetricsNormalValues_MetricType");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.MetricsNormalValues)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_MetricsNormalValues_Patient");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Notification_AspNetUsers");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Patient_Genders");
            });

            modelBuilder.Entity<PatientDevice>(entity =>
            {
                entity.HasOne(d => d.Device)
                    .WithMany(p => p.PatientDevices)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_PatientDevices_Device");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientDevices)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientDevices_Patient");
            });

            modelBuilder.Entity<PatientMedicine>(entity =>
            {
                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.PatientMedicines)
                    .HasForeignKey(d => d.MedicineId)
                    .HasConstraintName("FK_PatientMedicine_Medicine");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientMedicines)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientMedicine_Patient");
            });

            modelBuilder.Entity<UserDevicetoken>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDevicetokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserDevicetoken_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
