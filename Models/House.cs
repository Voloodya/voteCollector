using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("house")]
    public partial class House
    {
        public House()
        {
            Friends = new HashSet<Friend>();
            PollingStations = new HashSet<PollingStation>();
        }

        [Key]
        [Column("Id_House")]
        public int IdHouse { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("Street_id")]
        public int? StreetId { get; set; }
        [Column("MicroDistrict_id")]
        public int? MicroDistrictId { get; set; }

        [ForeignKey(nameof(MicroDistrictId))]
        [InverseProperty(nameof(Microdistrict.Houses))]
        public virtual Microdistrict MicroDistrict { get; set; }
        [ForeignKey(nameof(StreetId))]
        [InverseProperty("Houses")]
        public virtual Street Street { get; set; }
        [InverseProperty("House")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("House")]
        public virtual ICollection<PollingStation> PollingStations { get; set; }
    }
}
