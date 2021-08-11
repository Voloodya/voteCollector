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
    [Table("organization")]
    public partial class Organization
    {
        public Organization()
        {
            Friends = new HashSet<Friend>();
            Groupus = new HashSet<Groupu>();
        }

        [Key]
        [Column("Id_Organization")]
        public int IdOrganization { get; set; }
        [DisplayName("Подведомственные учреждения")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }

        [DisplayName("Подведомственные учреждения")]
        [InverseProperty("Organization_")]
        public virtual ICollection<Friend> Friends { get; set; }
        [DisplayName("Организация")]
        [InverseProperty("Organization")]
        public virtual ICollection<Groupu> Groupus { get; set; }
    }
}
