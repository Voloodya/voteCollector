using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;

namespace voteCollector.Services
{
    public class ServiceFriends
    {
        private readonly VoterCollectorContext _context;

        public ServiceFriends()
        {
            _context = new VoterCollectorContext();
        }

        public Friend FindUserByQRtext(string qrText)
        {
            Friend friend = null;

            try
            {
                friend = _context.Friend.Where(frnd => frnd.TextQRcode.Equals(qrText)).FirstOrDefault();
            }
            catch(Exception ex)
            {

            }

            return friend;
        }

        public async Task<string> SaveFriends(Friend friend)
        {
            try
            {
                _context.Update(friend);
                await _context.SaveChangesAsync();

                return "Данные пользователя обновлены";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.ToString();
            }
        }

        public IQueryable<Friend> SearchFriendsByElectoralDistrict(ElectoralDistrictDTO electoralDistrict)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.ElectoralDistrictId == electoralDistrict.IdElectoralDistrict);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByElectoralDistrictAndGroups(ElectoralDistrictDTO electoralDistrict, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.ElectoralDistrictId == electoralDistrict.IdElectoralDistrict && groupsUser.Contains(frnd.GroupU));

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByFieldActivite(FieldActivityDTO fieldActivityDTO)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByFieldActiviteAndGroups(FieldActivityDTO fieldActivityDTO, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity && groupsUser.Contains(frnd.GroupU));

            return friends;
        }


        public IQueryable<Friend> SearchFriendsByNameElectoralDistrict(string nameElectoralDistrict)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Include(f => f.Organization_).Where(frnd => frnd.ElectoralDistrict.Name.Equals(nameElectoralDistrict));

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByNameElectoralDistrictAndGroups(string nameElectoralDistrict, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.ElectoralDistrict.Name.Equals(nameElectoralDistrict) && groupsUser.Contains(frnd.GroupU));

            return friends;
        }

        public List<Friend> SearchFriendsByIds(long[] idFriends)
        {
            List<Friend> friends = new List<Friend>();
            foreach (long id in idFriends)
            {
                Friend friend = _context.Friend.Find(id);

                if (friend != null)
                {
                    friends.Add(friend);
                }
                else { friends.Add(null); }
            }
            return friends;
        }
        public Friend CreateFreand(FriendDTO friendDTO, Regex regexTelephone, User user, List<Groupu> groupsUser, VoterCollectorContext _context)
        {
            Friend newFriend = new Friend();

            ServiceUser serviceUser = new ServiceUser(_context);

            newFriend.UserId = friendDTO.UserId;
            DateTime datesBirth;

            string[] dates = friendDTO.DateBirth.Trim().Split('.');
            try
            {
                datesBirth = new DateTime(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));
            }
            catch
            {
                datesBirth = new DateTime();
            }

            List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friendDTO.Name.Trim()) && frnd.FamilyName.Equals(friendDTO.FamilyName.Trim()) && frnd.PatronymicName.Equals(friendDTO.PatronymicName.Trim()) && frnd.DateBirth.Value.Date == datesBirth).ToList();

            if (searchFriend.Count == 0)
            {

                newFriend.FamilyName = friendDTO.FamilyName.Trim();
                newFriend.Name = friendDTO.Name.Trim();
                newFriend.PatronymicName = friendDTO.PatronymicName.Trim();
                newFriend.DateBirth = datesBirth;

                if (friendDTO.CityName != null && !friendDTO.CityName.Trim().Equals(""))
                {
                    int cityId = _context.City.Where(c => c.Name.Equals(friendDTO.CityName.Trim())).FirstOrDefault().IdCity;
                    newFriend.CityId = cityId;

                    int streetId = _context.Street.Where(s => s.Name.Equals(friendDTO.Street.Trim())).FirstOrDefault().IdStreet;
                    newFriend.StreetId = streetId;
                    if (friendDTO.Microdistrict != null && !friendDTO.Microdistrict.Trim().Equals(""))
                    {
                        newFriend.MicroDistrictId = _context.Microdistrict.Where(md => md.Name.Equals(friendDTO.Microdistrict.Trim())).FirstOrDefault().IdMicroDistrict;
                    }
                    House house = _context.House.Where(h => h.StreetId==streetId && h.Name.Equals(friendDTO.House.Trim())).FirstOrDefault();

                    if (house != null)
                    {
                        newFriend.HouseId = house.IdHouse;
                        newFriend.Apartment = friendDTO.Apartment.Trim();

                        PollingStation pollingStation = _context.PollingStation.Where(p => (p.CityId == cityId && p.StreetId == streetId && p.HouseId == house.IdHouse)).FirstOrDefault();
                        if (pollingStation != null)
                        {
                            newFriend.StationId = pollingStation.StationId;
                        }
                        else if (friendDTO.PollingStationName != null && !friendDTO.PollingStationName.Trim().Equals(""))
                        {
                            PollingStation pollingStationSearch = _context.PollingStation.Where(p => p.Name.Equals(friendDTO.PollingStationName.Trim())).FirstOrDefault();
                            //newFriend.PollingStationId = pollingStationSearch.IdPollingStation;
                            newFriend.StationId = pollingStationSearch.StationId;
                        }
                        if (friendDTO.ElectiralDistrict != null && !friendDTO.ElectiralDistrict.Trim().Equals(""))
                        {
                            newFriend.ElectoralDistrictId = _context.ElectoralDistrict.Where(d => d.Name.Equals(friendDTO.ElectiralDistrict.Trim())).FirstOrDefault().IdElectoralDistrict;
                        }
                        else
                        {
                            District district = _context.District.Where(d => d.StationId == newFriend.StationId).FirstOrDefault();

                            if (district != null)
                            {
                                newFriend.ElectoralDistrictId = district.ElectoralDistrictId;
                            }
                        }
                    }

                }
                else
                {
                    if (friendDTO.PollingStationName != null && !friendDTO.PollingStationName.Trim().Equals(""))
                    {
                        PollingStation pollingStationSearch = _context.PollingStation.Where(p => p.Name.Equals(friendDTO.PollingStationName.Trim())).FirstOrDefault();

                        if (pollingStationSearch != null)
                        {
                            newFriend.StationId = pollingStationSearch.StationId;

                            District district = _context.District.Where(d => d.StationId == newFriend.StationId).FirstOrDefault();

                            if (district != null)
                            {
                                newFriend.ElectoralDistrictId = district.ElectoralDistrictId;
                            }
                        }

                    }
                }        

                string telephone = friendDTO.Telephone.Trim().Replace("-", "");
                if (regexTelephone.IsMatch(telephone))
                {
                    newFriend.Telephone = telephone;
                }
                string telephoneResponsible = friendDTO.PhoneNumberResponsible.Trim().Replace("-", "");
                if (regexTelephone.IsMatch(telephoneResponsible))
                {
                    newFriend.PhoneNumberResponsible = telephoneResponsible;
                }

                if (friendDTO.Email != null)
                {
                    newFriend.Email = friendDTO.Email.Trim();
                }
                if (friendDTO.TextQRcode != null)
                {
                    newFriend.TextQRcode = friendDTO.TextQRcode.Trim();
                }
                newFriend.Organization = friendDTO.Organization.Trim();
                if (friendDTO.FieldActivityName != null && !friendDTO.FieldActivityName.Trim().Equals(""))
                {
                    newFriend.FieldActivityId = _context.Fieldactivity.Where(f => f.Name.Equals(friendDTO.FieldActivityName.Trim())).FirstOrDefault().IdFieldActivity;
                }
                if (friendDTO.DateBirth != null && !friendDTO.DateBirth.Trim().Equals(""))
                {
                    string[] datesR = friendDTO.DateRegistrationSite.Trim().Split('.');
                    DateTime datesRegistration = new DateTime(Convert.ToInt32(datesR[2]), Convert.ToInt32(datesR[1]), Convert.ToInt32(datesR[0]));
                    newFriend.DateRegistrationSite = datesRegistration;
                }
                if (friendDTO.VotingDate != null && !friendDTO.VotingDate.Trim().Equals(""))
                {
                    string[] datesV = friendDTO.VotingDate.Trim().Split('.');
                    DateTime datesVoting = new DateTime(Convert.ToInt32(datesV[2]), Convert.ToInt32(datesV[1]), Convert.ToInt32(datesV[0]));
                    newFriend.VotingDate = datesVoting;
                }
                newFriend.Description = friendDTO.Description;

                if (friendDTO.Group != null && !friendDTO.Group.Trim().Equals(""))
                {
                    newFriend.GroupUId = _context.Groupu.Where(g => g.Name.Equals(friendDTO.Group.Trim())).FirstOrDefault().IdGroup;
                }
                else
                {
                    newFriend.GroupUId = groupsUser[0].IdGroup;
                }
                if (friendDTO.Vote != null && !friendDTO.Vote.Trim().Equals(""))
                {
                    newFriend.Voter = friendDTO.Vote.ToLower().Trim().Equals("да") ? true : false;
                }
            }
            else
            {
                throw new Exception("Пользователь уже есть в списках!");
            }

            return newFriend;
        }

        public void RemoveFriends(List<Friend> friends)
        {
            foreach (Friend friend in friends)
            {
                if (friend != null)
                {
                    _context.Friend.Remove(friend);
                    _context.SaveChanges();
                }
            }
        }

    }
}
