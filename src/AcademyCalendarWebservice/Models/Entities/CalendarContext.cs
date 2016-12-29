using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AcademyCalendarWebservice.Models.Entities
{
    public partial class CalendarContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=Frank;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Decription).HasColumnType("varchar(max)");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.OccupantId).HasColumnName("Occupant_Id");

                entity.Property(e => e.RoomId).HasColumnName("Room_Id");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.HasOne(d => d.Occupant)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.OccupantId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BookingOccupant");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BookingRoom");
            });

            modelBuilder.Entity<Occupant>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasColumnType("varchar(64)");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Room__737584F6A68708F4")
                    .IsUnique();

                entity.Property(e => e.HasProjector).HasColumnName("Has_Projector");

                entity.Property(e => e.HasTvScreen).HasColumnName("Has_TvScreen");

                entity.Property(e => e.HasWhiteBoard).HasColumnName("Has_WhiteBoard");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(64)");
            });
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Occupant> Occupant { get; set; }
        public virtual DbSet<Room> Room { get; set; }
    }
}