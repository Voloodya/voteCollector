CREATE database voterCollector;
USE voterCollector;

create table Role(
					Id_Role INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					Name VARCHAR(100) NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table GroupU(
					Id_Group INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                    Name VARCHAR(256) null
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table User(
                      Id_User BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      UserName VARCHAR(100) NOT NULL,
                      Password VARCHAR(100) NOT NULL,
                      Role_id INT default null,
                      CONSTRAINT FK_User_Role FOREIGN KEY (Role_id) REFERENCES Role (Id_Role)
                      ON delete set null on update restrict,
                      Family_name VARCHAR(256) null,
                      Name_ VARCHAR(256) null,
                      Patronymic_name VARCHAR(256) NULL,
                      Date_birth date NULL,
                      Telephone VARCHAR(12) null
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table GroupsUsers(
					Id_GroupsUsers BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					GroupU_id INT default null,
                    CONSTRAINT FK_GroupsUsers_GroupU FOREIGN KEY (GroupU_id) REFERENCES GroupU (Id_Group),
                    Name VARCHAR(256) null,
                    User_id BIGINT default null,
					CONSTRAINT FK_GroupsUsers_User FOREIGN KEY (User_id) REFERENCES User (Id_User)
					ON delete set null on update restrict                    
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table City(
                      Id_City INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      Name VARCHAR(256) NULL                      
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table Electoral_District(
					Id_ElectoralDistrict INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					Name VARCHAR(256) NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table District(
					Id_District INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					Name VARCHAR(256) NULL,
                    City_id INT default null,
					CONSTRAINT FK_District_City FOREIGN KEY (City_id) REFERENCES City (Id_City)
                    ON delete set null on update restrict
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

ALTER TABLE `votercollector`.`district` 
ADD COLUMN `Electoral_District_id` INT NULL DEFAULT NULL AFTER `Id_District`;

ALTER TABLE `votercollector`.`district` 
ADD INDEX `FK_District_Electoral_District` (`Electoral_District_id` ASC) VISIBLE;
;
ALTER TABLE `votercollector`.`district` 
ADD CONSTRAINT `FK_District_Electoral_District`
  FOREIGN KEY (`Electoral_District_id`)
  REFERENCES `votercollector`.`electoral_district` (`Id_ElectoralDistrict`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
  
  ALTER TABLE `votercollector`.`district` 
ADD COLUMN `Street_id` INT NULL DEFAULT NULL AFTER `City_id`;

ALTER TABLE `votercollector`.`district` 
ADD INDEX `FK_District_Street` (`Street_id` ASC) VISIBLE;
;
ALTER TABLE `votercollector`.`district` 
ADD CONSTRAINT `FK_District_Street`
  FOREIGN KEY (`Street_id`)
  REFERENCES `votercollector`.`street` (`Id_Street`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
  
  ALTER TABLE `votercollector`.`district` 
ADD COLUMN `Station_id` INT NULL DEFAULT NULL AFTER `Electoral_District_id`;

ALTER TABLE `votercollector`.`district` 
ADD INDEX `FK_District_Station` (`Station_id` ASC) VISIBLE;
;
ALTER TABLE `votercollector`.`district` 
ADD CONSTRAINT `FK_District_Station`
  FOREIGN KEY (`Station_id`)
  REFERENCES `votercollector`.`station` (`Id_Station`)
  ON DELETE set null
  ON UPDATE restrict;
  
create table MicroDistrict(
					Id_MicroDistrict INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					Name VARCHAR(256) NULL,
                    City_id INT default null,
					CONSTRAINT FK_MicroDistrict_City FOREIGN KEY (City_id) REFERENCES City (Id_City)
                    ON delete set null on update restrict
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table Street(
					Id_Street INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					Name VARCHAR(256) NULL,
                    City_id INT default null,
					CONSTRAINT FK_Street_City FOREIGN KEY (City_id) REFERENCES City (Id_City)
                    ON delete set null on update restrict
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table House(
                      Id_House INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      Name VARCHAR(256) NULL,
                      Street_id INT default null,
					  CONSTRAINT FK_House_Street FOREIGN KEY (Street_id) REFERENCES Street (Id_Street),
                      MicroDistrict_id INT default null,
					  CONSTRAINT FK_House_MicroDistrict FOREIGN KEY (MicroDistrict_id) REFERENCES MicroDistrict (Id_MicroDistrict)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

ALTER TABLE `votercollector`.`house` 
ADD COLUMN `City_id` INT NULL DEFAULT NULL AFTER `MicroDistrict_id`,
ADD INDEX `FK_House_City_idx` (`City_id` ASC) VISIBLE;
;
ALTER TABLE `votercollector`.`house` 
ADD CONSTRAINT `FK_House_City`
  FOREIGN KEY (`City_id`)
  REFERENCES `votercollector`.`city` (`Id_City`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

create table Polling_station(
                      Id_Polling_station INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      Name VARCHAR(256) NULL,
                      City_id INT default null,
					  CONSTRAINT FK_PollingStation_City FOREIGN KEY (City_id) REFERENCES City (Id_City) ON delete set null on update restrict,
                      Street_id INT default null,
					  CONSTRAINT FK_PollingStation_Street FOREIGN KEY (Street_id) REFERENCES Street (Id_Street) 
                      ON delete set null on update restrict,
                      MicroDistrict_id INT default null,
					  CONSTRAINT FK_PollingStation_MicroDistrict FOREIGN KEY (MicroDistrict_id) REFERENCES MicroDistrict (Id_MicroDistrict)
                      ON delete set null on update restrict,
                      House_id INT default null,
					  CONSTRAINT FK_PollingStation_House FOREIGN KEY (House_id) REFERENCES House (Id_House)
                      ON delete set null on update restrict
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table FieldActivity(
                      Id_FieldActivity INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      Name VARCHAR(256) NULL                      
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

create table Friend(
                      Id_Friend BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      Family_name VARCHAR(256) NULL,
                      Name_ VARCHAR(256) NULL,
                      Patronymic_name VARCHAR(256) NULL,
                      Date_birth date,
                      City_id INT default null,
                      CONSTRAINT FK_Friend_City FOREIGN KEY (City_id) REFERENCES City (Id_City) ON delete set null on update restrict,
                      District_id INT default null,
                      CONSTRAINT FK_Friend_District FOREIGN KEY (District_id) REFERENCES District (Id_District)
                      ON delete set null on update restrict,
                      Street_id INT default null,
					  CONSTRAINT FK_Friend_Street FOREIGN KEY (Street_id) REFERENCES Street (Id_Street) ON delete set null on update restrict,
                      MicroDistrict_id INT default null,
					  CONSTRAINT FK_Friend_MicroDistrict FOREIGN KEY (MicroDistrict_id) REFERENCES MicroDistrict (Id_MicroDistrict) ON delete set null on update restrict,
                      House_id INT default null,
					  CONSTRAINT FK_Friend_House FOREIGN KEY (House_id) REFERENCES House (Id_House) ON delete set null on update restrict,
                      Building VARCHAR(10) null,
                      Apartment VARCHAR(10) null,
                      Telephone VARCHAR(12) null,                      
					  Polling_station_id INT default null,
                      CONSTRAINT FK_Friend_Polling_station FOREIGN KEY (Polling_station_id) REFERENCES Polling_station (Id_Polling_station)
                      ON delete set null on update restrict,
                      Organization VARCHAR(256) null,
                      FieldActivity_id INT default null,
                      CONSTRAINT FK_Friend_FieldActivity FOREIGN KEY (FieldActivity_id) REFERENCES FieldActivity (Id_FieldActivity)
                      ON delete set null on update restrict,
                      Phone_number_responsible VARCHAR(12) null,
                      Date_registration_site date null,
                      Voting_date date null,
					  Voter TINYINT NULL DEFAULT 0,
					  Adress VARCHAR(500) default null,
					  TextQRcode VARCHAR(256) default null,
                      QRcode VARCHAR(4500) default null,
					  Email VARCHAR(256) default null,
                      Description VARCHAR(256) null,
					  User_id BIGINT default null,
                      CONSTRAINT FK_Friend_User FOREIGN KEY (User_id) REFERENCES User (Id_User) ON delete set null on update restrict,
                      GroupU_id INT default null,
					CONSTRAINT FK_Friend_GroupU FOREIGN KEY (GroupU_id) REFERENCES GroupU (Id_Group)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO Role (Name) VALUES ('admin');
INSERT INTO Role (Name) VALUES ('user');

INSERT INTO GroupU (Name) VALUES ('Main');
INSERT INTO GroupU (Name) VALUES ('Отдел ЦТМУ УИС');
INSERT INTO GroupU (Name) VALUES ('Отдел ЦИиЗИ УИС');
INSERT INTO GroupU (Name) VALUES ('Отдел Инф. обеспеч. ДГЗО');
INSERT INTO GroupU (Name) VALUES ('Отдел земельных правоотношений ДГЗО');
INSERT INTO GroupU (Name) VALUES ('Отдел реестра земель ДГЗО');

INSERT INTO User (UserName, Password, Role_id, Family_name, Name_, Patronymic_name, Date_birth, Telephone) VALUES ('vldmr@ya.ru','qwerty321','1',
                     'Луценко','Владимир','Сергеевич','1987.01.01','+79292822101');
INSERT INTO User (UserName, Password, Role_id, Family_name, Name_, Patronymic_name, Date_birth, Telephone) VALUES ('alex@ya.ru','qwerty321','1',
                     'Синцев','Алексей','Алексеевич','1987.01.01','+00000000000');
INSERT INTO User (UserName, Password, Role_id, Family_name, Name_, Patronymic_name, Date_birth, Telephone) VALUES ('poligraf@ya.ru','qwerty321','2',
                     'Полиграф','Полиграфович','Шариков','1987.01.01','+00000000000');
INSERT INTO User (UserName, Password, Role_id, Family_name, Name_, Patronymic_name, Date_birth, Telephone) VALUES ('poligraf@ya.ru','qwerty321','2',
                     'Гунин','Алексанр','Павлович','1987.01.01','+79235653981');
                     
Insert into GroupsUsers(GroupU_id, User_id) values(1,1);
Insert into GroupsUsers(GroupU_id, User_id) values(2,4);
Insert into GroupsUsers(GroupU_id, User_id) values(3,4);
Insert into GroupsUsers(GroupU_id, User_id) values(2,2);
Insert into GroupsUsers(GroupU_id, User_id) values(4,3);

INSERT INTO District (Name, City_id) VALUES ('Северный',1);
INSERT INTO District (Name, City_id) VALUES ('Южный',1);

INSERT INTO MicroDistrict (Name, City_id) VALUES ('70 лет ВЛКСМ',1);
INSERT INTO MicroDistrict (Name, City_id) VALUES ('18 мкр-н',1);

INSERT INTO Street (Name, City_id) VALUES ('Советская',1);
INSERT INTO Street (Name, City_id) VALUES ('Пр-т Победы',1);
INSERT INTO Street (Name, City_id) VALUES ('Ногина',1);

INSERT INTO Street (Name, City_id) VALUES ('Подмоячная',2);
INSERT INTO Street (Name, City_id) VALUES ('Маячная',2);
INSERT INTO Street (Name, City_id) VALUES ('1 Мая',2);

INSERT INTO Street (Name, City_id) VALUES ('Сакмарская',3);
INSERT INTO Street (Name, City_id) VALUES ('1 Мая',3);
INSERT INTO Street (Name, City_id) VALUES ('Советская',3);

INSERT INTO House (Name, Street_id) VALUES ('1',1);
INSERT INTO House (Name, Street_id) VALUES ('11',1);
INSERT INTO House (Name, Street_id) VALUES ('15',1);
INSERT INTO House (Name, Street_id) VALUES ('2',2);
INSERT INTO House (Name, Street_id) VALUES ('22',2);
INSERT INTO House (Name, Street_id) VALUES ('25',2);
INSERT INTO House (Name, Street_id) VALUES ('4',4);
INSERT INTO House (Name, Street_id) VALUES ('44',4);

INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1009',1,1,1);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1009',1,1,2);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1009',1,1,3);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1010',1,2,1);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1011',1,2,2);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1012',1,2,3);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1010',2,1,1);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1011',2,2,1);
INSERT INTO Polling_station (Name,City_id,Street_id,House_id) VALUES ('1012',3,1,1);
                                    

INSERT INTO Friend (Family_name, Name_, Patronymic_name, Date_birth, City_id, Street_id, House_id, Apartment, Telephone, Polling_station_id, District_id, Organization, FieldActivity_id, Phone_number_responsible, Date_registration_site, Voting_date,  Description,  User_id,GroupU_id) VALUES
				('Шариков','Полиграф','Полиграфович','1925.01.01',1,1,1,'','+79200002102',1,2,'ПО СТрела',1,'+79225353971','2021.04.01','2021.04.20','',1,1);
INSERT INTO Friend (Family_name, Name_, Patronymic_name, Date_birth, City_id, Street_id, House_id, Apartment, Telephone, Polling_station_id, District_id, Organization, FieldActivity_id, Phone_number_responsible, Date_registration_site, Voting_date,  Description,  User_id,GroupU_id) VALUES
				('Иванов','Иван','Иванович','1991.01.01',2,2,2,'5','+79200022102',2,2,'ОГУ',2,'+79225353971','2021.04.01','2021.04.20','',1,2);
INSERT INTO Friend (Family_name, Name_, Patronymic_name, Date_birth, City_id, Street_id, House_id, Apartment, Telephone, Polling_station_id, District_id, Organization, FieldActivity_id, Phone_number_responsible, Date_registration_site, Voting_date,  Description,  User_id,GroupU_id) VALUES
				('Петров','Петр','Петрович','1991.01.01',1,3,1,'5','+79292000102',3,3,'АО Ромашка',3,'+79225353971','2021.04.01','2021.04.20','',2,1);
                
Drop table Friend;
Drop table House;
Drop table polling_station;
Drop table district;
Drop table MicroDistrict;
Drop table Street;
Drop table fieldactivity;
Drop table GroupsUsers;
Drop table GroupU;
Drop table city;
Drop table user;
Drop table role;