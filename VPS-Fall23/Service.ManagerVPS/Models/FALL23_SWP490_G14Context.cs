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
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server = 210.211.127.85,6666; database = FALL23_SWP490_G14; uid=nghianvho; pwd=Random@11092023#@!;");
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

                entity.Property(e => e.ExpireVerifyCode).HasColumnType("datetime");

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
                    .HasMaxLength(255)
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
                    .HasConstraintName("FK__contract__parkin__0E6E26BF");

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
                    .HasConstraintName("FK__feedback__parkin__0B91BA14");
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

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.LicensePlate)
                    .HasMaxLength(20)
                    .HasColumnName("license_plate");

                entity.Property(e => e.ParkingZoneId).HasColumnName("parking_zone_id");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasColumnName("phone");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(d => d.CheckinByNavigation)
                    .WithMany(p => p.ParkingTransactionCheckinByNavigations)
                    .HasForeignKey(d => d.CheckinBy)
                    .HasConstraintName("FK__parking_t__check__5629CD9C");

                entity.HasOne(d => d.CheckoutByNavigation)
                    .WithMany(p => p.ParkingTransactionCheckoutByNavigations)
                    .HasForeignKey(d => d.CheckoutBy)
                    .HasConstraintName("FK__parking_t__check__571DF1D5");

                entity.HasOne(d => d.ParkingZone)
                    .WithMany(p => p.ParkingTransactions)
                    .HasForeignKey(d => d.ParkingZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_t__parki__0F624AF8");

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

                entity.Property(e => e.IsApprove).HasColumnName("is_approve");

                entity.Property(e => e.IsFull)
                    .HasColumnName("is_full")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lat)
                    .HasColumnType("decimal(18, 10)")
                    .HasColumnName("lat");

                entity.Property(e => e.Lng)
                    .HasColumnType("decimal(18, 10)")
                    .HasColumnName("lng");

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

                entity.Property(e => e.RejectReason)
                    .HasColumnType("ntext")
                    .HasColumnName("reject_reason");

                entity.Property(e => e.Slots).HasColumnName("slots");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.Property(e => e.WorkFrom)
                    .HasColumnName("work_from")
                    .HasDefaultValueSql("('06:00:00')");

                entity.Property(e => e.WorkTo)
                    .HasColumnName("work_to")
                    .HasDefaultValueSql("('23:00:00')");

                entity.HasOne(d => d.Commune)
                    .WithMany(p => p.ParkingZones)
                    .HasForeignKey(d => d.CommuneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_z__commu__10566F31");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.ParkingZones)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_z__owner__0D7A0286");
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
                    .HasConstraintName("FK__parking_z__parki__0C85DE4D");
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
                    .HasConstraintName("FK__parking_z__parki__114A936A");
            });

            modelBuilder.Entity<ParkingZoneOwner>(entity =>
            {
                entity.ToTable("parking_zone_owner");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ParkingZoneOwner)
                    .HasForeignKey<ParkingZoneOwner>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parking_zone__id__70DDC3D8");
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasKey(e => e.TxnRef)
                    .HasName("PK__payment___B914BF1A850DE58C");

                entity.ToTable("payment_transaction");

                entity.Property(e => e.TxnRef)
                    .HasMaxLength(100)
                    .HasColumnName("txn_ref");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("amount");

                entity.Property(e => e.BankCode)
                    .HasMaxLength(20)
                    .HasColumnName("bank_code");

                entity.Property(e => e.BankTranNo)
                    .HasMaxLength(255)
                    .HasColumnName("bank_tran_no");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.CardType)
                    .HasMaxLength(20)
                    .HasColumnName("card_type");

                entity.Property(e => e.ConnectionId)
                    .HasMaxLength(255)
                    .HasColumnName("connection_id");

                entity.Property(e => e.OrderInfo)
                    .HasMaxLength(255)
                    .HasColumnName("order_info");

                entity.Property(e => e.PayDate)
                    .HasColumnType("datetime")
                    .HasColumnName("pay_date");

                entity.Property(e => e.ResponseCode).HasColumnName("response_code");

                entity.Property(e => e.SecureHash)
                    .HasMaxLength(256)
                    .HasColumnName("secure_hash");

                entity.Property(e => e.SecureHashType)
                    .HasMaxLength(10)
                    .HasColumnName("secure_hash_type");

                entity.Property(e => e.TransactionNo).HasColumnName("transaction_no");

                entity.Property(e => e.TransactionStatus).HasColumnName("transaction_status");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.PaymentTransactions)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__payment_t__booki__1F98B2C1");
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

                entity.Property(e => e.Email)
                    .HasMaxLength(1)
                    .HasColumnName("email");

                entity.Property(e => e.Phone)
                    .HasMaxLength(1)
                    .HasColumnName("phone");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SubId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sub_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__report__created___5CD6CB2B");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.ReportStatusNavigations)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_report_global_status1");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.ReportTypeNavigations)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_report_global_status");
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
