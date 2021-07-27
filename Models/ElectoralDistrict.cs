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
    [Table("electoral_district")]
    public partial class ElectoralDistrict
    {
        public ElectoralDistrict()
        {
            Districts = new HashSet<District>();
        }

        [Key]
        [Column("Id_ElectoralDistrict")]
        public int IdElectoralDistrict { get; set; }
        [Column(TypeName = "varchar(256)")]
        [DisplayName("Избирательный округ")]
        public string Name { get; set; }

        [InverseProperty("ElectoralDistrict")]
        public virtual ICollection<District> Districts { get; set; }
    }
}
