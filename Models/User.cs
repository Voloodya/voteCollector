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
    [Table("user")]
    public partial class User
    {
        public User()
        {
            Friends = new HashSet<Friend>();
            Groupsusers = new HashSet<Groupsusers>();
        }

        [Key]
        [Column("Id_User")]
        public long IdUser { get; set; }
        [DisplayName("Пользователь")]
        [Required(ErrorMessage = "Не указано имя")]
        [Column(TypeName = "varchar(100)")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }
        [DisplayName("Роль")]
        [Column("Role_id")]
        public int? RoleId { get; set; }
        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Не указана фамилия")]
        [Column("Family_name", TypeName = "varchar(256)")]
        public string FamilyName { get; set; }
        [DisplayName("Имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [Column("Name_", TypeName = "varchar(256)")]
        public string Name { get; set; }
        [DisplayName("Отчество")]
        [Column("Patronymic_name", TypeName = "varchar(256)")]
        public string PatronymicName { get; set; }
        [DisplayName("Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column("Date_birth", TypeName = "date")]
        public DateTime? DateBirth { get; set; }
        [MinLength(11)]
        [MaxLength(12)]
        [RegularExpression(@"(^[+]{0,1}[0-9]{11})"), StringLength(12)]
        [DisplayName("Телефон")]
        [Column(TypeName = "varchar(12)")]
        public string Telephone { get; set; }

        [DisplayName("Роль")]
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; }
        [DisplayName("Друзья")]
        [InverseProperty("User")]
        public virtual ICollection<Friend> Friends { get; set; }
        [DisplayName("Группы")]
        [InverseProperty("User")]
        public virtual ICollection<Groupsusers> Groupsusers { get; set; }

        [DisplayName("Кол-во избирателей")]
        [NotMapped]
        public string numberFriends { get; set; }

    }
}
