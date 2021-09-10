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
    [Table("district")]
    public partial class District
    {

        [Key]
        [Column("Id_District")]
        public int IdDistrict { get; set; }
        [DisplayName("Округ")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("Electoral_District_id")]
        [DisplayName("Избирательный округ")]
        public int? ElectoralDistrictId { get; set; }
        [Column("Electoral_district_gov_duma_id")]
        [DisplayName("Округ гос. думы")]
        public int? ElectoralDistrictGovDumaId { get; set; }
        [Column("Electoral_district_assembly_law_id")]
        [DisplayName("Округ зак. собрания")]
        public int? ElectoralDistrictAssemblyLawId { get; set; }

        [Column("Station_id")]
        [DisplayName("Избират. участок")]
        public int? StationId { get; set; }
        [Column("City_id")]
        public int? CityId { get; set; }

        [ForeignKey(nameof(ElectoralDistrictId))]
        [InverseProperty("Districts")]
        [DisplayName("Избирательный округ")]
        public virtual ElectoralDistrict ElectoralDistrict { get; set; }
        [ForeignKey(nameof(ElectoralDistrictAssemblyLawId))]
        [InverseProperty("Districts")]
        [DisplayName("Округ зак. собрания")]
        public virtual ElectoralDistrictAssemblyLaw ElectoralDistrictAssemblyLaw { get; set; }
        [ForeignKey(nameof(ElectoralDistrictGovDumaId))]
        [InverseProperty("Districts")]
        [DisplayName("Округ гос. думы")]
        public virtual ElectoralDistrictGovDuma ElectoralDistrictGovDuma { get; set; }

        [ForeignKey(nameof(StationId))]
        [InverseProperty("Districts")]
        [DisplayName("Избират. участок")]
        public virtual Station Station { get; set; }
        [ForeignKey(nameof(CityId))]
        [InverseProperty("Districts")]
        public virtual CityDistrict CityDistrict { get; set; }
    }
}
