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
    [Table("city_district")]
    public partial class CityDistrict
    {
        public CityDistrict()
        {
            Districts = new HashSet<District>();
            Friends = new HashSet<Friend>();
            Microdistricts = new HashSet<Microdistrict>();
            PollingStations = new HashSet<PollingStation>();
            Streets = new HashSet<Street>();
            Houses = new HashSet<House>();
        }

        [Key]
        [Column("Id_City")]
        public int IdCity { get; set; }
        [DisplayName("Городской огруг")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }

        [InverseProperty("CityDistrict")]
        public virtual ICollection<District> Districts { get; set; }
        [InverseProperty("CityDistrict")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("CityDistrict")]
        public virtual ICollection<House> Houses { get; set; }
        [InverseProperty("CityDistrict")]
        public virtual ICollection<Microdistrict> Microdistricts { get; set; }
        [InverseProperty("CityDistrict")]
        public virtual ICollection<PollingStation> PollingStations { get; set; }
        [InverseProperty("CityDistrict")]
        public virtual ICollection<Street> Streets { get; set; }
    }
}
