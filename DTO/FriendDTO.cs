﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectVoters.DTO
{
    public class FriendDTO
    {
        public long IdFriend { get; set; }
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string PatronymicName { get; set; }
        public string DateBirth { get; set; }
        public string CityName { get; set; }
        public string Street { get; set; }
        public string Microdistrict { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string Telephone { get; set; }
        public string DistrictName { get; set; }
        public string PollingStationName { get; set; }
        public string Organization { get; set; }
        public string FieldActivityName { get; set; }
        public string PhoneNumberResponsible { get; set; }
        public string DateRegistrationSite { get; set; }
        public string VotingDate { get; set; }
        public string Vote { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public long? UserId { get; set; }

    }
}
