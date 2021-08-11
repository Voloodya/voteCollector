using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("city")]
    public partial class City
    {
        public City()
        {
            CityDistricts = new HashSet<CityDistrict>();
            Friends = new HashSet<Friend>();
        }

        [Key]
        [Column("Id_City")]
        public int IdCity { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }

        [InverseProperty("City")]
        public virtual ICollection<CityDistrict> CityDistricts { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<Friend> Friends { get; set; }
    }
}
