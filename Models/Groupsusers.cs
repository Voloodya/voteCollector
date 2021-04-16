﻿using System;
using System.Collections.Generic;
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
        public int? GroupUId { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("User_id")]
        public long? UserId { get; set; }

        [ForeignKey(nameof(GroupUId))]
        [InverseProperty(nameof(Groupu.Groupsusers))]
        public virtual Groupu GroupU { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Groupsusers")]
        public virtual User User { get; set; }
    }
}
