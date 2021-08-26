using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Models
{
    [NotMapped]
    public class Report
    {
        [Key]
        public int IdOdject { get; set; }
        [DisplayName("Уровень")]
        public int Level { get; set; }
        [DisplayName("Наименование")]
        public string NameObject { get; set; }
        [DisplayName("Ответственный")]
        public string Responseble { get; set; }
        [DisplayName("Кол-во избирателей")]
        public int NumberVoters { get; set; }
        [DisplayName("Кол-во проголосовавших")]
        public int NumberVoted { get; set; }
        [DisplayName("Численность сотрудников")]
        public int NumberEmployees { get; set; }
        [DisplayName("Процент избирателей к численности")]
        public double PersentVotersByEmploees { get; set; }
        [DisplayName("Процент проголосовавших к избирателям")]
        public double PersentVotedByVoters { get; set; }
        [DisplayName("Процент проголосовавших к численности")]
        public double PersentVotedByEmploees { get; set; }
        public int IdParent { get; set; }

        [DisplayName("Вышестоящая структурная единица")]
        public string NameParent { get; set; }
        public List<Organization> ChildOrganization { get; set; }
        public List<Groupu> ChildGroupu { get; set; }
    }
}
