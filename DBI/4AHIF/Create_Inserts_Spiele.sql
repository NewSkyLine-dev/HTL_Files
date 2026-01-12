create database spiel;

DROP   TABLE IF EXISTS Spiel;
DROP   TABLE IF EXISTS Person;
DROP   TABLE IF EXISTS Team;
DROP   TABLE IF EXISTS Ort;



CREATE TABLE Ort (
  ONr               INTEGER PRIMARY KEY,
  Bezeichnung       VARCHAR(20),
  Einwohner         INTEGER,
  Flaeche           INTEGER -- in Quadratkilometer
);
insert into Ort values ( 1,'Wien',  1700000, 415);
insert into Ort values ( 2,'Graz',   265000, 128);
insert into Ort values ( 3,'London',7400000,1580);
insert into Ort values ( 4,'Paris', 2200000,1200);
select * from Ort;

CREATE TABLE Team (
  TNr               INTEGER PRIMARY KEY,
  Bezeichnung       VARCHAR(20),
  Land              VARCHAR(10),
  Gruendungsjahr    Integer,
  CHECK (Land IN ('A', 'D', 'F', 'I', 'S')),
  CHECK (Gruendungsjahr BETWEEN 1900 AND 2010)
);
insert into Team values (10,'Sport Wien',     'A',1980);
insert into Team values (20,'Rot-Gelb',       'D',1921);
insert into Team values (30,'Rapid Wien',     'A',1915);
insert into Team values (40,'Dynamique Paris','F',1968);
select * from Team;

CREATE TABLE Person (
  ONr               INTEGER REFERENCES Ort, -- Geburtsort der Person
  Lfnd              INTEGER,
  PRIMARY KEY       (ONr,Lfnd),
  NName             VARCHAR(20),
  VName             VARCHAR(20),
  GebDat            DATE,
  TNr               INTEGER  REFERENCES Team
);
insert into Person values ( 1,1,'Gruber', 'Martina','1981-06-01',10);
insert into Person values ( 1,2,'Hofer',  'Markus', '1960-03-14',10);
insert into Person values ( 1,3,'Haller', 'Peter',  '1983-10-23',20);
insert into Person values ( 2,1,'Burger', 'Franz', '1955-04-06',30);
insert into Person values ( 2,2,'Bauer',  'Petra',  '1993-03-22',30);
insert into Person values ( 4,1,'Wagner', 'Michael','1972-12-11',20);
insert into Person values ( 4,2,'Bauer',  'Josef', '1961-05-01' ,20);
insert into Person values ( 4,3,'Hofer',  'Josef',  '1974-01-31',10);
select * from Person;

CREATE TABLE Spiel (
  SNr               INTEGER PRIMARY KEY, 
  Datum             DATE,
  ONr               INTEGER REFERENCES Ort,           -- Austragungsort
  TNrSieg           INTEGER REFERENCES Team, -- Sieger-Team
  TNrVerlierer          INTEGER REFERENCES Team, -- Sieger-Team
  ONrSchieds        INTEGER, -- ) Schieds-
  LfndSchieds       INTEGER, -- ) richter
  FOREIGN KEY (ONrSchieds,LfndSchieds) REFERENCES Person,
  Zuschauer         INTEGER,
  CHECK (TNrSieg <> TNrVerlierer)
);

insert into Spiel values (100,'2013-02-04',1,10,20,2,   1,    5000);
insert into Spiel values (101,'2012-12-01',3,10,40,2,   2,   15000);
insert into Spiel values (102,'2013-08-18',4,20,40,2,   1,   10000);
insert into Spiel values (103,'2014-04-23',3,40,10,4,   1,     500);
insert into Spiel values (104,'2014-07-13',4,10,40,NULL,NULL, 8000);
insert into Spiel values (105,'2013-08-20',3,20,40,4,   1,    6000);
insert into Spiel values (106,'2012-10-07',3,40,10,1,   1,   12000);
insert into Spiel values (107,'2011-09-11',1,20,10,4,   2,   11000);
insert into Spiel values (108,'2012-04-07',4,10,40,1,   1,    7000);
insert into Spiel values (109,'2014-03-30',1,40,20,1,   2,    4500);
insert into Spiel values (110,'2014-05-29',3,30,10,NULL,NULL, 2000);

-- DQL
-- 1. Alles, sortierte nach Datum und Bezeichnung des Ortes
SELECT 
    S.Datum, 
    O.Bezeichnung AS Austragungsort, 
    TS.Bezeichnung AS Sieger_Team, 
    TV.Bezeichnung AS Verlierer_Team, 
    P.VName + ' ' + P.NName AS Schiedsrichter
FROM Spiel S
	JOIN Ort O ON S.ONr = O.ONr
	JOIN Team TS ON S.TNrSieg = TS.TNr
	JOIN Team TV ON S.TNrVerlierer = TV.TNr
	LEFT JOIN Person P ON S.ONrSchieds = P.ONr AND S.LfndSchieds = P.Lfnd
ORDER BY S.Datum, O.Bezeichnung;

-- 2. Wie oft haben die einzelnen Teams gespielt
SELECT 
    T.Bezeichnung, 
    T.Land, 
    COUNT(S.SNr) AS [Anzahl Spiele]
FROM Team T
	LEFT JOIN Spiel S ON T.TNr = S.TNrSieg OR T.TNr = S.TNrVerlierer
GROUP BY T.Bezeichnung, T.Land
ORDER BY [Anzahl Spiele] DESC;

-- 3. Welche Personen waren noch bei keinem Spiel
SELECT P.NName, P.VName
FROM Person P
	LEFT JOIN Spiel S ON P.ONr = S.ONrSchieds AND P.Lfnd = S.LfndSchieds
WHERE S.SNr IS NULL;

-- 4. In welchen Teams sind Personen die in Wien und in Paris geboren sind
SELECT P.TNr
FROM Person P
	JOIN Ort O ON P.ONr = O.ONr
WHERE O.Bezeichnung = 'Wien'
INTERSECT
SELECT P.TNr
FROM Person P
	JOIN Ort O ON P.ONr = O.ONr
WHERE O.Bezeichnung = 'Paris';

-- 5. In wie viele Orten sind mehr als Zwei Personen geboren
SELECT ONr 
FROM Person
GROUP BY ONr
HAVING COUNT(Lfnd) > 2;

-- 6. Personen die vor dem Grundungsjahr des Teams geboren wurden
SELECT * 
FROM Team T
	JOIN Person P ON T.TNr = P.TNr
WHERE YEAR(P.GebDat) < T.Gruendungsjahr;

-- DML
-- 1. Zuschauer um 20% wo Schiedsrichter jünger als 40 ist
UPDATE Spiel
SET Zuschauer = Zuschauer * 1.2
WHERE SNr IN (
	SELECT S.SNr 
	FROM Spiel S
		JOIN Person P ON S.ONrSchieds = P.ONr AND S.LfndSchieds = P.Lfnd
	WHERE DATEDIFF(YEAR, P.GebDat, GETDATE()) < 40
);

-- 2
DELETE FROM Spiel
WHERE ONr IN (
	SELECT ONr
	FROM Ort
	WHERE Einwohner < 2000000
);