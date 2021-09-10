using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using voteCollector.Data;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportsController : Controller
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly VoterCollectorContext _context;
        private ServiceGroup _serviceGroup;

        public ReportsController(VoterCollectorContext context, ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
            _serviceGroup = new ServiceGroup(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReportOrganization(int id)
        {
            Groupu parentGroupu = _context.Groupu.Include(g => g.Organization).Include(g => g.UserResponsible).Include(g => g.InverseGroupParents)
                .Where(g => g.IdGroup == id).FirstOrDefault();

            List<Report> reports = new List<Report>();

            if (parentGroupu != null)
            {
                List<Friend> friends = _context.Friend.ToList();
                string nameParent = parentGroupu.Name;

                if (parentGroupu.InverseGroupParents != null && parentGroupu.InverseGroupParents.Count > 0)
                {

                    foreach (Groupu grpLvl2 in parentGroupu.InverseGroupParents)
                    {
                        _context.Entry(grpLvl2).Collection(g => g.InverseGroupParents).Load();
                        _context.Entry(grpLvl2).Reference(g => g.UserResponsible).Load();

                        Report report = new Report
                        {
                            Responseble = grpLvl2.UserResponsible != null ? grpLvl2.UserResponsible.FamilyName + " " + grpLvl2.UserResponsible.Name + " " + grpLvl2.UserResponsible.PatronymicName + " (" + grpLvl2.UserResponsible.Telephone + ")" : "",
                            IdOdject = grpLvl2.IdGroup,
                            NameObject = grpLvl2.Organization != null ? grpLvl2.Organization.Name : grpLvl2.Name,
                            Level = grpLvl2.Level ?? 0,
                            NumberEmployees = grpLvl2.NumberEmployees ?? 0,
                            NameParent = nameParent,
                            childGroup = false
                        };
                        int numberVoters = 0;
                        int numberVoted = 0;
                        int numberQRcodes = 0;

                        List<Groupu> groupsChild = _serviceGroup.GetAllChildGroupsBFS(grpLvl2, grpLvl2, grpLvl2);

                        if (groupsChild != null && groupsChild.Count > 0)
                        {

                            foreach (Groupu groupu in groupsChild)
                            {
                                if (groupu.IdGroup != grpLvl2.IdGroup) report.childGroup = true;

                                numberVoters += friends.Where(f => f.GroupUId == groupu.IdGroup).Count();
                                numberVoted += friends.Where(f => f.GroupUId == groupu.IdGroup && f.Voter == true).Count();
                                numberQRcodes += friends.Where(f => f.GroupUId == groupu.IdGroup && f.TextQRcode != null && !f.TextQRcode.Equals("")).Count();
                            }
                        }
                        else
                        {
                            numberVoters += friends.Where(f => f.GroupUId == grpLvl2.IdGroup).Count();
                            numberVoted += friends.Where(f => f.GroupUId == grpLvl2.IdGroup && f.Voter == true).Count();
                            numberQRcodes += friends.Where(f => f.GroupUId == grpLvl2.IdGroup && f.TextQRcode != null && !f.TextQRcode.Equals("")).Count();
                        }
                        report.NumberVoters = numberVoters;
                        report.NumberVoted = numberVoted;
                        report.NumberQRcodesText = numberQRcodes;

                        if (numberVoters != 0)
                        {
                            report.PersentVotedByVoters = Math.Round((double)numberVoted / numberVoters * 100, 2);
                        }
                        if (report.NumberEmployees != 0)
                        {
                            report.PersentVotedByEmploees = Math.Round((double)numberVoted / report.NumberEmployees * 100, 2);
                        }
                        if (report.NumberEmployees != 0)
                        {
                            report.PersentVotersByEmploees = Math.Round((double)numberVoters / report.NumberEmployees * 100, 2);
                        }
                        reports.Add(report);
                    }
                }
                else
                {
                    reports.Add(new Report
                    {
                        NameParent = nameParent
                    });
                }
            }
            reports.Sort((r1, r2) => r1.NameObject.CompareTo(r2.NameObject));
            return View(reports);
        }

        [HttpGet]
        public async Task<IActionResult> ReportGroup(int id)
        {
            Groupu parentGroupu = _context.Groupu.Include(g => g.Organization).Include(g => g.UserResponsible).Include(g => g.InverseGroupParents)
                .Where(g => g.IdGroup == id).FirstOrDefault();

            List<Report> reports = new List<Report>();

            if (parentGroupu != null)
            {
                List<Friend> friends = _context.Friend.ToList();
                string nameParent = parentGroupu.Name;

                Report reportOwner = new Report
                {
                    Responseble = parentGroupu.UserResponsible != null ? parentGroupu.UserResponsible.FamilyName + " " + parentGroupu.UserResponsible.Name + " " + parentGroupu.UserResponsible.PatronymicName + " (" + parentGroupu.UserResponsible.Telephone + ")" : "",
                    IdOdject = parentGroupu.OrganizationId ?? 0,
                    NameObject = parentGroupu.Name != null ? parentGroupu.Name : "",
                    Level = parentGroupu.Level ?? 0,
                    NumberEmployees = parentGroupu.NumberEmployees ?? 0,
                    NameParent = nameParent,
                    NumberVoters = friends.Where(f => f.GroupUId == parentGroupu.IdGroup).Count(),
                    NumberVoted = friends.Where(f => f.GroupUId == parentGroupu.IdGroup && f.Voter == true).Count(),
                    NumberQRcodesText = friends.Where(f => f.GroupUId == parentGroupu.IdGroup && f.TextQRcode != null && !f.TextQRcode.Equals("")).Count()
                };

                if (reportOwner.NumberVoters != 0)
                {
                    reportOwner.PersentVotedByVoters = Math.Round((double)reportOwner.NumberVoted / reportOwner.NumberVoters * 100, 2);
                }
                if (reportOwner.NumberEmployees != 0)
                {
                    reportOwner.PersentVotedByEmploees = Math.Round((double)reportOwner.NumberVoted / reportOwner.NumberEmployees * 100, 2);
                }
                if (reportOwner.NumberEmployees != 0)
                {
                    reportOwner.PersentVotersByEmploees = Math.Round((double)reportOwner.NumberVoters / reportOwner.NumberEmployees * 100, 2);
                }

                if (parentGroupu.InverseGroupParents != null && parentGroupu.InverseGroupParents.Count > 0)
                {

                    foreach (Groupu grpLvl3 in parentGroupu.InverseGroupParents)
                    {
                        _context.Entry(grpLvl3).Reference(g => g.UserResponsible).Load();

                        Report report = new Report
                        {
                            Responseble = grpLvl3.UserResponsible != null ? grpLvl3.UserResponsible.FamilyName + " " + grpLvl3.UserResponsible.Name + " " + grpLvl3.UserResponsible.PatronymicName + " (" + grpLvl3.UserResponsible.Telephone + ")" : "",
                            IdOdject = grpLvl3.OrganizationId ?? 0,
                            NameObject = grpLvl3.Name != null ? grpLvl3.Name : "",
                            Level = grpLvl3.Level ?? 0,
                            NumberEmployees = grpLvl3.NumberEmployees ?? 0,
                            NameParent = nameParent
                        };
                        int numberVoters = 0;
                        int numberVoted = 0;
                        int numberQRcodes = 0;

                        List<Groupu> groupsChild = _serviceGroup.GetAllChildGroupsBFS(grpLvl3, grpLvl3, grpLvl3);

                        if (groupsChild != null && groupsChild.Count > 0)
                        {
                            foreach (Groupu groupu in groupsChild)
                            {
                                numberVoters += friends.Where(f => f.GroupUId == groupu.IdGroup).Count();
                                numberVoted += friends.Where(f => f.GroupUId == groupu.IdGroup && f.Voter == true).Count();
                                numberQRcodes += friends.Where(f => f.GroupUId == groupu.IdGroup && f.TextQRcode != null && !f.TextQRcode.Equals("")).Count();
                            }
                            report.NumberVoters = numberVoters;
                            report.NumberVoted = numberVoted;
                            report.NumberQRcodesText = numberQRcodes;
                        }
                        else
                        {
                            report.NumberVoters = friends.Where(f => f.GroupUId == grpLvl3.IdGroup).Count();
                            report.NumberVoted = friends.Where(f => f.GroupUId == grpLvl3.IdGroup && f.Voter == true).Count();
                            report.NumberQRcodesText = friends.Where(f => f.GroupUId == grpLvl3.IdGroup && f.TextQRcode != null && !f.TextQRcode.Equals("")).Count();
                        }

                        if (numberVoters != 0)
                        {
                            report.PersentVotedByVoters = Math.Round((double)numberVoted / numberVoters * 100, 2);
                        }
                        if (report.NumberEmployees != 0)
                        {
                            report.PersentVotedByEmploees = Math.Round((double)numberVoted / report.NumberEmployees * 100, 2);
                        }
                        if (report.NumberEmployees != 0)
                        {
                            report.PersentVotersByEmploees = Math.Round((double)numberVoters / report.NumberEmployees * 100, 2);
                        }
                        reports.Add(report);
                    }
                }
                else
                {
                    reports.Add(new Report
                    {
                        NameParent = nameParent
                    });
                }
                reports.Sort((r1, r2) => r1.NameObject.CompareTo(r2.NameObject));
                reports.Insert(0, reportOwner);
            }

            return View(reports);
        }


        [HttpGet]
        public async Task<IActionResult> ReportDistrictGovLaw()
        {
            List<ReportDistrict> reportDistricts = new List<ReportDistrict>();

            List<ElectoralDistrictGovDuma> electoralDistrictGovDumas = _context.ElectoralDistrictGovDuma.ToList();
            List<Friend> friends = _context.Friend.ToList();
            List<District> districts = _context.District.ToList();

            foreach (ElectoralDistrictGovDuma electoralDistrictGovDuma in electoralDistrictGovDumas)
            {
                int?[] idStationsGovDuma = districts.Where(d => d.ElectoralDistrictGovDumaId == electoralDistrictGovDuma.IdElectoralDistrictGovDuma).Select(d => d.StationId).ToArray();
                int numberVoters = 0;
                int numberVoted = 0;

                if (idStationsGovDuma != null)
                {
                    numberVoters = friends.Where(f => idStationsGovDuma.Contains(f.StationId)).Count();
                    numberVoted = friends.Where(f => idStationsGovDuma.Contains(f.StationId) && f.Voter == true).Count();
                }

                ReportDistrict reportDistrict = new ReportDistrict
                {
                    IdOdject = electoralDistrictGovDuma.IdElectoralDistrictGovDuma,
                    DistrictName = electoralDistrictGovDuma.Name,
                    NumberVoters = numberVoters,
                    NumberVoted = numberVoted,
                    FontWeight = "bold",
                    PersentVotedByVoters = numberVoters != 0 ? Math.Round((double)numberVoted / numberVoters * 100, 2) : 0,
                    NumberQRcodesText = friends.Where(f => (idStationsGovDuma.Contains(f.StationId)) && (f.TextQRcode != null ? !f.TextQRcode.Trim().Equals("") : false)).Count()
                };
                if (reportDistrict.NumberVoters > 0)
                {
                    reportDistricts.Add(reportDistrict);
                }

                int?[] idDistrictsAssembleLaw = districts.Where(d => d.ElectoralDistrictGovDumaId == electoralDistrictGovDuma.IdElectoralDistrictGovDuma).Select(d => d.ElectoralDistrictAssemblyLawId).ToArray();

                List<ElectoralDistrictAssemblyLaw> electoralDistrictAssemblyLaws = _context.ElectoralDistrictAssemblyLaw.Where(ed => idDistrictsAssembleLaw.Contains(ed.IdElectoralDistrictAssemblyLaw)).ToList();

                //ReportDistrict reportDistrictEmpty = new ReportDistrict
                //{
                //    DistrictName = "",
                //};
                //reportDistricts.Add(reportDistrictEmpty);
                foreach (ElectoralDistrictAssemblyLaw electoralDistrictAssemblyLaw in electoralDistrictAssemblyLaws)
                {
                    int?[] idStationsAssemblyLaw = districts.Where(d => d.ElectoralDistrictAssemblyLawId == electoralDistrictAssemblyLaw.IdElectoralDistrictAssemblyLaw &&
                    d.ElectoralDistrictGovDumaId == electoralDistrictGovDuma.IdElectoralDistrictGovDuma).Select(d => d.StationId).ToArray();

                    if (idStationsAssemblyLaw != null)
                    {
                        numberVoters = friends.Where(f => idStationsAssemblyLaw.Contains(f.StationId)).Count();
                        numberVoted = friends.Where(f => idStationsAssemblyLaw.Contains(f.StationId) && f.Voter == true).Count();
                    }

                    ReportDistrict reportDistrictLaw = new ReportDistrict
                    {
                        IdOdject = electoralDistrictAssemblyLaw.IdElectoralDistrictAssemblyLaw,
                        DistrictName = electoralDistrictAssemblyLaw.Name,
                        NumberVoters = numberVoters,
                        NumberVoted = numberVoted,
                        PersentVotedByVoters = numberVoters != 0 ? Math.Round((double)numberVoted / numberVoters * 100, 2) :0,
                        NumberQRcodesText = friends.Where(f => (idStationsAssemblyLaw.Contains(f.StationId)) && (f.TextQRcode != null ? !f.TextQRcode.Trim().Equals("") : false)).Count()
                    };

                    if (reportDistrictLaw.NumberVoters > 0)
                    {
                        reportDistricts.Add(reportDistrictLaw);
                    }
                }
            }

            return View(reportDistricts);
        }

        [HttpGet]
        public async Task<IActionResult> ReportStations()
        {
            List<ReportStation> reportStations = new List<ReportStation>();

            List<IGrouping<string, Friend>> frndsStationsGrp = _context.Friend.Include(f => f.Station).ToList().GroupBy(f => (f.Station != null ? f.Station.Name : "")).ToList();

            foreach(IGrouping<string, Friend> frnd in frndsStationsGrp)
            {
                int numberVoters = frnd.Count();
                int numberVoted = frnd.Where(f => f.Voter == true).Count();
                ReportStation reportStation = new ReportStation
                {
                    IdOdject = frnd.First().GroupUId ?? 0,
                    DistrictName = frnd.Key,
                    NumberVoters = numberVoters,
                    NumberVoted = numberVoted,
                    PersentVotedByVoters = numberVoters != 0 ? Math.Round((double)numberVoted / numberVoters * 100, 2) : 0,
                    NumberQRcodesText = frnd.Where(f => f.TextQRcode != null ? !f.TextQRcode.Trim().Equals("") : false).Count()
                };
                reportStations.Add(reportStation);
            }

            reportStations.Sort();
            return View(reportStations);
        }

        [HttpGet]
        public async Task<IActionResult> ReportCities()
        {
            List<ReportCity> reportCities = new List<ReportCity>();

            List<IGrouping<string, Friend>> frndsStationsGrp = _context.Friend.Include(f => f.City).ToList().GroupBy(f => (f.City != null ? f.City.Name : "")).ToList();

            foreach (IGrouping<string, Friend> frnd in frndsStationsGrp)
            {
                int numberVoters = frnd.Count();
                int numberVoted = frnd.Where(f => f.Voter == true).Count();

                ReportCity reportStation = new ReportCity
                {
                    IdOdject = frnd.First().CityId ?? 0,
                    CityName = frnd.Key,
                    NumberVoters = numberVoters,
                    NumberVoted = numberVoted,
                    PersentVotedByVoters = numberVoters != 0 ? Math.Round((double)numberVoted / numberVoters * 100, 2) : 0,
                    NumberQRcodesText = frnd.Where(f => f.TextQRcode != null ? !f.TextQRcode.Trim().Equals("") : false).Count()
                };

                reportCities.Add(reportStation);
            }

            reportCities.Sort();

            return View(reportCities);
        }

    }
}
