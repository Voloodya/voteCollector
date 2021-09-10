using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Models
{
    public class ReportCity : IComparable<ReportCity>
    {
        [Key]
        public int IdOdject { get; set; }
        [DisplayName("Город")]
        public string CityName { get; set; }
        [DisplayName("Кол-во участников")]
        public int NumberVoters { get; set; }
        [DisplayName("Кол-во зарегистрированных участников")]
        public int NumberVoted { get; set; }
        [DisplayName("Количество QR-кодов")]
        public int NumberQRcodesText { get; set; }
        [DisplayName("Процент зарегистрированных к участникам")]
        public double PersentVotedByVoters { get; set; }


        public int CompareTo(ReportCity other)
        {
            if (other == null) return 1;

            return this.CityName.CompareTo(other.CityName);
        }
    }
}
