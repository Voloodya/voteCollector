using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("microdistrict")]
    public partial class Microdistrict
    {
        public Microdistrict()
        {
            Friends = new HashSet<Friend>();
            Houses = new HashSet<House>();
            PollingStations = new HashSet<PollingStation>();
        }

        [Key]
        [Column("Id_MicroDistrict")]
        public int IdMicroDistrict { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("City_id")]
        public int? CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Microdistricts")]
        public virtual City City { get; set; }
        [InverseProperty("MicroDistrict")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("MicroDistrict")]
        public virtual ICollection<House> Houses { get; set; }
        [InverseProperty("MicroDistrict")]
        public virtual ICollection<PollingStation> PollingStations { get; set; }
    }
}
