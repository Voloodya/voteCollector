SELECT `friend`.`Id_Friend`,
    `friend`.`Family_name`,
    `friend`.`Name_`,
    `friend`.`Patronymic_name`,
    `friend`.`Date_birth`,
    `friend`.`Unpinning`,
	`City`.`Name` as 'Город',
    `city_district`.`Name` as 'Округ',
    `electoral_district`.`Name` as 'Избират. Округ',
    `street`.`Name` as 'Улица',
    `House`.`Name` as 'Дом',
    `friend`.`Apartment`,
    `friend`.`Telephone`,
    `station`.`Name` as 'Участок',
	`fieldactivity`.`Name` as 'Организация',
    `organization`.`Name` as 'Подразделение',
	`groupu`.`Name` as 'Подвед',
    `friend`.`Phone_number_responsible`,
    `friend`.`Date_registration_site`,
    `friend`.`Voting_date`,
    `friend`.`Voter`,
    `friend`.`Adress`,
    `friend`.`TextQRcode`,
    `friend`.`Email`,
    `friend`.`Description`,
    `user`.`Family_name`,
    `user`.`Name_`,
    `user`.`Patronymic_name`,
    `user`.`Telephone`,
    `friend_status`.`Name` 
    FROM friend Inner Join city_district ON friend.City_id=city_district.Id_CityDistrict
	left join City ON City.Id_City=friend.City_id
    left Join street ON street.Id_Street=friend.Street_id
    left Join House ON house.Id_House=friend.House_id
    left Join station ON station.Id_Station=friend.Station_id
    left Join electoral_district on electoral_district.Id_ElectoralDistrict=friend.Electoral_district_id
    left join organization ON organization.Id_Organization=friend.Organization_id
    left Join fieldactivity ON fieldactivity.Id_FieldActivity=friend.FieldActivity_id
    left Join groupu ON groupu.Id_Group=friend.GroupU_id
    left Join user ON user.Id_User=friend.User_id
    left join friend_status ON friend_status.Id_friend_status=friend.Station_id    
    where friend.FieldActivity_id=4;