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
        [Required(ErrorMessage = "Не указана фамилия")]
        [DisplayName("Фамилия")]
        [Column("Family_name", TypeName = "varchar(256)")]
        public string FamilyName { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        [DisplayName("Имя")]
        [Column("Name_", TypeName = "varchar(256)")]
        public string Name { get; set; }
        [DisplayName("Отчество")]
        [Column("Patronymic_name", TypeName = "varchar(256)")]
        public string PatronymicName { get; set; }
        [DisplayName("Дата рожд.")]
        [Required(ErrorMessage = "Не указана дата рождения")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column("Date_birth", TypeName = "date")]
        public DateTime? DateBirth { get; set; }
        [DisplayName("Откреплен")]
        [Column("Unpinning", TypeName = "TINYINT")]
        public bool Unpinning { get; set; }
        //[Required(ErrorMessage = "Не указан город")]
        [Column("City_id")]
        [DisplayName("Город")]
        public int? CityId { get; set; }
        //[Required(ErrorMessage = "Городской округ")]
        [DisplayName("Городской округ")]
        [Column("CityDistrict_id")]
        public int? CityDistrictId { get; set; }
        [DisplayName("Округ")]
        [Column("Electoral_district_id")]
        public int? ElectoralDistrictId { get; set; }
        //[Required(ErrorMessage = "Не указана улица")]
        [DisplayName("Улица/просп./пл./шоссе")]
        [Column("Street_id")]
        public int? StreetId { get; set; }
        [DisplayName("Микрорайон")]
        [Column("MicroDistrict_id")]
        public int? MicroDistrictId { get; set; }
        //[Required(ErrorMessage = "Не указан дом")]
        [DisplayName("Дом")]
        [Column("House_id")]
        public int? HouseId { get; set; }
        [DisplayName("Строение")]
        [Column(TypeName = "varchar(10)")]
        public string Building { get; set; }
        [DisplayName("Квартира")]
        [Column(TypeName = "varchar(10)")]
        public string Apartment { get; set; }
        [MinLength(11)]
        [MaxLength(12)]
        [RegularExpression(@"(^[+]{0,1}[0-9]{11})"), StringLength(12)]
        [DisplayName("Тел. участника")]
        [Column(TypeName = "varchar(12)")]
        [Required(ErrorMessage = "Не указан телефон участника")]
        public string Telephone { get; set; }
        //[Required(ErrorMessage = "Не указан участок")]
        [Column("Station_id")]
        [DisplayName("Участок")]
        public int? StationId { get; set; }
        [DisplayName("Организация")]
        [Column(TypeName = "varchar(256)")]
        public string Organization { get; set; }
        [DisplayName("Подразделение")]
        [Column("Organization_id")]
        public int? OrganizationId { get; set; }
        [Required(ErrorMessage = "Не указана группа (подведомств. учреждение!")]
        [DisplayName("Подведом. учрежд.")]
        [Column("GroupU_id")]
        public int? GroupUId { get; set; }

        [DisplayName("Организация")]
        [Column("FieldActivity_id")]
        public int? FieldActivityId { get; set; }
        //[MinLength(11)]
        //[MaxLength(12)]
        //[RegularExpression(@"(^[+]{0,1}[0-9]{11})"), StringLength(12)]
        [DisplayName("Тел. ответств-го")]
        [Column("Phone_number_responsible", TypeName = "varchar(12)")]
        public string PhoneNumberResponsible { get; set; }
        [DisplayName("Дата рег. на сайте")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column("Date_registration_site", TypeName = "date")]
        public DateTime? DateRegistrationSite { get; set; }
        [DisplayName("Дата регистрации")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column("Voting_date", TypeName = "DATETIME")]
        public DateTime? VotingDate { get; set; }
        [DisplayName("Зарегистрировался")]
        [Column("Voter", TypeName = "TINYINT")]
        public bool Voter { get; set; }
        [DisplayName("Адрес")]
        [Column(TypeName = "varchar(500)")]
        public string Adress { get; set; }
        [DisplayName("QR текст")]
        [Column("TextQRcode", TypeName = "varchar(256)")]
        public string TextQRcode { get; set; }
        [DisplayName("QRcode")]
        [Column("QRcode", TypeName = "varchar(4500)")]
        public string Qrcode { get; set; }
        [DisplayName("Email")]
        [Column("Email", TypeName = "varchar(256)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Примечание")]
        [Column(TypeName = "varchar(256)")]
        public string Description { get; set; }
        [DisplayName("Внесший")]
        [Column("User_id")]
        public long? UserId { get; set; }
        [Column("FriendStatus_id")]
        [DisplayName("Статус участника")]
        public int? FriendStatusId { get; set; }
        [Column("ByteQRcode", TypeName = "mediumblob")]
        public byte[] ByteQrcode { get; set; }
        [Column(TypeName = "varchar(45)")]
        public string TypeImage { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Friends")]
        [DisplayName("Город")]
        public virtual City City { get; set; }
        [DisplayName("Городской огруг")]
        [ForeignKey(nameof(CityDistrictId))]
        [InverseProperty("Friends")]
        public virtual CityDistrict CityDistrict { get; set; }
        [DisplayName("Округ")]
        [ForeignKey(nameof(ElectoralDistrictId))]
        [InverseProperty("Friends")]
        public virtual ElectoralDistrict ElectoralDistrict { get; set; }
        [DisplayName("Организация")]
        [ForeignKey(nameof(FieldActivityId))]
        [InverseProperty(nameof(Fieldactivity.Friends))]
        public virtual Fieldactivity FieldActivity { get; set; }
        [DisplayName("Статус участника")]
        [ForeignKey(nameof(FriendStatusId))]
        [InverseProperty("Friends")]
        public virtual FriendStatus FriendStatus { get; set; }
        [DisplayName("Подведом. учрежд.")]
        [ForeignKey(nameof(GroupUId))]
        [InverseProperty(nameof(Groupu.Friends))]
        public virtual Groupu GroupU { get; set; }
        [DisplayName("Дом")]
        [ForeignKey(nameof(HouseId))]
        [InverseProperty("Friends")]
        public virtual House House { get; set; }
        [DisplayName("Микрорайон")]
        [ForeignKey(nameof(MicroDistrictId))]
        [InverseProperty(nameof(Microdistrict.Friends))]
        public virtual Microdistrict MicroDistrict { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("Friends")]
        [DisplayName("Подразделение")]
        public virtual Organization Organization_ { get; set; }

        [ForeignKey(nameof(StationId))]
        [InverseProperty("Friends")]
        [DisplayName("Участок")]
        public virtual Station Station { get; set; }
        [DisplayName("Улица")]
        [ForeignKey(nameof(StreetId))]
        [InverseProperty("Friends")]
        public virtual Street Street { get; set; }
        [DisplayName("Записавший участника")]
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Friends")]
        public virtual User User { get; set; }

        [NotMapped]
        public Byte[] QRcodeBytes { get; set; }
    }
}
