using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using voteCollector.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Data
{
    public partial class VoterCollectorContext : DbContext
    {
        public VoterCollectorContext()
        {
        }

        public VoterCollectorContext(DbContextOptions<VoterCollectorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Fieldactivity> Fieldactivity { get; set; }
        public virtual DbSet<Friend> Friend { get; set; }
        public virtual DbSet<Groupsusers> Groupsusers { get; set; }
        public virtual DbSet<Groupu> Groupu { get; set; }
        public virtual DbSet<House> House { get; set; }
        public virtual DbSet<Microdistrict> Microdistrict { get; set; }
        public virtual DbSet<PollingStation> PollingStation { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Street> Street { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=root;database=voterCollector", x => x.ServerVersion("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.IdCity)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.IdDistrict)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_District_City");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_District_City");
            });

            modelBuilder.Entity<Fieldactivity>(entity =>
            {
                entity.HasKey(e => e.IdFieldActivity)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(e => e.IdFriend)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_Friend_City");

                entity.HasIndex(e => e.DistrictId)
                    .HasName("FK_Friend_District");

                entity.HasIndex(e => e.FieldActivityId)
                    .HasName("FK_Friend_FieldActivity");

                entity.HasIndex(e => e.GroupUId)
                    .HasName("FK_Friend_GroupU");

                entity.HasIndex(e => e.HouseId)
                    .HasName("FK_Friend_House");

                entity.HasIndex(e => e.MicroDistrictId)
                    .HasName("FK_Friend_MicroDistrict");

                entity.HasIndex(e => e.PollingStationId)
                    .HasName("FK_Friend_Polling_station");

                entity.HasIndex(e => e.StreetId)
                    .HasName("FK_Friend_Street");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_Friend_User");

                entity.Property(e => e.Adress)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Apartment)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Building)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FamilyName)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Organization)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PatronymicName)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PhoneNumberResponsible)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Qrcode)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TextQRcode)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Email)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Telephone)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Voter).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_City");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_District");

                entity.HasOne(d => d.FieldActivity)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.FieldActivityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_FieldActivity");

                entity.HasOne(d => d.GroupU)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.GroupUId)
                    .HasConstraintName("FK_Friend_GroupU");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.HouseId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_House");

                entity.HasOne(d => d.MicroDistrict)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.MicroDistrictId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_MicroDistrict");

                entity.HasOne(d => d.PollingStation)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.PollingStationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_Polling_station");

                entity.HasOne(d => d.Street)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.StreetId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_Street");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_User");
            });

            modelBuilder.Entity<Groupsusers>(entity =>
            {
                entity.HasKey(e => e.IdGroupsUsers)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.GroupUId)
                    .HasName("FK_GroupsUsers_GroupU");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_GroupsUsers_User");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.GroupU)
                    .WithMany(p => p.Groupsusers)
                    .HasForeignKey(d => d.GroupUId)
                    .HasConstraintName("FK_GroupsUsers_GroupU");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Groupsusers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_GroupsUsers_User");
            });

            modelBuilder.Entity<Groupu>(entity =>
            {
                entity.HasKey(e => e.IdGroup)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.HasKey(e => e.IdHouse)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.MicroDistrictId)
                    .HasName("FK_House_MicroDistrict");

                entity.HasIndex(e => e.StreetId)
                    .HasName("FK_House_Street");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.MicroDistrict)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.MicroDistrictId)
                    .HasConstraintName("FK_House_MicroDistrict");

                entity.HasOne(d => d.Street)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.StreetId)
                    .HasConstraintName("FK_House_Street");
            });

            modelBuilder.Entity<Microdistrict>(entity =>
            {
                entity.HasKey(e => e.IdMicroDistrict)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_MicroDistrict_City");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Microdistricts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MicroDistrict_City");
            });

            modelBuilder.Entity<PollingStation>(entity =>
            {
                entity.HasKey(e => e.IdPollingStation)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_PollingStation_City");

                entity.HasIndex(e => e.HouseId)
                    .HasName("FK_PollingStation_House");

                entity.HasIndex(e => e.MicroDistrictId)
                    .HasName("FK_PollingStation_MicroDistrict");

                entity.HasIndex(e => e.StreetId)
                    .HasName("FK_PollingStation_Street");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.PollingStations)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PollingStation_City");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.PollingStations)
                    .HasForeignKey(d => d.HouseId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PollingStation_House");

                entity.HasOne(d => d.MicroDistrict)
                    .WithMany(p => p.PollingStations)
                    .HasForeignKey(d => d.MicroDistrictId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PollingStation_MicroDistrict");

                entity.HasOne(d => d.Street)
                    .WithMany(p => p.PollingStations)
                    .HasForeignKey(d => d.StreetId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PollingStation_Street");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.HasKey(e => e.IdStreet)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_Street_City");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Streets)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Street_City");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.RoleId)
                    .HasName("FK_User_Role");

                entity.Property(e => e.FamilyName)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Password)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PatronymicName)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Telephone)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserName)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
