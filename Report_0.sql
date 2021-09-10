SELECT `friend`.`Id_Friend`,
    `friend`.`Family_name`,
    `friend`.`Name_`,
    `friend`.`Patronymic_name`,
    `friend`.`Date_birth`,
    `friend`.`Unpinning` AS `Откреплен`,
	`City`.`Name` AS `Город`,
    `city_district`.`Name` AS `Городской округ`,
    `electoral_district`.`Name` AS `Избират. округ`,
    `street`.`Name` AS `Улица`,
    `House`.`Name` AS `Дом`,
    `friend`.`Apartment` AS `Квартира`,
    `friend`.`Telephone`,
    `station`.`Name` AS `Участок`,
	`fieldactivity`.`Name` AS `Организация`,
    `organization`.`Name` AS `Подразделение`,
	`groupu`.`Name` AS `Подведом. учрежд.`,
    `friend`.`Phone_number_responsible`,
    `friend`.`Date_registration_site`,
    `friend`.`Voting_date`,
    `friend`.`Voter`,
    `friend`.`Adress`,
    `friend`.`TextQRcode` AS `Текст QR кода`,
    `friend`.`Email`,
    `friend`.`Description` AS `Примечание`,
    `user`.`Family_name` AS `Фамилия ответственного`,
    `user`.`Name_` AS `Имя ответственного`,
    `user`.`Patronymic_name` AS `Отчество ответственного`,
    `user`.`Telephone` AS `Тел. ответственного`,
    `friend_status`.`Name` AS `Статус участника`
    FROM friend left join city_district ON friend.CityDistrict_id=city_district.Id_CityDistrict
	left join City ON City.Id_City=friend.City_id
    left Join street ON street.Id_Street=friend.Street_id
    left Join House ON house.Id_House=friend.House_id
    left Join station ON station.Id_Station=friend.Station_id
    left Join electoral_district on electoral_district.Id_ElectoralDistrict=friend.Electoral_district_id
    left join organization ON organization.Id_Organization=friend.Organization_id
    left Join fieldactivity ON fieldactivity.Id_FieldActivity=friend.FieldActivity_id
    left Join groupu ON groupu.Id_Group=friend.GroupU_id
    left Join user ON user.Id_User=friend.User_id
    left join friend_status ON friend_status.Id_friend_status=friend.FriendStatus_id;
	
	u1451597_root
	@volodyaadmin001