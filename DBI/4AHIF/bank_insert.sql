use bank;
go
insert into Kunde values(1,'Müller','Hans','1010 Wien, Linzerstr123')
insert into Kunde values(2,'Müller','Karl','1010 Wien, Linzerstr123')
insert into Kunde values(3,'Müller','Anna','1010 Wien, Linzerstr123')
insert into Kunde values(4,'Schneider','Otto','1010 Wien, Linzerstr123')
insert into Kunde values(5,'Schneider','Maria','1010 Wien, Linzerstr123')
insert into Kunde values(6,'Schneider','Susi','1010 Wien, Linzerstr123')
insert into Kunde values(7,'Schuster','Werner','1010 Wien, Linzerstr123')
insert into Kunde values(8,'Schuster','Lotte','1010 Wien, Linzerstr123')
insert into Kunde values(9,'Tischler','Karl','1010 Wien, Linzerstr123')
insert into Kunde values(10,'Tischler','Hans','1010 Wien, Linzerstr123')
insert into Kunde values(11,'Energieversorger',NULL,'1010 Wien, Linzerstr123')
insert into Kunde values(12,'Tankstelle','Hans','1010 Wien, Linzerstr123')
insert into Kunde values(13,'Versicherung','Hans','1010 Wien, Linzerstr123')
insert into Kunde values(14,'Hauseigentümer','Hans','1010 Wien, Linzerstr123')
go
select * from Kunde
go
insert into Bank VALUES ('BAWAG Wiener Neustadt Herzogleopoldstrasse');
insert into Bank VALUES ('BAWAG Wiener Neustadt Wienerstrasse');
insert into Bank VALUES ('BAWAG Wiener Neustadt Postamt');
insert into Bank VALUES ('BAWAG Wiener Neustadt Fantasiestrasse');
insert into Bank VALUES ('BAWAG Wiener Neustadt Arndtgasse');
go

INSERT into Konto VALUES (1, 100, 1);
INSERT into Konto VALUES (1, 101, 2);
INSERT into Konto VALUES (1, 102, 3);
INSERT into Konto VALUES (1, 103, 4);
INSERT into Konto VALUES (1, 104, 5);
INSERT into Konto VALUES (2, 200, 11);
INSERT into Konto VALUES (2, 202, 12);
INSERT into Konto VALUES (2, 204, 13);
INSERT into Konto VALUES (3, 307, 14);
INSERT into Konto VALUES (3, 309, 11);
INSERT into Konto VALUES (3, 311, 12);
INSERT into Konto VALUES (3, 313, 13);
INSERT into Konto VALUES (3, 315, 14);
select * from konto
go

insert into Kontobewegung values(1,100,1,'Start',1000,'1.12.2020')
insert into Kontobewegung values(1,100,2,'AutoVersicherung',-340,'1.12.2020')
insert into Kontobewegung values(1,100,3,'Miete',-600,'1.12.2020')
insert into Kontobewegung values(1,100,4,'Benzin',-67,'1.12.2020')
insert into Kontobewegung values(1,100,5,'Strom',-123,'1.12.2020')
insert into Kontobewegung values(1,101,1,'Start',1000,'1.12.2020')
insert into Kontobewegung values(1,101,2,'AutoVersicherung',-340,'1.12.2020')
insert into Kontobewegung values(1,101,3,'Miete',-600,'1.12.2020')
insert into Kontobewegung values(1,101,4,'Benzin',-67,'1.12.2020')
insert into Kontobewegung values(1,101,5,'Strom',-123,'1.12.2020')
insert into Kontobewegung values(1,102,1,'Start',1000,'1.12.2020')
insert into Kontobewegung values(1,102,2,'AutoVersicherung',-340,'1.12.2020')
insert into Kontobewegung values(1,102,3,'Miete',-600,'1.12.2020')
insert into Kontobewegung values(1,102,4,'Benzin',-67,'1.12.2020')
insert into Kontobewegung values(1,102,5,'Strom',-123,'1.12.2020')
insert into Kontobewegung values(1,103,1,'Start',1000,'1.12.2020')
insert into Kontobewegung values(1,103,2,'AutoVersicherung',-340,'1.12.2020')
insert into Kontobewegung values(1,103,3,'Miete',-600,'1.12.2020')
insert into Kontobewegung values(1,103,4,'Benzin',-67,'1.12.2020')
insert into Kontobewegung values(1,104,4,'Strom',-123,'1.12.2020')
insert into Kontobewegung values(2,200,1,'Strom',123,'1.12.2020')
insert into Kontobewegung values(2,200,2,'Strom',123,'1.12.2020')
insert into Kontobewegung values(2,200,3,'Strom',123,'1.12.2020')
insert into Kontobewegung values(2,200,4,'Strom',123,'1.12.2020')
insert into Kontobewegung values(2,202,1,'Benzin',67,'1.12.2020')
insert into Kontobewegung values(2,202,2,'Benzin',67,'1.12.2020')
insert into Kontobewegung values(2,202,3,'Benzin',67,'1.12.2020')
insert into Kontobewegung values(2,202,4,'Benzin',67,'1.12.2020')
insert into Kontobewegung values(2,204,1,'AutoVersicherung',340,'1.12.2020')
insert into Kontobewegung values(2,204,2,'AutoVersicherung',340,'1.12.2020')
insert into Kontobewegung values(2,204,3,'AutoVersicherung',340,'1.12.2020')
insert into Kontobewegung values(2,204,4,'AutoVersicherung',340,'1.12.2020')
insert into Kontobewegung values(2,206,1,'Miete',600,'1.12.2020')
insert into Kontobewegung values(2,206,2,'Miete',600,'1.12.2020')
insert into Kontobewegung values(2,206,3,'Miete',600,'1.12.2020')
insert into Kontobewegung values(2,206,4,'Miete',600,'1.12.2020')

