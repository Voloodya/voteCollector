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
        [Required(ErrorMessage = "Не указан номер дома")]
        [DisplayName("Номер дома")]
        public string Name { get; set; }
        [Column("Street_id")]
        [Required(ErrorMessage = "Не указана улица")]
        [DisplayName("Улица")]
        public int? StreetId { get; set; }
        [Column("MicroDistrict_id")]
        [DisplayName("Микрорайон")]
        public int? MicroDistrictId { get; set; }
        [Column("City_id")]
        [Required(ErrorMessage = "Не указан населен. п-т")]
        [DisplayName("Населенный пункт")]
        public int? CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Houses")]
        public virtual City City { get; set; }

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
