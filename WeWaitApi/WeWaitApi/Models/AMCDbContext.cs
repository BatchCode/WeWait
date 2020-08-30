using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WeWaitApi.Models
{
    public partial class AMCDbContext : DbContext
    {
        public AMCDbContext()
        {
        }

        public AMCDbContext(DbContextOptions<AMCDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Confirmbooking> Confirmbooking { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Eventbooking> Eventbooking { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Satehistory> Satehistory { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=wewait;database=wewait", x => x.ServerVersion("10.4.13-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");

                entity.HasIndex(e => e.ConfirmBookingId)
                    .HasName("fk_Booking_ConfirmBooking1_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_Booking_User1_idx");

                entity.HasIndex(e => e.WeWaiterId)
                    .HasName("fk_Booking_User2_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ConfirmBookingId)
                    .HasColumnName("ConfirmBooking_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WeWaiterId)
                    .HasColumnName("WeWaiter_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ConfirmBooking)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.ConfirmBookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Booking_ConfirmBooking1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookingUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Booking_User1");

                entity.HasOne(d => d.WeWaiter)
                    .WithMany(p => p.BookingWeWaiter)
                    .HasForeignKey(d => d.WeWaiterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Booking_User2");
            });

            modelBuilder.Entity<Confirmbooking>(entity =>
            {
                entity.ToTable("confirmbooking");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BookingCode)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Confirm).HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.HasIndex(e => e.LocationId)
                    .HasName("fk_Event_Location1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Actor)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DateEnd)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.LocationId)
                    .HasColumnName("Location_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Seats)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'99'");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Event_Location1");
            });

            modelBuilder.Entity<Eventbooking>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.BookingId })
                    .HasName("PRIMARY");

                entity.ToTable("eventbooking");

                entity.HasIndex(e => e.BookingId)
                    .HasName("fk_Event_has_Booking_Booking1_idx");

                entity.HasIndex(e => e.EventId)
                    .HasName("fk_Event_has_Booking_Event1_idx");

                entity.Property(e => e.EventId)
                    .HasColumnName("Event_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BookingId)
                    .HasColumnName("Booking_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Eventbooking)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Event_has_Booking_Booking1");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Eventbooking)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Event_has_Booking_Event1");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Adress1)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Adress2)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.District)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasIndex(e => e.Label)
                    .HasName("NameRole_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Satehistory>(entity =>
            {
                entity.HasKey(e => new { e.StateId, e.BookingId })
                    .HasName("PRIMARY");

                entity.ToTable("satehistory");

                entity.HasIndex(e => e.BookingId)
                    .HasName("fk_State_has_Booking_Booking1_idx");

                entity.HasIndex(e => e.StateId)
                    .HasName("fk_State_has_Booking_State1_idx");

                entity.Property(e => e.StateId)
                    .HasColumnName("State_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BookingId)
                    .HasColumnName("Booking_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Satehistory)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_State_has_Booking_Booking1");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Satehistory)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_State_has_Booking_State1");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("state");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.LocationId)
                    .HasName("fk_User_Location1_idx");

                entity.HasIndex(e => e.RoleId)
                    .HasName("fk_User_Role1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Age).HasColumnType("int(11)");

                entity.Property(e => e.CompteBancaire)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(80)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.LocationId)
                    .HasColumnName("Location_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.RoleId)
                    .HasColumnName("Role_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_User_Location1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_User_Role1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
