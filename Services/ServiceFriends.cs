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
        private string NameServer;
        private string WayController;
        private string NameQRcodeParametrs;
        private string WayPathQrCodes;

        public ServiceFriends()
        {
            _context = new VoterCollectorContext();
            NameServer = "http://оренбургвсе.рф";
            //WayController = "/CollectVoters/api/QRcodeСheckAPI/checkqrcode";
            WayController = "/api/QRcodeСheckAPI/checkqrcode";
            NameQRcodeParametrs = "qrText";
            WayPathQrCodes = "/wwwroot/qr_codes/";
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
                throw new Exception("Пользователь с данным QR кодом не найден!");
            }
            if (friend == null)
            {
                throw new Exception("Пользователь с данным QR кодом не найден!");
            }

            return friend;
        }

        public Friend FindUserByPhoneNumber(string phoneNumber)
        {
            Friend friend = null;

            try
            {
                friend = _context.Friend.Where(frnd => frnd.Telephone.Substring(1,10).Equals(phoneNumber)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Пользователь с данным номером телефона не найден!");
            }
            if (friend==null)
            {
                throw new Exception("Пользователь с данным номером телефона не найден!");
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

        public IQueryable<Friend> GetAllFriends()
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_);

            return friends;
        }

        public NumberFriendsDTO CountNumberAllFriends()
        {
            IQueryable<Friend> friends = _context.Friend;

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            return numberFriendsDTO;
        }

        public IQueryable<Friend> GetAllFriendsLimit(int limit)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Take(limit);

            return friends;
        }

        public IQueryable<Friend> GetAllFriendsByGroupUsers(List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(f => groupsUser.Contains(f.GroupU));

            return friends;
        }

        public NumberFriendsDTO CountNumberAllFriendsByGroupsUsers(List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Where(f => groupsUser.Contains(f.GroupU));

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            return numberFriendsDTO;
        }

        public IQueryable<Friend> GetAllFriendsByGroupUsersLimit(List<Groupu> groupsUser, int limit)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(f => groupsUser.Contains(f.GroupU)).Take(limit);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByFieldActivite(FieldActivityDTO fieldActivityDTO)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity);

            return friends;
        }

        public NumberFriendsDTO CountNumberFriendsByFieldActivite(FieldActivityDTO fieldActivityDTO)
        {
            IQueryable<Friend> friends = _context.Friend.Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity);

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            return numberFriendsDTO;
        }

        public IQueryable<Friend> SearchFriendsByFieldActiviteLimit(FieldActivityDTO fieldActivityDTO, int limit)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity).Take(limit);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByFieldActiviteAndGroupsUsers(FieldActivityDTO fieldActivityDTO, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity && groupsUser.Contains(frnd.GroupU));

            return friends;
        }

        public NumberFriendsDTO CountNumberFriendsByFieldActiviteAndGroupsUsers(FieldActivityDTO fieldActivityDTO, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity && groupsUser.Contains(frnd.GroupU));

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            return numberFriendsDTO;
        }

        public IQueryable<Friend> SearchFriendsByFieldActiviteAndGroupsUsersLimit(FieldActivityDTO fieldActivityDTO, List<Groupu> groupsUser, int limit)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.FieldActivityId == fieldActivityDTO.IdFieldActivity && groupsUser.Contains(frnd.GroupU)).Take(limit);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByOrganization(OrganizationDTO organizationDTO)
        {
            //
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.OrganizationId == organizationDTO.IdOrganization);

            return friends;
        }

        public NumberFriendsDTO CountNumberFriendsByOrganization(OrganizationDTO organizationDTO)
        {
            //
            IQueryable<Friend> friends = _context.Friend.Where(frnd => frnd.OrganizationId == organizationDTO.IdOrganization);

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            return numberFriendsDTO;
        }
        public IQueryable<Friend> SearchFriendsByOrganizationLimit(OrganizationDTO organizationDTO, int limit)
        {
            //
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.OrganizationId == organizationDTO.IdOrganization).Take(limit);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByOrganizationAndGroupsUsers(OrganizationDTO organizationDTO, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.OrganizationId == organizationDTO.IdOrganization && groupsUser.Contains(frnd.GroupU));

            return friends;
        }

        public NumberFriendsDTO CountNumberFriendsByOrganizationAndGroupsUsers(OrganizationDTO organizationDTO, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Where(frnd => frnd.OrganizationId == organizationDTO.IdOrganization && groupsUser.Contains(frnd.GroupU));

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            return numberFriendsDTO;
        }

        public IQueryable<Friend> SearchFriendsByOrganizationAndGroupsUsersLimit(OrganizationDTO organizationDTO, List<Groupu> groupsUser, int limit)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.OrganizationId == organizationDTO.IdOrganization && groupsUser.Contains(frnd.GroupU)).Take(limit);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByGroup(GroupDTO groupDTO)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.GroupUId == groupDTO.IdGroup);

            return friends;
        }

        public NumberFriendsDTO CountNumberFriendsByGroup(GroupDTO groupDTO)
        {
            IQueryable<Friend> friends = _context.Friend.Where(frnd => frnd.GroupUId == groupDTO.IdGroup);

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            return numberFriendsDTO;
        }


        public IQueryable<Friend> SearchFriendsByGroupLimit(GroupDTO groupDTO, int limit)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.GroupUId == groupDTO.IdGroup).Take(limit);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByGroupAndGroupsUsers(GroupDTO groupDTO, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.GroupUId == groupDTO.IdGroup && groupsUser.Contains(frnd.GroupU));

            return friends;
        }

        public NumberFriendsDTO CountNumberFriendsByGroupAndGroupsUsers(GroupDTO groupDTO, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Where(frnd => frnd.GroupUId == groupDTO.IdGroup && groupsUser.Contains(frnd.GroupU));

            NumberFriendsDTO numberFriendsDTO = new NumberFriendsDTO { NumberFriends = friends.Count(), NumberVoter = friends.Where(f => f.Voter == true).Count() };
            
            return numberFriendsDTO;
        }


        public IQueryable<Friend> SearchFriendsByGroupAndGroupsUsersLimit(GroupDTO groupDTO, List<Groupu> groupsUser, int limit)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.GroupUId == groupDTO.IdGroup && groupsUser.Contains(frnd.GroupU)).Take(limit);

            return friends;
        }
        public IQueryable<Friend> SearchFriendsByFieldActiviteAndOrganization(int idFieldActivity, int idOrganization)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.FieldActivityId == idFieldActivity && frnd.OrganizationId == idOrganization);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByElectoralDistrict(ElectoralDistrictDTO electoralDistrict)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.ElectoralDistrictId == electoralDistrict.IdElectoralDistrict);

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByElectoralDistrictAndGroupsUsers(ElectoralDistrictDTO electoralDistrict, List<Groupu> groupsUser)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.ElectoralDistrictId == electoralDistrict.IdElectoralDistrict && groupsUser.Contains(frnd.GroupU));

            return friends;
        }

        public IQueryable<Friend> SearchFriendsByNameElectoralDistrict(string nameElectoralDistrict)
        {
            IQueryable<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(frnd => frnd.ElectoralDistrict.Name.Equals(nameElectoralDistrict));

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

        public Friend CreateFreand(FriendDTO friendDTO, Regex regexTelephone, User userSave, List<Groupu> groupsUser, DateTime dateRegistration, VoterCollectorContext _context)
        {
            Friend newFriend = new Friend();

            ServiceUser serviceUser = new ServiceUser(_context);

            newFriend.UserId = friendDTO.UserId;
            DateTime datesBirth;
            string[] dates;

            try
            {
                dates = friendDTO.DateBirth.Trim().Split('.');
            }
            catch
            {
                throw new Exception("Не указана или указана в неверном формате дата рождения!");
            }
            try
            {
                datesBirth = new DateTime(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));
            }
            catch
            {
                throw new Exception("Не указана или указана в неверном формате дата рождения!");
            }

            List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friendDTO.Name.Trim()) && frnd.FamilyName.Equals(friendDTO.FamilyName.Trim()) && frnd.PatronymicName.Equals(friendDTO.PatronymicName.Trim()) && frnd.DateBirth.Value.Date == datesBirth).ToList();

            if (searchFriend.Count == 0)
            {
                if (!friendDTO.FamilyName.Trim().Equals("") && !friendDTO.Name.Trim().Equals(""))
                {
                    newFriend.FamilyName = friendDTO.FamilyName.Trim();
                    newFriend.Name = friendDTO.Name.Trim();
                    newFriend.PatronymicName = friendDTO.PatronymicName.Trim();
                }
                else
                {
                    throw new Exception("Не указана фамилия или имя избирателя!");
                }
                newFriend.DateBirth = datesBirth;
                newFriend.DateRegistrationSite = dateRegistration;

                if (friendDTO.FriendStatus != null && !friendDTO.FriendStatus.Trim().Equals(""))
                {
                    FriendStatus friendStatus = _context.FriendStatus.Where(fs => fs.Name.Equals(friendDTO.FriendStatus)).FirstOrDefault();
                    if (friendStatus != null) {
                        newFriend.FriendStatusId = friendStatus.IdFriendStatus;
                            }
                    else
                    {
                        throw new Exception("Не верно указан статус избирателя!");
                    }
                }
                else
                {
                    FriendStatus friendStatus = _context.FriendStatus.Where(fs => fs.Name.Equals("Сотрудник")).FirstOrDefault();
                    if (friendStatus != null) {
                        newFriend.FriendStatusId = friendStatus.IdFriendStatus;
                            }
                }

                bool unpinning = false;

                if (friendDTO.Unpinning != null && !friendDTO.Unpinning.Trim().Equals(""))
                {
                    unpinning = friendDTO.Unpinning.Trim().Equals("Да") ? true : false;
                }

                City city = _context.City.FirstOrDefault(c => c.Name.Equals(friendDTO.City));

                if (city != null)
                {
                    newFriend.CityId = city.IdCity;

                    if (unpinning)
                    {
                        newFriend.Unpinning = unpinning;
                        if (friendDTO.Adress != null && !friendDTO.Adress.Trim().Equals(""))
                        {
                            newFriend.Adress = friendDTO.Adress;
                        }
                        else
                        {
                            throw new Exception("Не указан адресс для иногороднего!");
                        }

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
                                else
                                {
                                    throw new Exception("Не найден избирательный округ по указанному участку!");
                                }
                            }
                            else
                            {
                                throw new Exception("Не указан или указан не верно избирательный участок!");
                            }
                        }
                    }
                    else
                    {
                        if (friendDTO.CityDistrict == null) friendDTO.CityDistrict = " ";
                        if (friendDTO.CityDistrict.Trim().Equals("Оренбург")) friendDTO.CityDistrict = " ";                        
                        if (friendDTO.CityDistrict != null)
                        {
                            CityDistrict cityDistrict = _context.CityDistrict.Where(c => c.Name.Equals(friendDTO.CityDistrict)).FirstOrDefault();
                            int cityDistrictId;
                            if (cityDistrict != null)
                            {
                                cityDistrictId = cityDistrict.IdCityDistrict;
                                newFriend.CityDistrictId = cityDistrictId;
                            }
                            else
                            {
                                throw new Exception("Не указан или указан в неверном формате населенный пункт входящий в МО Оренбург!");
                            }

                            int? streetId=null;
                            if (friendDTO.Street != null && !friendDTO.Street.Trim().Equals(""))
                            {
                                Street street = _context.Street.Where(s => s.Name.Equals(friendDTO.Street.Trim())).FirstOrDefault();
                                if (street != null)
                                {
                                    streetId = street.IdStreet;
                                    newFriend.StreetId = streetId;
                                }
                                else
                                {
                                    //throw new Exception("Не указана или не верно указана учица!");
                                }
                            }
                            else
                            {
                                //throw new Exception("Не указана или не верно указана учица!");
                            }

                            if (friendDTO.Microdistrict != null && !friendDTO.Microdistrict.Trim().Equals(""))
                            {
                                Microdistrict microdistrict = _context.Microdistrict.Where(md => md.Name.Equals(friendDTO.Microdistrict.Trim())).FirstOrDefault();
                                if (microdistrict != null)
                                {
                                    newFriend.MicroDistrictId = microdistrict.IdMicroDistrict;
                                }
                            }
                            House house = null;
                            if (streetId != null)
                            {
                                house = _context.House.FirstOrDefault(h => h.StreetId == streetId && h.Name.Equals(friendDTO.House.Trim()));
                            }

                            if (house != null)
                            {
                                newFriend.HouseId = house.IdHouse;
                                if (friendDTO.Apartment != null)
                                {
                                    newFriend.Apartment = friendDTO.Apartment.Trim();
                                }

                                PollingStation pollingStation = _context.PollingStation.FirstOrDefault(p => (p.CityDistrictId == cityDistrictId && p.StreetId == streetId && p.HouseId == house.IdHouse));
                                if (pollingStation != null)
                                {
                                    newFriend.StationId = pollingStation.StationId;
                                    District district = _context.District.FirstOrDefault(d => d.StationId == newFriend.StationId);

                                    if (district != null)
                                    {
                                        newFriend.ElectoralDistrictId = _context.District.FirstOrDefault(d => d.StationId == newFriend.StationId).ElectoralDistrictId;
                                    }
                                    else
                                    {
                                        throw new Exception("Не найден избирательный округ по указанному участку!");
                                    }
                                }
                                else if (friendDTO.PollingStationName != null && !friendDTO.PollingStationName.Trim().Equals(""))
                                {
                                    PollingStation pollingStationSearch = _context.PollingStation.FirstOrDefault(p => p.Name.Equals(friendDTO.PollingStationName.Trim()));
                                    //newFriend.PollingStationId = pollingStationSearch.IdPollingStation;
                                    
                                    if (pollingStationSearch != null)
                                    {
                                        newFriend.StationId = pollingStationSearch.StationId;
                                        District district = _context.District.FirstOrDefault(d => d.StationId == newFriend.StationId);

                                        if (district != null)
                                        {
                                            newFriend.ElectoralDistrictId = _context.District.FirstOrDefault(d => d.StationId == newFriend.StationId).ElectoralDistrictId;
                                        }
                                        else
                                        {
                                            throw new Exception("Не найден избирательный округ по указанному участку!");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Не верно указан избирательный участок!");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Не указан или не верно указан избирательный участок!");
                                }
                            }
                            else
                            {
                               // throw new Exception("Не указан или не верно указан дом!");
                            }

                        }
                        else
                        {
                            if (friendDTO.PollingStationName != null && !friendDTO.PollingStationName.Trim().Equals(""))
                            {
                                PollingStation pollingStationSearch = _context.PollingStation.FirstOrDefault(p => p.Name.Equals(friendDTO.PollingStationName.Trim()));

                                if (pollingStationSearch != null)
                                {
                                    newFriend.StationId = pollingStationSearch.StationId;

                                    District district = _context.District.Where(d => d.StationId == newFriend.StationId).FirstOrDefault();

                                    if (district != null)
                                    {
                                        newFriend.ElectoralDistrictId = district.ElectoralDistrictId;
                                    }
                                    else
                                    {
                                        throw new Exception("Не найден избирательный округ по указанному участку!");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Не верно указан избирательный участок!");
                                }
                            }
                            else
                            {
                                throw new Exception("Не указан адрес и неуказан/указан неверно избирательный участок!");
                            }
                        }
                    }

                    // Телефон избирателя, телефон ответственного, Email, QRcode
                    string telephone = friendDTO.Telephone;
                    if (telephone != null && !telephone.Trim().Equals(""))
                    {
                        string processedTelephone =ServicePhoneNumber.LeaveOnlyNumbers(friendDTO.Telephone.Trim());
                        if (regexTelephone.IsMatch(processedTelephone))
                        {
                            newFriend.Telephone = processedTelephone;
                        }
                        else
                        {
                            throw new Exception("Номер телефона избирателя указан в неверном формате!");
                        }
                    }
                    else
                    {
                        throw new Exception("Не указан номер телефона избирателя!");
                    }
                    if (friendDTO.PhoneNumberResponsible != null && !friendDTO.PhoneNumberResponsible.Equals(""))
                    {
                        string telephoneResponsible = friendDTO.PhoneNumberResponsible.Trim().Replace("-", "");
                        if (regexTelephone.IsMatch(telephoneResponsible))
                        {
                            newFriend.PhoneNumberResponsible = telephoneResponsible;
                        }
                    }
                    else
                    {
                        newFriend.PhoneNumberResponsible = userSave.Telephone;
                    }                    

                    if (friendDTO.Email != null && !friendDTO.Email.Trim().Equals(""))
                    {
                        newFriend.Email = friendDTO.Email.Trim();
                    }

                    if (friendDTO.TextQRcode != null && !friendDTO.TextQRcode.Trim().Equals(""))
                    {
                        newFriend.TextQRcode = friendDTO.TextQRcode.Trim();

                        // Генерация QR кода
                        newFriend.ByteQrcode = QRcodeServices.GenerateQRcodeFile(newFriend.FamilyName + " " + newFriend.Name + " " + newFriend.PatronymicName, newFriend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + newFriend.TextQRcode, "png", WayPathQrCodes);
                       // newFriend.Qrcode = fileNameQRcode;

                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////

                    // Отрасль, организация, группа
                    if (friendDTO.Organization != null && !friendDTO.Organization.Trim().Equals(""))
                    {
                        newFriend.Organization = friendDTO.Organization.Trim();
                        Organization organization = _context.Organization.FirstOrDefault(org => org.Name.Equals(friendDTO.Organization));
                        if (organization != null)
                        {
                            newFriend.OrganizationId = organization.IdOrganization;
                        }
                        else if(groupsUser[0].OrganizationId!=null)
                        {
                            newFriend.OrganizationId = groupsUser[0].OrganizationId;
                        }
                        else
                        {
                            throw new Exception("Не найдено структурное подразделение для указанной учетной записи (логина)!");
                        }
                    }
                    else if(groupsUser[0].OrganizationId != null)
                    {
                        newFriend.OrganizationId = groupsUser[0].OrganizationId;
                    }
                    else
                    {
                        throw new Exception("Не найдено структурное подразделение для указанной учетной записи (логина)!");
                    }
                    if (friendDTO.FieldActivityName != null && !friendDTO.FieldActivityName.Trim().Equals(""))
                    {
                        newFriend.FieldActivityId = _context.Fieldactivity.Where(f => f.Name.Equals(friendDTO.FieldActivityName.Trim())).FirstOrDefault().IdFieldActivity;
                    }
                    else if(groupsUser[0].FieldActivityId!=null)
                    {
                        newFriend.FieldActivityId = groupsUser[0].FieldActivityId;
                    }
                    else
                    {
                        throw new Exception("Не найдено структурное подразделение для указанной учетной записи (логина)!");
                    }
                    if (friendDTO.Group != null && !friendDTO.Group.Trim().Equals(""))
                    {
                        newFriend.GroupUId = _context.Groupu.Where(g => g.Name.Equals(friendDTO.Group.Trim())).FirstOrDefault().IdGroup;
                    }
                    else if(groupsUser[0]!=null)
                    {
                        newFriend.GroupUId = groupsUser[0].IdGroup;
                    }
                    else
                    {
                        throw new Exception("Не найдено структурное подразделение для указанной учетной записи (логина)!");
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    

                    // Дата голосования, Vote ? true : false
                    if (friendDTO.VotingDate != null && !friendDTO.VotingDate.Trim().Equals(""))
                    {
                        string[] datesV = friendDTO.VotingDate.Trim().Split('.');
                        DateTime datesVoting = new DateTime(Convert.ToInt32(datesV[2]), Convert.ToInt32(datesV[1]), Convert.ToInt32(datesV[0]));
                        newFriend.VotingDate = datesVoting;
                    }           
                    if (friendDTO.Vote != null && !friendDTO.Vote.Trim().Equals(""))
                    {
                        newFriend.Voter = friendDTO.Vote.ToLower().Trim().Equals("да") ? true : false;
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    newFriend.Description = friendDTO.Description;
                }
                else
                {
                    throw new Exception("Город не найден!");
                }
            }
            else
            {
                throw new Exception("Пользователь уже есть в списках!");
            }

            if (newFriend.StationId == null)
            {
                throw new Exception("Не удалось определить избирательный участок!");
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
