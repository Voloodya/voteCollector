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
    [Table("fieldactivity")]
    public partial class Fieldactivity
    {
        public Fieldactivity()
        {
            Friends = new HashSet<Friend>();
            Groupus = new HashSet<Groupu>();
        }

        [Key]
        [Column("Id_FieldActivity")]
        public int IdFieldActivity { get; set; }
        [DisplayName("Организация")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }

        [InverseProperty("FieldActivity")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("FieldActivity")]
        [DisplayName("Организация")]
        public virtual ICollection<Groupu> Groupus { get; set; }
    }
}
