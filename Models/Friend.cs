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
    [Table("friend")]
    public partial class Friend
    {
        [Key]
        [Column("Id_Friend")]
        public long IdFriend { get; set; }
        [Column("Family_name", TypeName = "varchar(256)")]
        public string FamilyName { get; set; }
        [Column("Name_", TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("Patronymic_name", TypeName = "varchar(256)")]
        public string PatronymicName { get; set; }
        [Column("Date_birth", TypeName = "date")]
        public DateTime? DateBirth { get; set; }
        [Column("City_id")]
        public int? CityId { get; set; }
        [Column("District_id")]
        public int? DistrictId { get; set; }
        [Column("Street_id")]
        public int? StreetId { get; set; }
        [Column("MicroDistrict_id")]
        public int? MicroDistrictId { get; set; }
        [Column("House_id")]
        public int? HouseId { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Building { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Apartment { get; set; }
        [Column(TypeName = "varchar(12)")]
        public string Telephone { get; set; }
        [Column("Polling_station_id")]
        public int? PollingStationId { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Organization { get; set; }
        [Column("FieldActivity_id")]
        public int? FieldActivityId { get; set; }
        [Column("Phone_number_responsible", TypeName = "varchar(12)")]
        public string PhoneNumberResponsible { get; set; }
        [Column("Date_registration_site", TypeName = "date")]
        public DateTime? DateRegistrationSite { get; set; }
        [Column("Voting_date", TypeName = "date")]
        public DateTime? VotingDate { get; set; }
        [DisplayName("Проголос-л")]
        [Column("Voter", TypeName = "TINYINT")]
        public bool Voter { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Adress { get; set; }
        [Column("QRcode", TypeName = "varchar(4500)")]
        public string Qrcode { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Description { get; set; }
        [Column("User_id")]
        public long? UserId { get; set; }
        [Column("GroupU_id")]
        public int? GroupUId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Friends")]
        public virtual City City { get; set; }
        [ForeignKey(nameof(DistrictId))]
        [InverseProperty("Friends")]
        public virtual District District { get; set; }
        [ForeignKey(nameof(FieldActivityId))]
        [InverseProperty(nameof(Fieldactivity.Friends))]
        public virtual Fieldactivity FieldActivity { get; set; }
        [ForeignKey(nameof(GroupUId))]
        [InverseProperty(nameof(Groupu.Friends))]
        public virtual Groupu GroupU { get; set; }
        [ForeignKey(nameof(HouseId))]
        [InverseProperty("Friends")]
        public virtual House House { get; set; }
        [ForeignKey(nameof(MicroDistrictId))]
        [InverseProperty(nameof(Microdistrict.Friends))]
        public virtual Microdistrict MicroDistrict { get; set; }
        [ForeignKey(nameof(PollingStationId))]
        [InverseProperty("Friends")]
        public virtual PollingStation PollingStation { get; set; }
        [ForeignKey(nameof(StreetId))]
        [InverseProperty("Friends")]
        public virtual Street Street { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Friends")]
        public virtual User User { get; set; }
    }
}
