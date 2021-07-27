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
        public District()
        {
            Friends = new HashSet<Friend>();
        }

        [Key]
        [Column("Id_District")]
        public int IdDistrict { get; set; }
        [DisplayName("Округ")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("Electoral_District_id")]
        public int? ElectoralDistrictId { get; set; }
        [Column("Station_id")]
        public int? StationId { get; set; }
        [Column("City_id")]
        public int? CityId { get; set; }
        [Column("Street_id")]
        public int? StreetId { get; set; }

        [ForeignKey(nameof(ElectoralDistrictId))]
        [InverseProperty("Districts")]
        public virtual ElectoralDistrict ElectoralDistrict { get; set; }
        [ForeignKey(nameof(StationId))]
        [InverseProperty("Districts")]
        public virtual Station Station { get; set; }
        [ForeignKey(nameof(CityId))]
        [InverseProperty("Districts")]
        public virtual City City { get; set; }
        [InverseProperty("Districts")]
        public virtual Street Street { get; set; }

        [InverseProperty("District")]
        public virtual ICollection<Friend> Friends { get; set; }
    }
}
