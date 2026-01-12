CREATE DATABASE Aufgabe3;
USE Aufgabe3;

DROP TABLE IF EXISTS Inspektionen;
DROP TABLE IF EXISTS Vermietungen;
DROP TABLE IF EXISTS Kunden;
DROP TABLE IF EXISTS Fahrzeuge;
DROP TABLE IF EXISTS Hersteller;

DROP INDEX IF EXISTS idx_Hersteller_Name ON Hersteller;
DROP INDEX IF EXISTS idx_Fahrzeuge_Modell ON Fahrzeuge;
DROP INDEX IF EXISTS idx_Kunden_Name ON Kunden;
DROP INDEX IF EXISTS idx_Vermietungen_Startdatum ON Vermietungen;
DROP INDEX IF EXISTS idx_Inspektionen_Datum ON Inspektionen;

CREATE TABLE Hersteller
(
    HerstellerID INTEGER,
    Name VARCHAR(100) UNIQUE,
    Land VARCHAR(100)
);

CREATE TABLE Fahrzeuge
(
    FahrzeugID INTEGER,
    Modell VARCHAR(100),
    HerstellerID INTEGER,
    Baujahr INTEGER,
    Kilometerstand INTEGER,
    CHECK (Kilometerstand > 0),
    CHECK (Baujahr > 1945)
);

CREATE TABLE Kunden
(
    KundenID INTEGER,
    Name VARCHAR(100),
    Email VARCHAR(255),
    Telefonnummer VARCHAR(15),
    UNIQUE (Email),
    UNIQUE (Telefonnummer)
);

CREATE TABLE Vermietungen
(
    VermietungID INTEGER,
    KundenID INTEGER,
    FahrzeugID INTEGER,
    Startdatum Date,
    Enddatum Date,
    Preis FLOAT,
    CHECK (Preis > 0),
    CHECK (Startdatum < Enddatum)
);

CREATE TABLE Inspektionen
(
    InspektionID INTEGER,
    FahrzeugID INTEGER,
    Datum Date Not Null,
    Beschreibung VARCHAR(100),
    Kosten FLOAT,
    CHECK (Kosten > 0)
);

CREATE INDEX idx_Hersteller_Name ON Hersteller (Name);
CREATE INDEX idx_Fahrzeuge_Modell ON Fahrzeuge (Modell);
CREATE INDEX idx_Kunden_Name ON Kunden (Name);
CREATE INDEX idx_Vermietungen_Startdatum ON Vermietungen (Startdatum);
CREATE INDEX idx_Inspektionen_Datum ON Inspektionen (Datum);

INSERT INTO Hersteller
    (HerstellerID, Name, Land)
VALUES
    (1, 'Tesla', 'USA'),
    (2, 'BMW', 'Deutschland'),
    (3, 'Toyota', 'Japan'),
    (4, 'Ford', 'USA'),
    (5, 'Volkswagen', 'Deutschland'),
    (6, 'Audi', 'Deutschland'),
    (7, 'Hyundai', 'S�dkorea'),
    (8, 'Mercedes-Benz', 'Deutschland'),
    (9, 'Kia', 'S�dkorea'),
    (10, 'Nissan', 'Japan');

INSERT INTO Fahrzeuge
    (FahrzeugID, Modell, HerstellerID, Baujahr, Kilometerstand)
VALUES
    (1, 'Model S', 1, 2021, 5000),
    (2, 'Model X', 1, 2021, 15000),
    (3, 'Model Y', 1, 2022, 1000),
    (4, '3er', 2, 2019, 20000),
    (5, '5er', 2, 2018, 25000),
    (6, '7er', 2, 2022, 500),
    (7, 'Camry', 3, 2018, 35000),
    (8, 'Corolla', 3, 2021, 12000),
    (9, 'Prius', 3, 2019, 20000),
    (10, 'Fiesta', 4, 2020, 15000),
    (11, 'Focus', 4, 2021, 9000),
    (12, 'Mustang', 4, 2022, 3000),
    (13, 'Golf', 5, 2017, 40000),
    (14, 'Passat', 5, 2018, 31000),
    (15, 'Polo', 5, 2021, 7000),
    (16, 'A4', 6, 2022, 1000),
    (17, 'A6', 6, 2020, 13000),
    (18, 'A8', 6, 2021, 5000),
    (19, 'Q2', 6, 2016, 35000),
    (20, 'Santa Fe', 7, 2020, 18000);

INSERT INTO Kunden
    (KundenID, Name, Email, Telefonnummer)
VALUES
    (1, 'Anna M�ller', 'anna.mueller@email.com', '0123456789'),
    (2, 'Bert Schmidt', 'bert.schmidt@email.com', '0123456790'),
    (3, 'Cara Klein', 'cara.klein@email.com', '0123456791'),
    (4, 'Dennis Gross', 'dennis.gross@email.com', '0123456792'),
    (5, 'Eva Schmidt', 'eva.schmidt@email.com', '0123456793');

INSERT INTO Vermietungen
    (VermietungID, KundenID, FahrzeugID, Startdatum, Enddatum, Preis)
VALUES
    (1, 1, 1, '2023-05-01', '2023-05-07', 500),
    (2, 2, 20, '2023-05-03', '2023-05-10', 700),
    (3, 3, 3, '2023-05-05', '2023-05-12', 650),
    (4, 4, 4, '2023-05-08', '2023-05-15', 450),
    (5, 5, 15, '2023-05-10', '2023-05-17', 750),
    (6, 1, 6, '2023-05-15', '2023-05-22', 800),
    (7, 2, 7, '2023-05-20', '2023-05-27', 600),
    (8, 3, 8, '2023-05-22', '2023-05-29', 900),
    (9, 4, 19, '2023-05-25', '2023-05-30', 550),
    (10, 5, 10, '2023-05-28', '2023-06-04', 700);

INSERT INTO Inspektionen
    (InspektionID, FahrzeugID, Datum, Beschreibung, Kosten)
VALUES
    (1, 1, '2023-04-10', 'Jahresinspektion', 300),
    (2, 12, '2023-04-11', 'Bremsen erneuern', 500),
    (3, 13, '2023-04-12', '�lwechsel', 100),
    (4, 4, '2023-04-15', 'Reifenwechsel', 400),
    (5, 5, '2023-04-20', 'Jahresinspektion', 350),
    (6, 16, '2023-04-21', 'Bremsen erneuern', 500),
    (7, 17, '2023-04-25', 'Licht einstellen', 50),
    (8, 8, '2023-04-28', 'Bremsfl�ssigkeit wechseln', 150),
    (9, 9, '2023-04-29', 'Jahresinspektion', 320),
    (10, 20, '2023-04-30', 'Keilriemen erneuern', 180);