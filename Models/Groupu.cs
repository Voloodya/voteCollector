using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("groupu")]
    public partial class Groupu
    {
        public Groupu()
        {
            Friends = new HashSet<Friend>();
            Groupsusers = new HashSet<Groupsusers>();
        }

        [Key]
        [Column("Id_Group")]
        public int IdGroup { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }

        [InverseProperty("GroupU")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("GroupU")]
        public virtual ICollection<Groupsusers> Groupsusers { get; set; }

    }
}
