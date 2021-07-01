using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("groupsusers")]
    public partial class Groupsusers
    {
        [Key]
        [Column("Id_GroupsUsers")]
        public long IdGroupsUsers { get; set; }
        [Column("GroupU_id")]
        [DisplayName("Группа")]
        public int? GroupUId { get; set; }
        [DisplayName("Группа")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [DisplayName("Пользователь")]
        [Column("User_id")]
        public long? UserId { get; set; }

        [DisplayName("Группа")]
        [ForeignKey(nameof(GroupUId))]
        [InverseProperty(nameof(Groupu.Groupsusers))]
        public virtual Groupu GroupU { get; set; }
        [DisplayName("Пользователь")]
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Groupsusers")]
        public virtual User User { get; set; }
    }
}
