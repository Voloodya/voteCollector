using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("electoral_district")]
    public partial class ElectoralDistrict : IComparable<ElectoralDistrict>, IComparer<ElectoralDistrict>
    {
        public ElectoralDistrict()
        {
            Districts = new HashSet<District>();
            Friends = new HashSet<Friend>();
        }

        [Key]
        [Column("Id_ElectoralDistrict")]
        public int IdElectoralDistrict { get; set; }
        [Column(TypeName = "varchar(256)")]
        [DisplayName("Избирательный округ")]
        public string Name { get; set; }

        [InverseProperty("ElectoralDistrict")]
        public virtual ICollection<District> Districts { get; set; }
        [InverseProperty("ElectoralDistrict")]
        public virtual ICollection<Friend> Friends { get; set; }

        public int Compare([AllowNull] ElectoralDistrict x, [AllowNull] ElectoralDistrict y)
        {
            if (y == null) return 1;
            else if (x == null) return -1;
            else return x.Name.CompareTo(y.Name);
        }

        public int CompareTo([AllowNull] ElectoralDistrict other)
        {
            if (other == null) return 1;
            else return this.Name.CompareTo(other.Name);
        }
    }
}
