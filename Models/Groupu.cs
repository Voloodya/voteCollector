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
    [Table("groupu")]
    public partial class Groupu
    {
        public Groupu()
        {
            Friends = new HashSet<Friend>();
            Groupsusers = new HashSet<Groupsusers>();
            InverseGroupParents = new HashSet<Groupu>();
        }

        [Key]
        [Column("Id_Group")]
        public int IdGroup { get; set; }
        [DisplayName("Группа")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [DisplayName("Организация")]
        //[Required(ErrorMessage = "Не указана отрасль")]
        [Column("FieldActivity_id")]
        public int? FieldActivityId { get; set; }
        [DisplayName("Подведом. учрежд.")]
        [Column("Organization_id")]
        public int? OrganizationId { get; set; }
        [DisplayName("Автор группы")]
        [Column(TypeName = "varchar(256)")]
        public string CreatorGroup { get; set; }
        [DisplayName("Уровень группы")]
        [Column("Level")]
        public int? Level { get; set; }
        [DisplayName("Подчиняется")]
        [Column("Group_Parents_id")]
        public int? GroupParentsId { get; set; }
        [DisplayName("Численность")]
        [Column("NumberEmployees")]
        public int? NumberEmployees { get; set; }
        [DisplayName("Ответственный")]
        [Column("User_Responsible_id")]
        public long? UserResponsibleId { get; set; }

        [ForeignKey(nameof(FieldActivityId))]
        [InverseProperty(nameof(Fieldactivity.Groupus))]
        [DisplayName("Организация")]
        public virtual Fieldactivity FieldActivity { get; set; }
        [DisplayName("Подчиняется")]
        [ForeignKey(nameof(GroupParentsId))]
        [InverseProperty(nameof(Groupu.InverseGroupParents))]
        public virtual Groupu GroupParents { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(Fieldactivity.Groupus))]
        [DisplayName("Подведом. учрежд.")]
        public virtual Organization Organization { get; set; }
        [DisplayName("Ответственный")]
        [ForeignKey(nameof(UserResponsibleId))]
        [InverseProperty(nameof(User.Groupus))]
        public virtual User UserResponsible { get; set; }

        [DisplayName("Избиратели")]
        [InverseProperty("GroupU")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("GroupU")]
        public virtual ICollection<Groupsusers> Groupsusers { get; set; }
        [InverseProperty(nameof(Groupu.GroupParents))]
        public virtual ICollection<Groupu> InverseGroupParents { get; set; }

        [NotMapped]
        public bool Visited { get; set; }
    }
}
