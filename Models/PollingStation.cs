using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("polling_station")]
    public partial class PollingStation
    {
        public PollingStation()
        {
            Friends = new HashSet<Friend>();
        }

        [Key]
        [Column("Id_Polling_station")]
        public int IdPollingStation { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("City_id")]
        public int? CityId { get; set; }
        [Column("Street_id")]
        public int? StreetId { get; set; }
        [Column("MicroDistrict_id")]
        public int? MicroDistrictId { get; set; }
        [Column("House_id")]
        public int? HouseId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("PollingStations")]
        public virtual City City { get; set; }
        [ForeignKey(nameof(HouseId))]
        [InverseProperty("PollingStations")]
        public virtual House House { get; set; }
        [ForeignKey(nameof(MicroDistrictId))]
        [InverseProperty(nameof(Microdistrict.PollingStations))]
        public virtual Microdistrict MicroDistrict { get; set; }
        [ForeignKey(nameof(StreetId))]
        [InverseProperty("PollingStations")]
        public virtual Street Street { get; set; }
        [InverseProperty("PollingStation")]
        public virtual ICollection<Friend> Friends { get; set; }
    }
}
