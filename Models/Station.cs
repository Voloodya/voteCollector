using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace voteCollector.Models
{
    [Table("station")]
    public partial class Station : IComparable<Station>, IComparer<Station>
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
        [DisplayName("Участок")]
        public virtual ICollection<District> Districts { get; set; }
        [InverseProperty("Station")]
        [DisplayName("Участок")]
        public virtual ICollection<PollingStation> PollingStations { get; set; }

        [InverseProperty("Station")]
        [DisplayName("Участок")]

        public virtual ICollection<Friend> Friends { get; set; }

        public int Compare(Station x, Station y)
        {
            if (y == null) return 1;
            else if (x == null) return -1;
            else
            {
                string s1 = x.Name;
                string s2 = y.Name;
                if (x.Name.Trim().Equals("")) s1 = "0";
                if (y.Name.Trim().Equals("")) s2 = "0";
                return Convert.ToInt32(s1) - Convert.ToInt32(s2);
            }            
        }
        public int CompareTo(Station other)
        {
            if (other == null) return 1;
            else
            {
                int count1 = 0;
                int count2 = 0;
                if (this.Name.Trim().Equals(""));
                else if (int.TryParse(this.Name, out count1));
                if (other.Name.Trim().Equals(""));
                else if (int.TryParse(other.Name, out count2));

                return count1 - count2;
            }
        }
    }
}
