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
    public class ReportDistrict : IComparable<ReportDistrict>
    {
        [Key]
        public int IdOdject { get; set; }
        [DisplayName("Округ")]
        public string DistrictName { get; set; }
        [DisplayName("Кол-во участников")]
        public int NumberVoters { get; set; }
        [DisplayName("Кол-во зарегистрированных участников")]
        public int NumberVoted { get; set; }
        [DisplayName("Количество QR-кодов")]
        public int NumberQRcodesText { get; set; }
        [DisplayName("Процент зарегистрированных к участникам")]
        public double PersentVotedByVoters { get; set; }


        public string FontWeight { get; set; }

        public int CompareTo(ReportDistrict other)
        {
            if (other == null) return 1;
            else
            {
                int count1 = 0;
                int count2 = 0;
                if (this.DistrictName.Trim().Equals("")) ;
                else if (int.TryParse(this.DistrictName, out count1)) ;
                if (other.DistrictName.Trim().Equals("")) ;
                else if (int.TryParse(other.DistrictName, out count2)) ;

                return count1 - count2;
            }
        }
    }
}
