using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace voteCollector.Models
{
    [Table("friend_status")]
    public partial class FriendStatus
    {
        public FriendStatus()
        {
            Friends = new HashSet<Friend>();
        }

        [Key]
        [Column("Id_friend_status")]
        public int IdFriendStatus { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [InverseProperty("FriendStatus")]
        public virtual ICollection<Friend> Friends { get; set; }
    }
}
