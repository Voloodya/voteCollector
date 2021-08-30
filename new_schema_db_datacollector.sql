CREATE TABLE `role` (
  `Id_Role` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id_Role`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `user` (
  `Id_User` bigint NOT NULL AUTO_INCREMENT,
  `UserName` varchar(100) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `Role_id` int DEFAULT NULL,
  `Family_name` varchar(256) DEFAULT NULL,
  `Name_` varchar(256) DEFAULT NULL,
  `Patronymic_name` varchar(256) DEFAULT NULL,
  `Date_birth` date DEFAULT NULL,
  `Telephone` varchar(12) DEFAULT NULL,
  PRIMARY KEY (`Id_User`),
  KEY `FK_User_Role` (`Role_id`),
  CONSTRAINT `FK_User_Role` FOREIGN KEY (`Role_id`) REFERENCES `role` (`Id_Role`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=622 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `fieldactivity` (
  `Id_FieldActivity` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id_FieldActivity`)
) ENGINE=InnoDB AUTO_INCREMENT=1539 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `organization` (
  `Id_Organization` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id_Organization`)
) ENGINE=InnoDB AUTO_INCREMENT=124 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `groupu` (
  `Id_Group` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(500) DEFAULT NULL,
  `FieldActivity_id` int DEFAULT NULL,
  `Organization_id` int DEFAULT NULL,
  `CreatorGroup` varchar(256) DEFAULT NULL,
  `Level` int DEFAULT NULL,
  `Group_Parents_id` int DEFAULT NULL,
  `User_Responsible_id` bigint DEFAULT NULL,
  `NumberEmployees` int DEFAULT NULL,
  PRIMARY KEY (`Id_Group`),
  KEY `FK_Groupu_Fieldactivity` (`FieldActivity_id`),
  KEY `FK_Groupu_Organization` (`Organization_id`),
  KEY `FK_Groupu_Groupu` (`Group_Parents_id`),
  KEY `FK_Groupu_User` (`User_Responsible_id`),
  CONSTRAINT `FK_Groupu_Fieldactivity` FOREIGN KEY (`FieldActivity_id`) REFERENCES `fieldactivity` (`Id_FieldActivity`),
  CONSTRAINT `FK_Groupu_Groupu` FOREIGN KEY (`Group_Parents_id`) REFERENCES `groupu` (`Id_Group`),
  CONSTRAINT `FK_Groupu_Organization` FOREIGN KEY (`Organization_id`) REFERENCES `organization` (`Id_Organization`),
  CONSTRAINT `FK_Groupu_User` FOREIGN KEY (`User_Responsible_id`) REFERENCES `user` (`Id_User`)
) ENGINE=InnoDB AUTO_INCREMENT=561 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `groupsusers` (
  `Id_GroupsUsers` bigint NOT NULL AUTO_INCREMENT,
  `GroupU_id` int DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `User_id` bigint DEFAULT NULL,
  PRIMARY KEY (`Id_GroupsUsers`),
  KEY `FK_GroupsUsers_GroupU` (`GroupU_id`),
  KEY `FK_GroupsUsers_User` (`User_id`),
  CONSTRAINT `FK_GroupsUsers_GroupU` FOREIGN KEY (`GroupU_id`) REFERENCES `groupu` (`Id_Group`),
  CONSTRAINT `FK_GroupsUsers_User` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id_User`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1304 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `city` (
  `Id_City` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id_City`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `city_district` (
  `Id_CityDistrict` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int DEFAULT NULL,
  PRIMARY KEY (`Id_CityDistrict`),
  KEY `FK_CityDistrict_City` (`City_id`),
  CONSTRAINT `FK_CityDistrict_City` FOREIGN KEY (`City_id`) REFERENCES `city` (`Id_City`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `electoral_district` (
  `Id_ElectoralDistrict` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id_ElectoralDistrict`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `street` (
  `Id_Street` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int DEFAULT NULL,
  PRIMARY KEY (`Id_Street`),
  KEY `FK_Street_City` (`City_id`),
  CONSTRAINT `FK_Street_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1604 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `microdistrict` (
  `Id_MicroDistrict` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int DEFAULT NULL,
  PRIMARY KEY (`Id_MicroDistrict`),
  KEY `FK_MicroDistrict_City` (`City_id`),
  CONSTRAINT `FK_MicroDistrict_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `house` (
  `Id_House` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  `Street_id` int DEFAULT NULL,
  `MicroDistrict_id` int DEFAULT NULL,
  `City_id` int DEFAULT NULL,
  PRIMARY KEY (`Id_House`),
  KEY `FK_House_Street` (`Street_id`),
  KEY `FK_House_MicroDistrict` (`MicroDistrict_id`),
  KEY `FK_House_City_idx` (`City_id`),
  CONSTRAINT `FK_House_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`),
  CONSTRAINT `FK_House_MicroDistrict` FOREIGN KEY (`MicroDistrict_id`) REFERENCES `microdistrict` (`Id_MicroDistrict`),
  CONSTRAINT `FK_House_Street` FOREIGN KEY (`Street_id`) REFERENCES `street` (`Id_Street`)
) ENGINE=InnoDB AUTO_INCREMENT=45575 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE `station` (
  `Id_Station` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id_Station`)
) ENGINE=InnoDB AUTO_INCREMENT=216 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `district` (
  `Id_District` int NOT NULL AUTO_INCREMENT,
  `Electoral_District_id` int DEFAULT NULL,
  `Station_id` int DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int DEFAULT NULL,
  `Street_id` int DEFAULT NULL,
  PRIMARY KEY (`Id_District`),
  KEY `FK_District_City` (`City_id`),
  KEY `FK_District_Electoral_District` (`Electoral_District_id`),
  KEY `FK_District_Street` (`Street_id`),
  KEY `FK_District_Station` (`Station_id`),
  CONSTRAINT `FK_District_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_District_Electoral_District` FOREIGN KEY (`Electoral_District_id`) REFERENCES `electoral_district` (`Id_ElectoralDistrict`),
  CONSTRAINT `FK_District_Station` FOREIGN KEY (`Station_id`) REFERENCES `station` (`Id_Station`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_District_Street` FOREIGN KEY (`Street_id`) REFERENCES `street` (`Id_Street`)
) ENGINE=InnoDB AUTO_INCREMENT=215 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `polling_station` (
  `Id_Polling_station` int NOT NULL AUTO_INCREMENT,
  `Station_id` int DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int DEFAULT NULL,
  `Street_id` int DEFAULT NULL,
  `MicroDistrict_id` int DEFAULT NULL,
  `House_id` int DEFAULT NULL,
  PRIMARY KEY (`Id_Polling_station`),
  KEY `FK_PollingStation_City` (`City_id`),
  KEY `FK_PollingStation_Street` (`Street_id`),
  KEY `FK_PollingStation_MicroDistrict` (`MicroDistrict_id`),
  KEY `FK_PollingStation_House` (`House_id`),
  KEY `FK_PollingStation_Station_idx` (`Station_id`),
  CONSTRAINT `FK_PollingStation_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_PollingStation_House` FOREIGN KEY (`House_id`) REFERENCES `house` (`Id_House`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_PollingStation_MicroDistrict` FOREIGN KEY (`MicroDistrict_id`) REFERENCES `microdistrict` (`Id_MicroDistrict`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_PollingStation_Station` FOREIGN KEY (`Station_id`) REFERENCES `station` (`Id_Station`),
  CONSTRAINT `FK_PollingStation_Street` FOREIGN KEY (`Street_id`) REFERENCES `street` (`Id_Street`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=115412 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `friend_status` (
  `Id_friend_status` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id_friend_status`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `friend` (
  `Id_Friend` bigint NOT NULL AUTO_INCREMENT,
  `Family_name` varchar(256) DEFAULT NULL,
  `Name_` varchar(256) DEFAULT NULL,
  `Patronymic_name` varchar(256) DEFAULT NULL,
  `Date_birth` date DEFAULT NULL,
  `Unpinning` tinyint DEFAULT '0',
  `City_id` int DEFAULT NULL,
  `ByteQRcode` mediumblob,
  `TypeImage` varchar(45) DEFAULT NULL,
  `CityDistrict_id` int DEFAULT NULL,
  `Electoral_district_id` int DEFAULT NULL,
  `Street_id` int DEFAULT NULL,
  `MicroDistrict_id` int DEFAULT NULL,
  `House_id` int DEFAULT NULL,
  `Building` varchar(10) DEFAULT NULL,
  `Apartment` varchar(10) DEFAULT NULL,
  `Telephone` varchar(12) DEFAULT NULL,
  `Station_id` int DEFAULT NULL,
  `Organization` varchar(256) DEFAULT NULL,
  `Organization_id` int DEFAULT NULL,
  `FieldActivity_id` int DEFAULT NULL,
  `Phone_number_responsible` varchar(12) DEFAULT NULL,
  `Date_registration_site` date DEFAULT NULL,
  `Voting_date` date DEFAULT NULL,
  `Voter` tinyint DEFAULT '0',
  `Adress` varchar(500) DEFAULT NULL,
  `TextQRcode` varchar(256) DEFAULT NULL,
  `QRcode` varchar(4500) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `Description` varchar(256) DEFAULT NULL,
  `User_id` bigint DEFAULT NULL,
  `GroupU_id` int DEFAULT NULL,
  `FriendStatus_id` int DEFAULT NULL,
  PRIMARY KEY (`Id_Friend`),
  KEY `FK_Friend_Street` (`Street_id`),
  KEY `FK_Friend_MicroDistrict` (`MicroDistrict_id`),
  KEY `FK_Friend_House` (`House_id`),
  KEY `FK_Friend_FieldActivity` (`FieldActivity_id`),
  KEY `FK_Friend_User` (`User_id`),
  KEY `FK_Friend_GroupU` (`GroupU_id`),
  KEY `FK_Friend_ElectoralDistrict` (`Electoral_district_id`),
  KEY `FK_Friend_Station_idx` (`Station_id`),
  KEY `FK_Friend_FriendStatus` (`FriendStatus_id`),
  KEY `FK_Friend_Organization` (`Organization_id`),
  KEY `FK_Friend_CityDistrict` (`CityDistrict_id`),
  KEY `FK_Friend_City` (`City_id`),
  CONSTRAINT `FK_Friend_City` FOREIGN KEY (`City_id`) REFERENCES `city` (`Id_City`),
  CONSTRAINT `FK_Friend_CityDistrict` FOREIGN KEY (`CityDistrict_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL,
  CONSTRAINT `FK_Friend_ElectoralDistrict` FOREIGN KEY (`Electoral_district_id`) REFERENCES `electoral_district` (`Id_ElectoralDistrict`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_Friend_FieldActivity` FOREIGN KEY (`FieldActivity_id`) REFERENCES `fieldactivity` (`Id_FieldActivity`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_Friend_FriendStatus` FOREIGN KEY (`FriendStatus_id`) REFERENCES `friend_status` (`Id_friend_status`),
  CONSTRAINT `FK_Friend_GroupU` FOREIGN KEY (`GroupU_id`) REFERENCES `groupu` (`Id_Group`),
  CONSTRAINT `FK_Friend_House` FOREIGN KEY (`House_id`) REFERENCES `house` (`Id_House`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_Friend_MicroDistrict` FOREIGN KEY (`MicroDistrict_id`) REFERENCES `microdistrict` (`Id_MicroDistrict`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_Friend_Organization` FOREIGN KEY (`Organization_id`) REFERENCES `organization` (`Id_Organization`),
  CONSTRAINT `FK_Friend_Station` FOREIGN KEY (`Station_id`) REFERENCES `station` (`Id_Station`) ON DELETE SET NULL,
  CONSTRAINT `FK_Friend_Street` FOREIGN KEY (`Street_id`) REFERENCES `street` (`Id_Street`) ON DELETE SET NULL ON UPDATE RESTRICT,
  CONSTRAINT `FK_Friend_User` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id_User`) ON DELETE SET NULL ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7832 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;













