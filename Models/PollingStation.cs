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
    [Table("polling_station")]
    public partial class PollingStation
    {

        [Key]
        [Column("Id_Polling_station")]
        public int IdPollingStation { get; set; }
        [DisplayName("Участок")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [DisplayName("Номер участка")]
        [Column("Station_id")]
        public int? StationId { get; set; }

        [DisplayName("Населен. п-т")]
        [Required(ErrorMessage = "Не указан населен. п-т")]
        [Column("City_id")]
        public int? CityId { get; set; }
        [DisplayName("Улица")]
        [Column("Street_id")]
        public int? StreetId { get; set; }
        [DisplayName("Микрорайон")]
        [Column("MicroDistrict_id")]
        public int? MicroDistrictId { get; set; }
        [DisplayName("Дом")]
        [Column("House_id")]
        public int? HouseId { get; set; }

        [ForeignKey(nameof(StationId))]
        [InverseProperty("PollingStations")]
        [DisplayName("Номер участка")]
        public virtual Station Station { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("PollingStations")]
        [DisplayName("Насел. п-т")]
        public virtual City City { get; set; }
        [ForeignKey(nameof(HouseId))]
        [InverseProperty("PollingStations")]
        [DisplayName("Дом")]
        public virtual House House { get; set; }
        [ForeignKey(nameof(MicroDistrictId))]
        [InverseProperty(nameof(Microdistrict.PollingStations))]
        [DisplayName("Микро р-н")]
        public virtual Microdistrict MicroDistrict { get; set; }
        [ForeignKey(nameof(StreetId))]
        [InverseProperty("PollingStations")]
        [DisplayName("Улица")]
        public virtual Street Street { get; set; }
    }
}
