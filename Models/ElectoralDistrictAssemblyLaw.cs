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
    [Table("electoral_district_assembly_law")]
    public partial class ElectoralDistrictAssemblyLaw
    {
        public ElectoralDistrictAssemblyLaw()
        {
            Districts = new HashSet<District>();
        }

        [Key]
        [Column("Id_electoral_district_assembly_law")]
        public int IdElectoralDistrictAssemblyLaw { get; set; }
        [DisplayName("Округ зак. собрания")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }

        [DisplayName("Округ зак. собрания")]
        [InverseProperty("ElectoralDistrictAssemblyLaw")]
        public virtual ICollection<District> Districts { get; set; }
    }
}
