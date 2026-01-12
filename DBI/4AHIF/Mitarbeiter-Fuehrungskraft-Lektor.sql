DROP TABLE IF EXISTS Fuehrungskraft1A;
DROP TABLE IF EXISTS Lektor1A;
DROP TABLE IF EXISTS Mitarbeiter1A;
DROP TABLE IF EXISTS Fuehrungskraft1B;
DROP TABLE IF EXISTS Lektor1B;
DROP TABLE IF EXISTS Mitarbeiter1B;

-- 1A create
CREATE TABLE Mitarbeiter1A (
    Mitarbeiter_ID INT PRIMARY KEY,
    Name VARCHAR(100),
    Geburtsdatum DATE
);

-- Sub-Entität: Führungskraft
CREATE TABLE Fuehrungskraft1A (
    Fuehrungskraft_ID INT PRIMARY KEY,
    Mitarbeiter_ID INT,
    Management_Ebene VARCHAR(50),
	Gehalt INT,
    FOREIGN KEY (Mitarbeiter_ID) REFERENCES Mitarbeiter1A(Mitarbeiter_ID)
);

-- Sub-Entität: Lektor
CREATE TABLE Lektor1A (
    Lektor_ID INT PRIMARY KEY,
    Mitarbeiter_ID INT,
    Spezialgebiet VARCHAR(100),
	Gehalt INT,
    FOREIGN KEY (Mitarbeiter_ID) REFERENCES Mitarbeiter1A(Mitarbeiter_ID)
);

-- 1B create
CREATE TABLE Mitarbeiter1B (
    Mitarbeiter_ID INT PRIMARY KEY,
    Name VARCHAR(100),
    Geburtsdatum DATE
);
CREATE TABLE Fuehrungskraft1B (
    Fuehrungskraft_ID INT PRIMARY KEY,
	Mitarbeiter_ID INT,
    Name VARCHAR(100),
    Geburtsdatum DATE,
    Management_Ebene VARCHAR(50),
	Gehalt INT,
	FOREIGN KEY (Mitarbeiter_ID) REFERENCES Mitarbeiter1B(Mitarbeiter_ID)
);
CREATE TABLE Lektor1B (
    Lektor_ID INT PRIMARY KEY,
	Mitarbeiter_ID INT,
    Name VARCHAR(100),
    Geburtsdatum DATE,
    Spezialgebiet VARCHAR(100),
	Gehalt INT,
	FOREIGN KEY (Mitarbeiter_ID) REFERENCES Mitarbeiter1B(Mitarbeiter_ID)
);

/*
1A: Da die Sub-Entitäten nur den Primärschlüssel der Super-Entität enthalten, 
ist kein redundanter Speicherbedarf für Attribute wie Name oder Geburtsdatum 
notwendig. Dies minimiert den Speicherbedarf und vermeidet Datenredundanz.

1B: Hier müssen alle Attribute der Super-Entität in den Sub-Entitäten 
gespeichert werden, was zu erhöhter Redundanz und größerem Speicherbedarf 
führt. Änderungen an einem Attribut müssen in jeder betroffenen Sub-Entität
durchgeführt werden, was die Konsistenz gefährden kann.
*/

SELECT * FROM Mitarbeiter1A m
	LEFT JOIN Lektor1A f ON m.Mitarbeiter_ID = f.Mitarbeiter_ID;

-- 1A insert
INSERT INTO Mitarbeiter1A(Mitarbeiter_ID, Name, Geburtsdatum) VALUES 
    (4, 'Clara Fischer', '1982-11-11'),
    (5, 'Michael Wagner', '1992-05-25'),
    (6, 'Laura Beck', '1987-09-13');
INSERT INTO Fuehrungskraft1A(Fuehrungskraft_ID, Mitarbeiter_ID, Management_Ebene, Gehalt) VALUES 
    (2, 4, 'Abteilung 2', 3000),
    (3, 5, 'Abteilung 3', 2800),
    (4, 6, 'Abteilung 4', 3100);
INSERT INTO Lektor1A(Lektor_ID, Mitarbeiter_ID, Spezialgebiet, Gehalt) VALUES 
    (2, 4, 'Informatik', 2600),
    (3, 5, 'Physik', 2400),
    (4, 6, 'Biologie', 2700);

-- 1B insert
INSERT INTO Mitarbeiter1B(Mitarbeiter_ID, Name, Geburtsdatum) VALUES 
    (4, 'Clara Fischer', '1982-11-11'),
    (5, 'Michael Wagner', '1992-05-25'),
    (6, 'Laura Beck', '1987-09-13');
INSERT INTO Fuehrungskraft1B(Fuehrungskraft_ID, Mitarbeiter_ID, Name, Geburtsdatum, Management_Ebene, Gehalt) VALUES 
    (2, 4, 'Clara Fischer', '1982-11-11', 'Abteilung 2', 3000),
    (3, 5, 'Michael Wagner', '1992-05-25', 'Abteilung 3', 2800),
    (4, 6, 'Laura Beck', '1987-09-13', 'Abteilung 4', 3100);
INSERT INTO Lektor1B(Lektor_ID, Mitarbeiter_ID, Name, Geburtsdatum, Spezialgebiet, Gehalt) VALUES 
    (2, 4, 'Clara Fischer', '1982-11-11', 'Informatik', 2600),
    (3, 5, 'Michael Wagner', '1992-05-25', 'Physik', 2400),
    (4, 6, 'Laura Beck', '1987-09-13', 'Biologie', 2700);

-- 1A select
SELECT F.*
FROM Fuehrungskraft1A F
	JOIN Mitarbeiter1A M ON F.Mitarbeiter_ID = M.Mitarbeiter_ID;
/*
Die Performance wird hier leicht eingeschränkt wegen dem JOINow.
Daher ist auch due Komplexität etwas großer.
*/

SELECT *
FROM Mitarbeiter1A M
	LEFT JOIN Fuehrungskraft1A F ON M.Mitarbeiter_ID = F.Mitarbeiter_ID
	LEFT JOIN Lektor1A L ON M.Mitarbeiter_ID = L.Mitarbeiter_ID;
/*
Da wir hier 2 LEFT JOINS benutzen wird die Performance ziemlich eingeschränkt,
daher ist die Komplexität auch ein wenig großer.
*/

-- 1B select
SELECT Fuehrungskraft_ID, Management_Ebene, Gehalt
FROM Fuehrungskraft1B;
/*
Da wir alle Informationen in der Tabelle haben ist die Komplexität und
die Performance hier sehr gut.
*/

SELECT * 
FROM Mitarbeiter1B m
	LEFT JOIN Fuehrungskraft1B f ON m.Mitarbeiter_ID = f.Mitarbeiter_ID
	LEFT JOIN Lektor1B l ON l.Mitarbeiter_ID = m.Mitarbeiter_ID;
/*
Hier müssen wir auch 2 LEFT JOINS machen, was die Komplexität steigert und
die Performance verschlächert.
*/