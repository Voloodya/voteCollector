using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace voteCollector.Models
{
    [Table("station")]
    public partial class Station
    {
        public Station()
        {
            Districts = new HashSet<District>();
            PollingStations = new HashSet<PollingStation>();
            Friends = new HashSet<Friend>();
        }

        [Key]
        [Column("Id_Station")]
        public int IdStation { get; set; }
        [Column(TypeName = "varchar(256)")]
        [DisplayName("Участок")]
        public string Name { get; set; }

        [InverseProperty("Station")]
        public virtual ICollection<District> Districts { get; set; }

        [InverseProperty("Station")]
        public virtual ICollection<PollingStation> PollingStations { get; set; }

        [InverseProperty("Station")]
        public virtual ICollection<Friend> Friends { get; set; }
    }
}
