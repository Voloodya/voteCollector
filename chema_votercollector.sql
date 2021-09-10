-- phpMyAdmin SQL Dump
-- version 5.0.3
-- https://www.phpmyadmin.net/
--
-- Хост: localhost:3306
-- Время создания: Сен 10 2021 г., 09:38
-- Версия сервера: 10.5.10-MariaDB-log
-- Версия PHP: 7.4.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `u1451597_votercollector`
--

-- --------------------------------------------------------

--
-- Структура таблицы `city`
--

CREATE TABLE `city` (
  `Id_City` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `city_district`
--

CREATE TABLE `city_district` (
  `Id_CityDistrict` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `district`
--

CREATE TABLE `district` (
  `Id_District` int(11) NOT NULL,
  `Electoral_District_id` int(11) DEFAULT NULL,
  `Electoral_district_gov_duma_id` int(11) DEFAULT NULL,
  `Electoral_district_assembly_law_id` int(11) DEFAULT NULL,
  `Station_id` int(11) DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `electoral_district`
--

CREATE TABLE `electoral_district` (
  `Id_ElectoralDistrict` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `electoral_district_assembly_law`
--

CREATE TABLE `electoral_district_assembly_law` (
  `Id_electoral_district_assembly_law` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `electoral_district_gov_duma`
--

CREATE TABLE `electoral_district_gov_duma` (
  `Id_electoral_district_gov_duma` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `fieldactivity`
--

CREATE TABLE `fieldactivity` (
  `Id_FieldActivity` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `friend`
--

CREATE TABLE `friend` (
  `Id_Friend` bigint(20) NOT NULL,
  `Family_name` varchar(256) DEFAULT NULL,
  `Name_` varchar(256) DEFAULT NULL,
  `Patronymic_name` varchar(256) DEFAULT NULL,
  `Date_birth` date DEFAULT NULL,
  `Unpinning` tinyint(4) DEFAULT 0,
  `City_id` int(11) DEFAULT NULL,
  `ByteQRcode` mediumblob DEFAULT NULL,
  `TypeImage` varchar(45) DEFAULT NULL,
  `CityDistrict_id` int(11) DEFAULT NULL,
  `Electoral_district_id` int(11) DEFAULT NULL,
  `Street_id` int(11) DEFAULT NULL,
  `MicroDistrict_id` int(11) DEFAULT NULL,
  `House_id` int(11) DEFAULT NULL,
  `Building` varchar(10) DEFAULT NULL,
  `Apartment` varchar(10) DEFAULT NULL,
  `Telephone` varchar(12) DEFAULT NULL,
  `Station_id` int(11) DEFAULT NULL,
  `Organization` varchar(256) DEFAULT NULL,
  `Organization_id` int(11) DEFAULT NULL,
  `FieldActivity_id` int(11) DEFAULT NULL,
  `Phone_number_responsible` varchar(12) DEFAULT NULL,
  `Date_registration_site` date DEFAULT NULL,
  `Voting_date` datetime DEFAULT NULL,
  `Voter` tinyint(4) DEFAULT 0,
  `Adress` varchar(500) DEFAULT NULL,
  `TextQRcode` varchar(256) DEFAULT NULL,
  `QRcode` varchar(4500) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `Description` varchar(256) DEFAULT NULL,
  `User_id` bigint(20) DEFAULT NULL,
  `GroupU_id` int(11) DEFAULT NULL,
  `FriendStatus_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `friend_status`
--

CREATE TABLE `friend_status` (
  `Id_friend_status` int(11) NOT NULL,
  `Name` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `groupsusers`
--

CREATE TABLE `groupsusers` (
  `Id_GroupsUsers` bigint(20) NOT NULL,
  `GroupU_id` int(11) DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `User_id` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `groupu`
--

CREATE TABLE `groupu` (
  `Id_Group` int(11) NOT NULL,
  `Name` varchar(500) DEFAULT NULL,
  `FieldActivity_id` int(11) DEFAULT NULL,
  `Organization_id` int(11) DEFAULT NULL,
  `CreatorGroup` varchar(256) DEFAULT NULL,
  `Level` int(11) DEFAULT NULL,
  `Group_Parents_id` int(11) DEFAULT NULL,
  `User_Responsible_id` bigint(20) DEFAULT NULL,
  `NumberEmployees` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `house`
--

CREATE TABLE `house` (
  `Id_House` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `Street_id` int(11) DEFAULT NULL,
  `MicroDistrict_id` int(11) DEFAULT NULL,
  `City_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `microdistrict`
--

CREATE TABLE `microdistrict` (
  `Id_MicroDistrict` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `organization`
--

CREATE TABLE `organization` (
  `Id_Organization` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `polling_station`
--

CREATE TABLE `polling_station` (
  `Id_Polling_station` int(11) NOT NULL,
  `Station_id` int(11) DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int(11) DEFAULT NULL,
  `Street_id` int(11) DEFAULT NULL,
  `MicroDistrict_id` int(11) DEFAULT NULL,
  `House_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `role`
--

CREATE TABLE `role` (
  `Id_Role` int(11) NOT NULL,
  `Name` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `station`
--

CREATE TABLE `station` (
  `Id_Station` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `street`
--

CREATE TABLE `street` (
  `Id_Street` int(11) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `City_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `user`
--

CREATE TABLE `user` (
  `Id_User` bigint(20) NOT NULL,
  `UserName` varchar(100) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `Role_id` int(11) DEFAULT NULL,
  `Family_name` varchar(256) DEFAULT NULL,
  `Name_` varchar(256) DEFAULT NULL,
  `Patronymic_name` varchar(256) DEFAULT NULL,
  `Date_birth` date DEFAULT NULL,
  `Telephone` varchar(12) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `city`
--
ALTER TABLE `city`
  ADD PRIMARY KEY (`Id_City`);

--
-- Индексы таблицы `city_district`
--
ALTER TABLE `city_district`
  ADD PRIMARY KEY (`Id_CityDistrict`),
  ADD KEY `FK_CityDistrict_City` (`City_id`);

--
-- Индексы таблицы `district`
--
ALTER TABLE `district`
  ADD PRIMARY KEY (`Id_District`),
  ADD KEY `FK_District_City` (`City_id`),
  ADD KEY `FK_District_Electoral_District` (`Electoral_District_id`),
  ADD KEY `FK_District_Station` (`Station_id`),
  ADD KEY `FK_District_ElectoralDistrictGovDuma` (`Electoral_district_gov_duma_id`),
  ADD KEY `FK_District_ElectoralDistrictAssemblyLaw` (`Electoral_district_assembly_law_id`);

--
-- Индексы таблицы `electoral_district`
--
ALTER TABLE `electoral_district`
  ADD PRIMARY KEY (`Id_ElectoralDistrict`);

--
-- Индексы таблицы `electoral_district_assembly_law`
--
ALTER TABLE `electoral_district_assembly_law`
  ADD PRIMARY KEY (`Id_electoral_district_assembly_law`);

--
-- Индексы таблицы `electoral_district_gov_duma`
--
ALTER TABLE `electoral_district_gov_duma`
  ADD PRIMARY KEY (`Id_electoral_district_gov_duma`);

--
-- Индексы таблицы `fieldactivity`
--
ALTER TABLE `fieldactivity`
  ADD PRIMARY KEY (`Id_FieldActivity`);

--
-- Индексы таблицы `friend`
--
ALTER TABLE `friend`
  ADD PRIMARY KEY (`Id_Friend`),
  ADD KEY `FK_Friend_Street` (`Street_id`),
  ADD KEY `FK_Friend_MicroDistrict` (`MicroDistrict_id`),
  ADD KEY `FK_Friend_House` (`House_id`),
  ADD KEY `FK_Friend_FieldActivity` (`FieldActivity_id`),
  ADD KEY `FK_Friend_User` (`User_id`),
  ADD KEY `FK_Friend_GroupU` (`GroupU_id`),
  ADD KEY `FK_Friend_ElectoralDistrict` (`Electoral_district_id`),
  ADD KEY `FK_Friend_Station_idx` (`Station_id`),
  ADD KEY `FK_Friend_FriendStatus` (`FriendStatus_id`),
  ADD KEY `FK_Friend_Organization` (`Organization_id`),
  ADD KEY `FK_Friend_CityDistrict` (`CityDistrict_id`),
  ADD KEY `FK_Friend_City` (`City_id`);

--
-- Индексы таблицы `friend_status`
--
ALTER TABLE `friend_status`
  ADD PRIMARY KEY (`Id_friend_status`);

--
-- Индексы таблицы `groupsusers`
--
ALTER TABLE `groupsusers`
  ADD PRIMARY KEY (`Id_GroupsUsers`),
  ADD KEY `FK_GroupsUsers_GroupU` (`GroupU_id`),
  ADD KEY `FK_GroupsUsers_User` (`User_id`);

--
-- Индексы таблицы `groupu`
--
ALTER TABLE `groupu`
  ADD PRIMARY KEY (`Id_Group`),
  ADD KEY `FK_Groupu_Fieldactivity` (`FieldActivity_id`),
  ADD KEY `FK_Groupu_Organization` (`Organization_id`),
  ADD KEY `FK_Groupu_Groupu` (`Group_Parents_id`),
  ADD KEY `FK_Groupu_User` (`User_Responsible_id`);

--
-- Индексы таблицы `house`
--
ALTER TABLE `house`
  ADD PRIMARY KEY (`Id_House`),
  ADD KEY `FK_House_Street` (`Street_id`),
  ADD KEY `FK_House_MicroDistrict` (`MicroDistrict_id`),
  ADD KEY `FK_House_City_idx` (`City_id`);

--
-- Индексы таблицы `microdistrict`
--
ALTER TABLE `microdistrict`
  ADD PRIMARY KEY (`Id_MicroDistrict`),
  ADD KEY `FK_MicroDistrict_City` (`City_id`);

--
-- Индексы таблицы `organization`
--
ALTER TABLE `organization`
  ADD PRIMARY KEY (`Id_Organization`);

--
-- Индексы таблицы `polling_station`
--
ALTER TABLE `polling_station`
  ADD PRIMARY KEY (`Id_Polling_station`),
  ADD KEY `FK_PollingStation_City` (`City_id`),
  ADD KEY `FK_PollingStation_Street` (`Street_id`),
  ADD KEY `FK_PollingStation_MicroDistrict` (`MicroDistrict_id`),
  ADD KEY `FK_PollingStation_House` (`House_id`),
  ADD KEY `FK_PollingStation_Station_idx` (`Station_id`);

--
-- Индексы таблицы `role`
--
ALTER TABLE `role`
  ADD PRIMARY KEY (`Id_Role`);

--
-- Индексы таблицы `station`
--
ALTER TABLE `station`
  ADD PRIMARY KEY (`Id_Station`);

--
-- Индексы таблицы `street`
--
ALTER TABLE `street`
  ADD PRIMARY KEY (`Id_Street`),
  ADD KEY `FK_Street_City` (`City_id`);

--
-- Индексы таблицы `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`Id_User`),
  ADD KEY `FK_User_Role` (`Role_id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `city`
--
ALTER TABLE `city`
  MODIFY `Id_City` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `city_district`
--
ALTER TABLE `city_district`
  MODIFY `Id_CityDistrict` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `district`
--
ALTER TABLE `district`
  MODIFY `Id_District` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `electoral_district`
--
ALTER TABLE `electoral_district`
  MODIFY `Id_ElectoralDistrict` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `electoral_district_assembly_law`
--
ALTER TABLE `electoral_district_assembly_law`
  MODIFY `Id_electoral_district_assembly_law` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `electoral_district_gov_duma`
--
ALTER TABLE `electoral_district_gov_duma`
  MODIFY `Id_electoral_district_gov_duma` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `fieldactivity`
--
ALTER TABLE `fieldactivity`
  MODIFY `Id_FieldActivity` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `friend`
--
ALTER TABLE `friend`
  MODIFY `Id_Friend` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `friend_status`
--
ALTER TABLE `friend_status`
  MODIFY `Id_friend_status` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `groupsusers`
--
ALTER TABLE `groupsusers`
  MODIFY `Id_GroupsUsers` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `groupu`
--
ALTER TABLE `groupu`
  MODIFY `Id_Group` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `house`
--
ALTER TABLE `house`
  MODIFY `Id_House` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `microdistrict`
--
ALTER TABLE `microdistrict`
  MODIFY `Id_MicroDistrict` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `organization`
--
ALTER TABLE `organization`
  MODIFY `Id_Organization` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `polling_station`
--
ALTER TABLE `polling_station`
  MODIFY `Id_Polling_station` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `role`
--
ALTER TABLE `role`
  MODIFY `Id_Role` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `station`
--
ALTER TABLE `station`
  MODIFY `Id_Station` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `street`
--
ALTER TABLE `street`
  MODIFY `Id_Street` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `user`
--
ALTER TABLE `user`
  MODIFY `Id_User` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `city_district`
--
ALTER TABLE `city_district`
  ADD CONSTRAINT `FK_CityDistrict_City` FOREIGN KEY (`City_id`) REFERENCES `city` (`Id_City`);

--
-- Ограничения внешнего ключа таблицы `district`
--
ALTER TABLE `district`
  ADD CONSTRAINT `FK_District_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_District_ElectoralDistrictAssemblyLaw` FOREIGN KEY (`Electoral_district_assembly_law_id`) REFERENCES `electoral_district_assembly_law` (`Id_electoral_district_assembly_law`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_District_ElectoralDistrictGovDuma` FOREIGN KEY (`Electoral_district_gov_duma_id`) REFERENCES `electoral_district_gov_duma` (`Id_electoral_district_gov_duma`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_District_Electoral_District` FOREIGN KEY (`Electoral_District_id`) REFERENCES `electoral_district` (`Id_ElectoralDistrict`),
  ADD CONSTRAINT `FK_District_Station` FOREIGN KEY (`Station_id`) REFERENCES `station` (`Id_Station`) ON DELETE SET NULL;

--
-- Ограничения внешнего ключа таблицы `friend`
--
ALTER TABLE `friend`
  ADD CONSTRAINT `FK_Friend_City` FOREIGN KEY (`City_id`) REFERENCES `city` (`Id_City`),
  ADD CONSTRAINT `FK_Friend_CityDistrict` FOREIGN KEY (`CityDistrict_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Friend_ElectoralDistrict` FOREIGN KEY (`Electoral_district_id`) REFERENCES `electoral_district` (`Id_ElectoralDistrict`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Friend_FieldActivity` FOREIGN KEY (`FieldActivity_id`) REFERENCES `fieldactivity` (`Id_FieldActivity`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Friend_FriendStatus` FOREIGN KEY (`FriendStatus_id`) REFERENCES `friend_status` (`Id_friend_status`),
  ADD CONSTRAINT `FK_Friend_GroupU` FOREIGN KEY (`GroupU_id`) REFERENCES `groupu` (`Id_Group`),
  ADD CONSTRAINT `FK_Friend_House` FOREIGN KEY (`House_id`) REFERENCES `house` (`Id_House`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Friend_MicroDistrict` FOREIGN KEY (`MicroDistrict_id`) REFERENCES `microdistrict` (`Id_MicroDistrict`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Friend_Organization` FOREIGN KEY (`Organization_id`) REFERENCES `organization` (`Id_Organization`),
  ADD CONSTRAINT `FK_Friend_Station` FOREIGN KEY (`Station_id`) REFERENCES `station` (`Id_Station`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Friend_Street` FOREIGN KEY (`Street_id`) REFERENCES `street` (`Id_Street`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_Friend_User` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id_User`) ON DELETE SET NULL;

--
-- Ограничения внешнего ключа таблицы `groupsusers`
--
ALTER TABLE `groupsusers`
  ADD CONSTRAINT `FK_GroupsUsers_GroupU` FOREIGN KEY (`GroupU_id`) REFERENCES `groupu` (`Id_Group`),
  ADD CONSTRAINT `FK_GroupsUsers_User` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id_User`) ON DELETE SET NULL;

--
-- Ограничения внешнего ключа таблицы `groupu`
--
ALTER TABLE `groupu`
  ADD CONSTRAINT `FK_Groupu_Fieldactivity` FOREIGN KEY (`FieldActivity_id`) REFERENCES `fieldactivity` (`Id_FieldActivity`),
  ADD CONSTRAINT `FK_Groupu_Groupu` FOREIGN KEY (`Group_Parents_id`) REFERENCES `groupu` (`Id_Group`),
  ADD CONSTRAINT `FK_Groupu_Organization` FOREIGN KEY (`Organization_id`) REFERENCES `organization` (`Id_Organization`),
  ADD CONSTRAINT `FK_Groupu_User` FOREIGN KEY (`User_Responsible_id`) REFERENCES `user` (`Id_User`);

--
-- Ограничения внешнего ключа таблицы `house`
--
ALTER TABLE `house`
  ADD CONSTRAINT `FK_House_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`),
  ADD CONSTRAINT `FK_House_MicroDistrict` FOREIGN KEY (`MicroDistrict_id`) REFERENCES `microdistrict` (`Id_MicroDistrict`),
  ADD CONSTRAINT `FK_House_Street` FOREIGN KEY (`Street_id`) REFERENCES `street` (`Id_Street`);

--
-- Ограничения внешнего ключа таблицы `microdistrict`
--
ALTER TABLE `microdistrict`
  ADD CONSTRAINT `FK_MicroDistrict_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL;

--
-- Ограничения внешнего ключа таблицы `polling_station`
--
ALTER TABLE `polling_station`
  ADD CONSTRAINT `FK_PollingStation_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_PollingStation_House` FOREIGN KEY (`House_id`) REFERENCES `house` (`Id_House`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_PollingStation_MicroDistrict` FOREIGN KEY (`MicroDistrict_id`) REFERENCES `microdistrict` (`Id_MicroDistrict`) ON DELETE SET NULL,
  ADD CONSTRAINT `FK_PollingStation_Station` FOREIGN KEY (`Station_id`) REFERENCES `station` (`Id_Station`),
  ADD CONSTRAINT `FK_PollingStation_Street` FOREIGN KEY (`Street_id`) REFERENCES `street` (`Id_Street`) ON DELETE SET NULL;

--
-- Ограничения внешнего ключа таблицы `street`
--
ALTER TABLE `street`
  ADD CONSTRAINT `FK_Street_City` FOREIGN KEY (`City_id`) REFERENCES `city_district` (`Id_CityDistrict`) ON DELETE SET NULL;

--
-- Ограничения внешнего ключа таблицы `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `FK_User_Role` FOREIGN KEY (`Role_id`) REFERENCES `role` (`Id_Role`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
