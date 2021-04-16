using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("user")]
    public partial class User
    {
        public User()
        {
            Friends = new HashSet<Friend>();
            Groupsusers = new HashSet<Groupsusers>();
        }

        [Key]
        [Column("Id_User")]
        public long IdUser { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string UserName { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }
        [Column("Role_id")]
        public int? RoleId { get; set; }
        [Column("Family_name", TypeName = "varchar(256)")]
        public string FamilyName { get; set; }
        [Column("Name_", TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("Patronymic_name", TypeName = "varchar(256)")]
        public string PatronymicName { get; set; }
        [Column("Date_birth", TypeName = "date")]
        public DateTime? DateBirth { get; set; }
        [Column(TypeName = "varchar(12)")]
        public string Telephone { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Groupsusers> Groupsusers { get; set; }
    }
}
