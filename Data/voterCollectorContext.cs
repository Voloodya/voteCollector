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
        public virtual DbSet<CityDistrict> CityDistrict { get; set; }
        public virtual DbSet<ElectoralDistrict> ElectoralDistrict { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<ElectoralDistrictAssemblyLaw> ElectoralDistrictAssemblyLaw { get; set; }
        public virtual DbSet<ElectoralDistrictGovDuma> ElectoralDistrictGovDuma { get; set; }
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
        public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<FriendStatus> FriendStatus { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=root;database=voterCollector; default command timeout=30", x => x.ServerVersion("8.0.22-mysql"));
                //optionsBuilder.UseMySql("server=10.15.15.40;port=3306;user=volodya;password=volodyaroot;database=voterCollector; default command timeout=30", x => x.ServerVersion("8.0.22-mysql"));
                //optionsBuilder.UseMySql("server=localhost;port=3306;user=u1451597_root;password=@volodyaadmin001;database=u1451597_voterCollector; default command timeout=30", x => x.ServerVersion("10.5.10-MariaDB-log"));
                //optionsBuilder.UseMySql("server=195.226.209.40;port=3306;user=volodya;password=volodyaroot;database=voterCollector; default command timeout=30", x => x.ServerVersion("8.0.22-mysql"));
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

            modelBuilder.Entity<CityDistrict>(entity =>
            {
                entity.HasKey(e => e.IdCityDistrict)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_CityDistrict_City");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CityDistricts)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CityDistrict_City");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.IdDistrict)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ElectoralDistrictId)
                    .HasName("FK_District_Electoral_District");

                entity.HasIndex(e => e.ElectoralDistrictAssemblyLawId)
    .HasName("FK_District_ElectoralDistrictAssemblyLaw");

                entity.HasIndex(e => e.ElectoralDistrictGovDumaId)
                    .HasName("FK_District_ElectoralDistrictGovDuma");

                entity.HasIndex(e => e.StationId)
                    .HasName("FK_District_Station");

                entity.HasIndex(e => e.CityId)
                    .HasName("FK_District_City");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.ElectoralDistrict)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ElectoralDistrictId)
                    .HasConstraintName("FK_District_Electoral_District");

                entity.HasOne(d => d.ElectoralDistrictAssemblyLaw)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ElectoralDistrictAssemblyLawId)
                    .HasConstraintName("FK_District_ElectoralDistrictAssemblyLaw");

                entity.HasOne(d => d.ElectoralDistrictGovDuma)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ElectoralDistrictGovDumaId)
                    .HasConstraintName("FK_District_ElectoralDistrictGovDuma");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_District_Station");

                entity.HasOne(d => d.CityDistrict)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_District_City");

            });

            modelBuilder.Entity<ElectoralDistrict>(entity =>
            {
                entity.HasKey(e => e.IdElectoralDistrict)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
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

                entity.HasIndex(e => e.CityDistrictId)
                    .HasName("FK_Friend_CityDistrict");

                entity.HasIndex(e => e.ElectoralDistrictId)
                    .HasName("FK_Friend_ElectoralDistrict");

                entity.HasIndex(e => e.FieldActivityId)
                    .HasName("FK_Friend_FieldActivity");

                entity.HasIndex(e => e.GroupUId)
                    .HasName("FK_Friend_GroupU");

                entity.HasIndex(e => e.HouseId)
                    .HasName("FK_Friend_House");

                entity.HasIndex(e => e.MicroDistrictId)
                    .HasName("FK_Friend_MicroDistrict");

                entity.HasIndex(e => e.OrganizationId)
                   .HasName("FK_Friend_Organization");

                entity.HasIndex(e => e.StationId)
                   .HasName("FK_Friend_Station");

                entity.HasIndex(e => e.StreetId)
                    .HasName("FK_Friend_Street");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_Friend_User");

                entity.HasIndex(e => e.FriendStatusId)
                    .HasName("FK_Friend_FriendStatus");

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

                entity.Property(e => e.userNameMessanger)
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

                entity.Property(e => e.TypeImage)
                   .HasCharSet("utf8mb4")
                   .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Voter).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Friend_CityDistrict");

                entity.HasOne(d => d.CityDistrict)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.CityDistrictId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_City");

                entity.HasOne(d => d.ElectoralDistrict)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.ElectoralDistrictId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_ElectoralDistrict");

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

                entity.HasOne(d => d.Organization_)
                   .WithMany(p => p.Friends)
                   .HasForeignKey(d => d.OrganizationId)
                   .HasConstraintName("FK_Friend_Organization");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Friend_Station");

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

                entity.HasOne(d => d.FriendStatus)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.FriendStatusId)
                    .HasConstraintName("FK_Friend_FriendStatus");
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

                entity.HasIndex(e => e.FieldActivityId)
                    .HasName("FK_Groupu_Fieldactivity");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("FK_Groupu_Organization");

                entity.HasIndex(e => e.GroupParentsId)
                    .HasName("FK_Groupu_Groupu");

                entity.HasIndex(e => e.UserResponsibleId)
                    .HasName("FK_Groupu_User");

                entity.Property(e => e.CreatorGroup)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.FieldActivity)
                   .WithMany(p => p.Groupus)
                   .HasForeignKey(d => d.FieldActivityId)
                   .HasConstraintName("FK_Groupu_Fieldactivity");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Groupus)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_Groupu_Organization");

                entity.HasOne(d => d.GroupParents)
                    .WithMany(p => p.InverseGroupParents)
                    .HasForeignKey(d => d.GroupParentsId)
                    .HasConstraintName("FK_Groupu_Groupu");

                entity.HasOne(d => d.UserResponsible)
                    .WithMany(p => p.Groupus)
                    .HasForeignKey(d => d.UserResponsibleId)
                    .HasConstraintName("FK_Groupu_User");
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.HasKey(e => e.IdHouse)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CityId)
                   .HasName("FK_House_City_idx");

                entity.HasIndex(e => e.MicroDistrictId)
                    .HasName("FK_House_MicroDistrict");

                entity.HasIndex(e => e.StreetId)
                    .HasName("FK_House_Street");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.CityDistrict)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_House_City");

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

                entity.HasOne(d => d.CityDistrict)
                    .WithMany(p => p.Microdistricts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MicroDistrict_City");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(e => e.IdStation)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<PollingStation>(entity =>
            {
                entity.HasKey(e => e.IdPollingStation)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.StationId)
                    .HasName("FK_PollingStation_Station");

                entity.HasIndex(e => e.CityDistrictId)
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

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.PollingStations)
                    .HasForeignKey(d => d.StationId)
                    .HasConstraintName("FK_PollingStation_Station");

                entity.HasOne(d => d.CityDistrict)
                    .WithMany(p => p.PollingStations)
                    .HasForeignKey(d => d.CityDistrictId)
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

                entity.HasOne(d => d.CityDistrict)
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

            modelBuilder.Entity<FriendStatus>(entity =>
            {
                entity.HasKey(e => e.IdFriendStatus)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.IdOrganization)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public DbSet<voteCollector.Models.Report> Report { get; set; }
        public DbSet<voteCollector.Models.ReportDistrict> ReportDistrict { get; set; }
        public DbSet<voteCollector.Models.ReportCity> ReportCity { get; set; }
    }
}
