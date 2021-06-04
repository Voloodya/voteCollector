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
    [Table("street")]
    public partial class Street
    {
        public Street()
        {
            Friends = new HashSet<Friend>();
            Houses = new HashSet<House>();
            PollingStations = new HashSet<PollingStation>();
        }

        [Key]
        [Column("Id_Street")]
        public int IdStreet { get; set; }
        [DisplayName("Улица")]
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column("City_id")]
        public int? CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Streets")]
        public virtual City City { get; set; }
        [InverseProperty("Street")]
        public virtual ICollection<Friend> Friends { get; set; }
        [InverseProperty("Street")]
        public virtual ICollection<House> Houses { get; set; }
        [InverseProperty("Street")]
        public virtual ICollection<PollingStation> PollingStations { get; set; }
    }
}
