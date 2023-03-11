using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SE1611_Group_4_Final_Project.Models
{
    public partial class MotelManagementContext : DbContext
    {
        public MotelManagementContext()
        {
        }

        public MotelManagementContext(DbContextOptions<MotelManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookingRequest> BookingRequests { get; set; } = null!;
        public virtual DbSet<FurnitureStatus> FurnitureStatuses { get; set; } = null!;
        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<RoomFurniture> RoomFurnitures { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=210.211.127.85,6666;uid=motel;password=Motel@01032023!@#;database=MotelManagement");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingRequest>(entity =>
            {
                entity.HasKey(e => e.RoomId)
                    .HasName("PK__BookingR__3286393927F62C4F");

                entity.ToTable("BookingRequest");

                entity.Property(e => e.RoomId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IdentifyNumber).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.HasOne(d => d.Room)
                    .WithOne(p => p.BookingRequest)
                    .HasForeignKey<BookingRequest>(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookingRe__RoomI__3A81B327");
            });

            modelBuilder.Entity<FurnitureStatus>(entity =>
            {
                entity.ToTable("FurnitureStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.GrandTotal).HasColumnType("money");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.RoomName).HasMaxLength(20);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Invoice__Payment__286302EC");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Invoice__RoomID__29572725");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Invoice__TypeID__267ABA7A");
            });

            modelBuilder.Entity<InvoiceType>(entity =>
            {
                entity.ToTable("InvoiceType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HideDate).HasColumnType("datetime");

                entity.Property(e => e.IsUse).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(255);
            });

            modelBuilder.Entity<PaymentStatus>(entity =>
            {
                entity.ToTable("PaymentStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.IsAvailable).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasMany(d => d.Notifications)
                    .WithMany(p => p.Rooms)
                    .UsingEntity<Dictionary<string, object>>(
                        "RoomNotification",
                        l => l.HasOne<Notification>().WithMany().HasForeignKey("NotificationId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Room_Noti__Notif__2D27B809"),
                        r => r.HasOne<Room>().WithMany().HasForeignKey("RoomId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Room_Noti__RoomI__2C3393D0"),
                        j =>
                        {
                            j.HasKey("RoomId", "NotificationId").HasName("PK__Room_Not__008ACBFA9F27E913");

                            j.ToTable("Room_Notification");

                            j.IndexerProperty<Guid>("RoomId").HasColumnName("RoomID");

                            j.IndexerProperty<Guid>("NotificationId").HasColumnName("NotificationID");
                        });
            });

            modelBuilder.Entity<RoomFurniture>(entity =>
            {
                entity.ToTable("RoomFurniture");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomFurnitures)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__RoomFurni__RoomI__1B0907CE");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.RoomFurnitures)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__RoomFurni__Statu__1A14E395");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.IdentifyNumber, "UQ__User__A1E7BE20F9180BA5")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.From).HasColumnType("datetime");

                entity.Property(e => e.IdentifyNumber).HasMaxLength(20);

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.To).HasColumnType("datetime");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__User__RoomID__21B6055D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
