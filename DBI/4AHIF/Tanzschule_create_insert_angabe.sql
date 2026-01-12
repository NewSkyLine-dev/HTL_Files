create database tanzschule;

drop table if exists termine
drop table if exists kursbelegung
drop table if exists tanzkurs
drop table if exists mitglied
go

CREATE TABLE Tanzkurs (
	KursNr CHAR(2) not null primary key,
	Kursname VARCHAR(35),
	Kursleiter CHAR(25),
	Kursstunden int DEFAULT 10,
	Preis DECIMAL(10,2) DEFAULT 40,
	CHECK (Kursstunden > 9 AND Kursstunden > 0),
);

CREATE TABLE Mitglied (
	MitgliedsNr INT not null IDENTITY primary key,
	Nachname VARCHAR(25),
	Vorname VARCHAR(20),
	Titel varchar(10),
	Gruppe CHAR(1) not null,
	TelefonNr VARCHAR(15),
	Plz CHAR(4),
	Ort VARCHAR(20),
	Strasse CHAR(30),
	Gebdat DATETIME not null,
	Maennlich BIT not null, --1=maennlich,0=weiblich
	FamStand VARCHAR(15),
	email varchar(40),
	check (Gebdat >= '1.1.1940' AND Gebdat < GETDATE()),
	check (FamStand in ('Ledig', 'Verheiratet', 'Geschieden', 'Verwitwet'))
);

CREATE TABLE Kursbelegung (
	KursNr CHAR(2) not null references Tanzkurs(KursNr),
	MitgliedsNr INT not null references Mitglied(MitgliedsNr),
	Tanzleistung INT,
	primary key (KursNr, MitgliedsNr),
	check (Tanzleistung between 1 and 5)
);

CREATE TABLE Termine(
	KursNr CHAR(2) not null,
	Termin DATETIME not null,
	Stunden int,
	FOREIGN KEY (KursNr) REFERENCES Tanzkurs(KursNr),
	primary key (KursNr, Termin)
);

--*****************************************************************************************
set dateformat dmy
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand,email) 
	VALUES ('Kaufmann',	'Patricia',null,'A','01/1234','1070','Wien','Georgirstr.1','01.01.1980',0,'Ledig','pat.kaufman@gmx.at');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email)
	 VALUES ('Schmidt',	'Barbara','Ing.','A','01/9645123','1011','Wien','Huberstr. 12','12.12.1979',0,'Verheiratet','b.schmidt@gmx.at');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Braun',	'Oliver','DI.FH','C','01/345123','1010','Wien','Wienerstr.123','23.07.1978',1,'Geschieden','oliver@gmx.at');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Kiefer',	'Bernd',null,'D','02622/123455','2700','Wr.Neustadt','Deichgasse 12','11.11.1944',1,'Ledig',Null);
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Schmolke',	'Tom',null,'B','02622/6543','2700','Wr.Neustadt','Karlgasse 23','21.04.1956',1,'Verwitwet','tom.schmolke@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Hansen',	'Gertrud',null,'A','02622/72974','2700','Wr.Neustadt','Landlgasse 23','14.06.1965',0,'Ledig','hansen@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Blume',	'Simon','Dr.','B','0732/45646','4020','Linz','Wiensrstr. 120','23.01.1967',1,'Verheiratet','blume12@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Kraus',	'Margit',null,'C','0732/56234','4020','Linz','Friedrichgasse 5','02.03.1959',0,'Verheiratet','margit.kraus@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Hugo',	'Xaver','Mag.','A','01/34567','1010','Wien','Paulistr.34','23.01.1970',1,'Geschieden','xaver@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email)
	 VALUES ('Tobisch',	'Lotte',null,'B','02622/5677','1020','Wr.Neustadt','Wienerstr.123','01.05.1978',0,'Ledig','tobisch@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Müller','Otto','Dipl.Ing.','C','01/3451','1010','Wien','Linzerstr. 44','03.04.1982',0,'Ledig','mueller@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Müller','Anna',null,'A','02732/123','4020','Linz','Leondingerstr.33','26.09.1978',0,'Ledig','mueller@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Schmied','Paul',null,'A','02732/4565','4020','Linz','Hafenstr.89','12.03.1988',0,'Ledig','paul.schmied@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Lotto','Inge','Dipl.Ing.','A','0456/89','8010','Graz','Wienerstr.567','01.01.1980',0,'Ledig','inge.lotto@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Huber','Georg','Ing.','B',NULL,'8010','Graz','Wienerstr:23','12.07.1978',0,'Ledig','huber@gmail.com');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Feiner','Hannes','Dr.','A',NULL,'2352','Mödling','Hinterbrühlerstr.12','23.07.1967',0,'Verheiratet','feiner@gmx.at');
INSERT INTO Mitglied (Nachname,Vorname,Titel,Gruppe,TelefonNr,Plz,Ort,Strasse,Gebdat,Maennlich,FamStand, email) 
	VALUES ('Degen','Walter',null,'A',NULL,'2352','Mödling','Perchtoldsdorferstr. 3','19.07.1970',0,'Verwitwet',Null);
select * from mitglied;
go


INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('D2','Tango','Meier',14,90);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('A3','Lateinamerikanische Tänze','Meier',10,40);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('C3','Salsa','Meier',10,60);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('E1','Wiener Walzer','Grimm',10,40);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('E2','Englischer Walzer','Sommer',10,40);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('A1','Standard 1','Müller',20,60);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('A2','Standard 2','Müller',20,60);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('B1','Step','Grimm',10,90);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('C1','Moderne Tänze','Sommer',15,90);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('C2','Rock'+''''+'n Roll','Sauer',15,80);
INSERT INTO Tanzkurs (KursNr,Kursname,Kursleiter,Kursstunden,Preis) VALUES ('D1','Bewegungstherapie','Grimm',20,100);
select * from tanzkurs;
go

INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D2','2.3.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D2','9.3.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D2','16.3.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D2','23.3.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D2','2.4.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D2','6.4.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D2','13.4.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A3','4.3.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A3','5.3.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C3','27.2.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C3','28.2.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C3','1.3.2021',2)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('E1','5.6.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('E1','12.6.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('E2','5.6.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('E2','12.6.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A1','5.6.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A1','12.6.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A1','19.6.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A1','26.6.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A1','1.7.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A2','8.7.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A2','12.7.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A2','16.7.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A2','20.7.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('A2','24.7.2021',4)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('B1','2.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('B1','3.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C1','2.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C1','3.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C1','4.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C2','2.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C2','3.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C2','4.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1','2.6.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1','3.6.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1','4.6.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1','2.5.2021',5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C1','4.5.2021',5)
-- INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C2','2.5.2021',5)
-- INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C2','3.5.2021',5)

INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('C2',getdate() + 12,5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1',getdate() + 31,5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1',getdate() + 15,5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1',getdate() + 16,5)
INSERT INTO Termine (KursNr,Termin,Stunden) VALUES ('D1',getdate() + 33,5)
select * from Termine
go

INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('A1',3,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('A1',4,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('A1',5,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('A1',8,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('A2',1,1);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('A2',3,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('A2',7,3);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('B1',2,5);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('B1',3,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('B1',6,NULL);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('C1',4,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('C1',5,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('C2',1,1);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('C2',2,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('C2',3,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('C2',4,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('C2',5,2);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('D1',7,5);
INSERT INTO Kursbelegung (KursNr,MitgliedsNr,Tanzleistung) VALUES ('D1',8,1);
select * from kursbelegung;
go

-- a. Alle Mitglieder mit Name und Telefonnummer
select Vorname, Nachname, STRING_AGG(TelefonNr, ', ') 
from Mitglied
group by Vorname, Nachname;

-- b. Alle Kursnamen sortiert
select Kursname
from Tanzkurs
order by Kursname asc;

-- c. Gruppen denen Mitglieder zugeornet sind, sortiert nach Gruppe und Nachname
select Gruppe, Nachname, Vorname
from Mitglied
order by Gruppe, Nachname;

-- d. Alle Mitgliederinformationen
select *
from Mitglied;

-- e. Wohnort von Mitgliedern
select Vorname, Ort
from Mitglied
order by ort;

-- f. Alle Tanzkurse, absteigend sortiert nach
select *
from Tanzkurs
order by Kursname desc;

-- g. Alle Mitglieder, absteigend sortiert nach Plz
select *
from Mitglied
order by Plz desc;

-- h. Alle Mitglieder mit Telefonnummer und Mitgliedsnummer, sortiert nach Nachname und Vorname
select Vorname, Nachname, TelefonNr, MitgliedsNr
from Mitglied
order by Nachname, Vorname;

-- i. 
select Vorname, Nachname, TelefonNr, MitgliedsNr
from Mitglied
order by Nachname desc, Vorname asc;

-- j.
select CONCAT(email, ' ist die Emailadresse von ', Vorname, ' ', Nachname)
from Mitglied;

-- k.
select datediff(day, Gebdat, GETDATE()) / 365.0
from Mitglied;

-- l.
select Vorname, Nachname
from Mitglied
where Ort <> 'Wien';

-- m.
select Vorname, Nachname
from Mitglied
where Gebdat = '1.1.1980';

-- n.
select *
from Tanzkurs
where Kursstunden > 12;

-- o.
select *
from Mitglied
where year(Gebdat) = 1970 and month(Gebdat) = 7;

-- p.
select *
from Mitglied
where Ort in ('Wr.Neustadt', 'Neunkirchen', 'Baden', 'Eisenstadt');

-- q.
select *
from Mitglied
where Plz is null;

-- r.
select *
from Mitglied
where Titel not in('Ing.', 'Dipl.Ing.', 'Dr.') or Titel is null;

-- s.
select *
from Mitglied
where Nachname like '%e%' and len(Nachname) = 5;

-- t.
select tk.Kursname, tk.Kursleiter, te.Termin
from Tanzkurs tk
	join Termine te on te.KursNr = tk.KursNr
order by tk.Kursleiter desc, te.Termin desc;

-- u.
select m.Nachname + ' ' + m.Vorname, DATEDIFF(year, m.Gebdat, getdate()) as [Alter]
from Kursbelegung kb
	join Mitglied m on kb.MitgliedsNr = m.MitgliedsNr
order by [Alter]

-- v.
select Nachname
from Mitglied
intersect
select Kursleiter
from Tanzkurs;

-- w.
select tk.Kursname, m.Vorname, m.Nachname
from Tanzkurs tk
	left join Kursbelegung kb on kb.KursNr = tk.KursNr
	left join Mitglied m on m.MitgliedsNr = kb.MitgliedsNr
order by m.Nachname;