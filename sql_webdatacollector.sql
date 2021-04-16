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

create table District(
					Id_District INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
					Name VARCHAR(256) NULL,
                    City_id INT default null,
					CONSTRAINT FK_District_City FOREIGN KEY (City_id) REFERENCES City (Id_City)
                    ON delete set null on update restrict
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
                      QRcode VARCHAR(4500) default null,
                      Description VARCHAR(256) null,
					  User_id BIGINT default null,
                      CONSTRAINT FK_Friend_User FOREIGN KEY (User_id) REFERENCES User (Id_User) ON delete set null on update restrict,
                      GroupU_id INT default null,
					CONSTRAINT FK_Friend_GroupU FOREIGN KEY (GroupU_id) REFERENCES GroupU (Id_Group)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO Role (Name) VALUES ('admin');
INSERT INTO Role (Name) VALUES ('user');

INSERT INTO GroupU (Name) VALUES ('UIS');
INSERT INTO GroupU (Name) VALUES ('FinUp');

INSERT INTO User (UserName, Password, Role_id, Family_name, Name_, Patronymic_name, Date_birth, Telephone) VALUES ('vldmr@ya.ru','qwerty321','1',
                     'Луценко','Владимир','Serg','1987.01.01','+79292822101');
INSERT INTO User (UserName, Password, Role_id, Family_name, Name_, Patronymic_name, Date_birth, Telephone) VALUES ('alex@ya.ru','qwerty654','1',
                     'Синцев','Алексей','Алексеевич','1987.01.01','+00000000000');
INSERT INTO User (UserName, Password, Role_id, Family_name, Name_, Patronymic_name, Date_birth, Telephone) VALUES ('poligraf@ya.ru','2','2',
                     'Полиграф','Полиграфович','Шариков','1987.01.01','+00000000000');
                     
Insert into GroupsUsers(GroupU_id, User_id) values(1,1);
Insert into GroupsUsers(GroupU_id, User_id) values(1,2);
Insert into GroupsUsers(GroupU_id, User_id) values(2,1);
Insert into GroupsUsers(GroupU_id, User_id) values(1,3);

INSERT INTO City (Name) VALUES ('Оренбург');
INSERT INTO City (Name) VALUES ('Ростоши');
INSERT INTO City (Name) VALUES ('Кушкуль');

INSERT INTO District (Name, City_id) VALUES ('Северный',1);
INSERT INTO District (Name, City_id) VALUES ('Южный',1);

INSERT INTO MicroDistrict (Name, City_id) VALUES ('70 лет ВЛКСМ',1);
INSERT INTO MicroDistrict (Name, City_id) VALUES ('18 мкр-н',1);

INSERT INTO Street (Name, City_id) VALUES ('Советская',1);
INSERT INTO Street (Name, City_id) VALUES ('Пр-т Победы',1);
INSERT INTO Street (Name, City_id) VALUES ('Ногина',1);

INSERT INTO House (Name, Street_id) VALUES ('10',1);
INSERT INTO House (Name, Street_id) VALUES ('15',1);
INSERT INTO House (Name, Street_id) VALUES ('10',2);
INSERT INTO House (Name, Street_id) VALUES ('15',3);

INSERT INTO Polling_station (Name,City_id,Street_id,MicroDistrict_id,House_id) VALUES ('1009',1,1,1,1);
INSERT INTO Polling_station (Name,City_id,Street_id,MicroDistrict_id,House_id) VALUES ('1009',1,1,1,2);
INSERT INTO Polling_station (Name,City_id,Street_id,MicroDistrict_id,House_id) VALUES ('1009',1,1,1,3);
                                    
INSERT INTO FieldActivity (Name) VALUES ('Промышленность');
INSERT INTO FieldActivity (Name) VALUES ('Образование');
INSERT INTO FieldActivity (Name) VALUES ('Торговля');



INSERT INTO Friend (Family_name, Name_, Patronymic_name, Date_birth, City_id, Street_id, House_id, Apartment, Telephone, Polling_station_id, District_id, Organization, FieldActivity_id, Phone_number_responsible, Date_registration_site, Voting_date,  Description,  User_id,GroupU_id) VALUES
				('Vladimir','Volodya','Serg','1995.01.01',1,1,1,'','+79292822102',1,1,'ПО СТрела',1,'+79225353971','2021.04.01','2021.04.20','',1,1);
INSERT INTO Friend (Family_name, Name_, Patronymic_name, Date_birth, City_id, Street_id, House_id, Apartment, Telephone, Polling_station_id, District_id, Organization, FieldActivity_id, Phone_number_responsible, Date_registration_site, Voting_date,  Description,  User_id,GroupU_id) VALUES
				('Иванов','Иван','Иванович','1991.01.01',2,2,2,'5','+79292822102',2,2,'ОГУ',2,'+79225353971','2021.04.01','2021.04.20','',1,2);
INSERT INTO Friend (Family_name, Name_, Patronymic_name, Date_birth, City_id, Street_id, House_id, Apartment, Telephone, Polling_station_id, District_id, Organization, FieldActivity_id, Phone_number_responsible, Date_registration_site, Voting_date,  Description,  User_id,GroupU_id) VALUES
				('Петров','Петр','Петрович','1991.01.01',1,3,1,'5','+79292822102',3,2,'АО Ромашка',3,'+79225353971','2021.04.01','2021.04.20','',2,1);
                
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