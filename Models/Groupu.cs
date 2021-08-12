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
        }

        [Key]
        [Column("Id_Group")]
        public int IdGroup { get; set; }
        [DisplayName("Группа")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [DisplayName("Организация")]
        [Required(ErrorMessage = "Не указана отрасль")]
        [Column("FieldActivity_id")]
        public int? FieldActivityId { get; set; }
        [DisplayName("Подведом. учрежд.")]
        [Column("Organization_id")]
        public int? OrganizationId { get; set; }
        [DisplayName("Автор группы")]
        [Column(TypeName = "varchar(256)")]
        public string CreatorGroup { get; set; }

        [ForeignKey(nameof(FieldActivityId))]
        [InverseProperty(nameof(Fieldactivity.Groupus))]
        [DisplayName("Организация")]
        public virtual Fieldactivity FieldActivity { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(Fieldactivity.Groupus))]
        [DisplayName("Подведом. учрежд.")]
        public virtual Organization Organization { get; set; }

        [DisplayName("Избиратели")]
        [InverseProperty("GroupU")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("GroupU")]
        public virtual ICollection<Groupsusers> Groupsusers { get; set; }

    }
}
