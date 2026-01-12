CREATE DATABASE Reisedatenbank;
USE Reisedatenbank;

DROP TABLE IF EXISTS Rueckfluege;
DROP TABLE IF EXISTS Hinfluege;
DROP TABLE IF EXISTS Hotels;
DROP TABLE IF EXISTS Fluege;
DROP TABLE IF EXISTS Kunden;
DROP TABLE IF EXISTS Reiseziele;
DROP TABLE IF EXISTS Buchungen;

CREATE TABLE Buchungen
(
    BNr INTEGER PRIMARY KEY NOT NULL,
    Buchungsdatum DATE NOT NULL,
    Preis INTEGER NOT NULL,
    Personen INTEGER NOT NULL,
    KNr INTEGER NOT NULL,
    HNr INTEGER NOT NULL,
    CHECK (Personen > 0),
    CHECK (Preis BETWEEN 50 AND 5000),
)

CREATE TABLE Reiseziele
(
    RZNr INTEGER PRIMARY KEY NOT NULL,
    Ziel VARCHAR(20) NOT NULL,
    UNIQUE (Ziel, RZNr)
)

CREATE TABLE Kunden
(
    KNr INTEGER PRIMARY KEY NOT NULL,
    Name VARCHAR(20) NOT NULL,
    Vorname VARCHAR(20) NOT NULL,
    Adresse VARCHAR(20) NOT NULL,
    Ort VARCHAR(20) NOT NULL,
    UNIQUE (Name, Adresse, Ort)
)

CREATE TABLE Fluege
(
    FNr VARCHAR(10) PRIMARY KEY NOT NULL,
    CHECK (len(FNr) = 10),
)

CREATE TABLE Hotels
(
    HNr INTEGER PRIMARY KEY NOT NULL,
    Hotel VARCHAR(20) NOT NULL,
    RZNr INTEGER NOT NULL,
    FOREIGN KEY (RZNr) REFERENCES Reiseziele(RZNr),
    CHECK (Hotel IN ('Hilton', 'Royal', 'Central', 'Aloha', 'Pallas', 'Perle', 'Tropica', 'Mango')),
)


CREATE TABLE Hinfluege
(
    BNr INTEGER NOT NULL,
    FNr VARCHAR(10) NOT NULL,
    HFDat DATE NOT NULL,
    HFZeit TIME NOT NULL,
    PRIMARY KEY (BNr, FNr),
    FOREIGN KEY (BNr) REFERENCES Buchungen(BNr),
    FOREIGN KEY (FNr) REFERENCES Fluege(FNr)
)

CREATE TABLE Rueckfluege
(
    BNr INTEGER NOT NULL,
    FNr VARCHAR(10) NOT NULL,
    RFDat DATE NOT NULL,
    RFZeit TIME NOT NULL,
    PRIMARY KEY (BNr, FNr),
    FOREIGN KEY (BNr) REFERENCES Buchungen(BNr),
    FOREIGN KEY (FNr) REFERENCES Fluege(FNr)
)

-- INSERT statements for Buchungen table
INSERT INTO Buchungen
    (BNr, Buchungsdatum, Preis, Personen, KNr, HNr)
VALUES
    (1, '2022-01-01', 100, 2, 1, 1);

INSERT INTO Buchungen
    (BNr, Buchungsdatum, Preis, Personen, KNr, HNr)
VALUES
    (2, '2022-02-01', 200, 4, 2, 2);

-- INSERT statements for Reiseziele table
INSERT INTO Reiseziele
    (RZNr, Ziel)
VALUES
    (1, 'Paris');

INSERT INTO Reiseziele
    (RZNr, Ziel)
VALUES
    (2, 'London');

-- INSERT statements for Kunden table
INSERT INTO Kunden
    (KNr, Name, Vorname, Adresse, Ort)
VALUES
    (1, 'Müller', 'Hans', 'Musterstraße 1', 'Wien');

INSERT INTO Kunden
    (KNr, Name, Vorname, Adresse, Ort)
VALUES
    (2, 'Schmidt', 'Anna', 'Hauptplatz 2', 'Graz');

-- INSERT statements for Fluege table
INSERT INTO Fluege
    (FNr)
VALUES
    ('ABC1234567'),
    ('DEF4567890');

-- INSERT statements for Hotels table
INSERT INTO Hotels
    (HNr, Hotel, RZNr)
VALUES
    (1, 'Hilton', 1);

INSERT INTO Hotels
    (HNr, Hotel, RZNr)
VALUES
    (2, 'Royal', 2);

-- INSERT statements for Hinfluege table
INSERT INTO Hinfluege
    (BNr, FNr, HFDat, HFZeit)
VALUES
    (1, 'ABC1234567', '2022-01-01', '10:00:00');

INSERT INTO Hinfluege
    (BNr, FNr, HFDat, HFZeit)
VALUES
    (2, 'DEF4567890', '2022-02-01', '12:00:00');

-- INSERT statements for Rueckfluege table
INSERT INTO Rueckfluege
    (BNr, FNr, RFDat, RFZeit)
VALUES
    (1, 'ABC1234567', '2022-01-05', '15:00:00');

INSERT INTO Rueckfluege
    (BNr, FNr, RFDat, RFZeit)
VALUES
    (2, 'DEF4567890', '2022-02-05', '17:00:00');
