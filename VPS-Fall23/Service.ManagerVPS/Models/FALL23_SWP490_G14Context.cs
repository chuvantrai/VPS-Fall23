using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Service.ManagerVPS.Models
{
    public partial class FALL23_SWP490_G14Context : DbContext
    {
        public FALL23_SWP490_G14Context()
        {
        }

        public FALL23_SWP490_G14Context(DbContextOptions<FALL23_SWP490_G14Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Commune> Communes { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<ContractLog> ContractLogs { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<GlobalStatus> GlobalStatuses { get; set; } = null!;
        public virtual DbSet<ParkingTransaction> ParkingTransactions { get; set; } = null!;
        public virtual DbSet<ParkingTransactionDetail> ParkingTransactionDetails { get; set; } = null!;
        public virtual DbSet<ParkingZone> ParkingZones { get; set; } = null!;
        public virtual DbSet<ParkingZoneAbsent> ParkingZoneAbsents { get; set; } = null!;
        public virtual DbSet<ParkingZoneAttendant> ParkingZoneAttendants { get; set; } = null!;
        public virtual DbSet<ParkingZoneOwner> ParkingZoneOwners { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("ConStr"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.Username, "UQ__account__F3DBC572CFEB8036")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnType("ntext")
                    .HasColumnName("address");

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.CommuneId).HasColumnName("commune_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsBlock).HasColumnName("is_block");

                entity.Property(e => e.IsVerified).HasColumnName("is_verified");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.Property(e => e.VerifyCode).HasColumnName("verify_code");

                entity.HasOne(d => d.Commune)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CommuneId)
                    .HasConstraintName("FK__account__commune__4AB81AF0");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__account__type_id__49C3F6B7");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__city__created_by__6383C8BA");
            });

            modelBuilder.Entity<Commune>(entity =>
            {
                entity.ToTable("commune");

                entity.HasIndex(e => e.Code, "UQ__commune__357D4CF9F3B53C3D")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Communes)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__commune__created__6477ECF3");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Communes)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__commune__distric__1A14E395");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("contract");

                entity.HasIndex(e => e.ContractCode, "UQ__contract__68CFBDBA35D02D0C")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(20)
                    .HasColumnName("contract_code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PaidTransactionId)
                    .HasMaxLength(255)
                    .HasColumnName("paid_transaction_id");

                entity.Property(e => e.ParkingZoneId).HasColumnName("parking_zone_id");

                entity.Property(e => e.PdfSavedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("pdf_saved_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SignedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("signed_at");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.ParkingZone)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.ParkingZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__contract__parkin__36B12243");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__contract__status__398D8EEE");
            });

            modelBuilder.Entity<ContractLog>(entity =>
            {
                entity.ToTable("contract_log");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractLogs)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK__contract___contr__3D5E1FD2");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ContractLogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__contract___creat__66603565");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.ContractLogs)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__contract___type___3F466844");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("district");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_city_district");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__district__create__656C112C");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("feedback");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ParkingZoneId).HasColumnName("parking_zone_id");

                entity.Property(e => e.Rate)
                    .HasColumnName("rate")
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.ParkingZone)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.ParkingZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__feedback__parkin__5FB337D6");
            });

            modelBuilder.Entity<GlobalStatus>(entity =>
            {
                entity.ToTable("global_status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.GlobalStatuses)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__global_st__type___32E0915F");
            });

            modelBuilder.Entity<ParkingTransaction>(entity =>
            {
                entity.ToTable("parking_transaction");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CheckinAt)
                    .HasColumnType("datetime")
                    .HasColumnName("checkin_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CheckinBy).HasColumnName("checkin_by");

                entity.Property(e => e.CheckoutAt)
                    .HasColumnType("datetime")
                    .HasColumnName("checkout_at");

                entity.Property(e => e.CheckoutBy).HasColumnName("checkout_by");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LicensePlate)
                    .HasMaxLength(20)
                    .HasColumnName("license_plate");

                entity.Property(e => e.PaidTransactionId)
                    .HasMaxLength(100)
                    .HasColumnName("paid_transaction_id");

                entity.Property(e => e.ParkingZoneId).HasColumnName("parking_zone_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.CheckinByNavigation)
                    .WithMany(p => p.ParkingTransactionCheckinByNavigations)
                    .HasForeignKey(d => d.CheckinBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_t__check__5629CD9C");

                entity.HasOne(d => d.CheckoutByNavigation)
                    .WithMany(p => p.ParkingTransactionCheckoutByNavigations)
                    .HasForeignKey(d => d.CheckoutBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_t__check__571DF1D5");

                entity.HasOne(d => d.ParkingZone)
                    .WithMany(p => p.ParkingTransactions)
                    .HasForeignKey(d => d.ParkingZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_t__parki__534D60F1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ParkingTransactions)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__parking_t__statu__5812160E");
            });

            modelBuilder.Entity<ParkingTransactionDetail>(entity =>
            {
                entity.ToTable("parking_transaction_detail");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Detail)
                    .HasMaxLength(255)
                    .HasColumnName("detail");

                entity.Property(e => e.From)
                    .HasColumnType("datetime")
                    .HasColumnName("from");

                entity.Property(e => e.ParkingTransactionId).HasColumnName("parking_transaction_id");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.Property(e => e.To)
                    .HasColumnType("datetime")
                    .HasColumnName("to");

                entity.Property(e => e.UnitPricePerHour)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("unit_price_per_hour");

                entity.HasOne(d => d.ParkingTransaction)
                    .WithMany(p => p.ParkingTransactionDetails)
                    .HasForeignKey(d => d.ParkingTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_t__parki__6B24EA82");
            });

            modelBuilder.Entity<ParkingZone>(entity =>
            {
                entity.ToTable("parking_zone");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CommuneId).HasColumnName("commune_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DetailAddress)
                    .HasMaxLength(255)
                    .HasColumnName("detail_address");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.PriceOverTimePerHour)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price_over_time_per_hour");

                entity.Property(e => e.PricePerHour)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price_per_hour");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.Commune)
                    .WithMany(p => p.ParkingZones)
                    .HasForeignKey(d => d.CommuneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_z__commu__403A8C7D");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.ParkingZones)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_z__owner__75A278F5");
            });

            modelBuilder.Entity<ParkingZoneAbsent>(entity =>
            {
                entity.ToTable("parking_zone_absent");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.From)
                    .HasColumnType("datetime")
                    .HasColumnName("from")
                    .HasDefaultValueSql("(datepart(day,getdate()))");

                entity.Property(e => e.ParkingZoneId).HasColumnName("parking_zone_id");

                entity.Property(e => e.Reason)
                    .HasColumnType("ntext")
                    .HasColumnName("reason");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.Property(e => e.To)
                    .HasColumnType("datetime")
                    .HasColumnName("to");

                entity.HasOne(d => d.ParkingZone)
                    .WithMany(p => p.ParkingZoneAbsents)
                    .HasForeignKey(d => d.ParkingZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_z__parki__4316F928");
            });

            modelBuilder.Entity<ParkingZoneAttendant>(entity =>
            {
                entity.ToTable("parking_zone_attendant");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ParkingZoneId).HasColumnName("parking_zone_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ParkingZoneAttendant)
                    .HasForeignKey<ParkingZoneAttendant>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_zone__id__02FC7413");

                entity.HasOne(d => d.ParkingZone)
                    .WithMany(p => p.ParkingZoneAttendants)
                    .HasForeignKey(d => d.ParkingZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_z__parki__03F0984C");
            });

            modelBuilder.Entity<ParkingZoneOwner>(entity =>
            {
                entity.ToTable("parking_zone_owner");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CommuneId).HasColumnName("commune_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DetailAddress)
                    .HasColumnType("ntext")
                    .HasColumnName("detail_address");

                entity.Property(e => e.IsApproved).HasColumnName("is_approved");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RejectedReason)
                    .HasColumnType("ntext")
                    .HasColumnName("rejected_reason");

                entity.HasOne(d => d.Commune)
                    .WithMany(p => p.ParkingZoneOwners)
                    .HasForeignKey(d => d.CommuneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_z__commu__71D1E811");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ParkingZoneOwner)
                    .HasForeignKey<ParkingZoneOwner>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_zone__id__70DDC3D8");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("report");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__report__created___5CD6CB2B");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
