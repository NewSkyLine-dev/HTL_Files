CREATE DATABASE SchulDB;
USE SchulDB;

DROP TABLE Fach;
DROP TABLE Lehrkraft;
DROP TABLE Klasse;

CREATE TABLE Unterricht
(
    persnr INT,
    kuerzel VARCHAR(10),
    name VARCHAR(10),
    stundenanzahl INT,
    PRIMARY KEY (persnr, kuerzel, name),
    FOREIGN KEY (persnr) REFERENCES Lehrkraft(persnr),
    FOREIGN KEY (kuerzel) REFERENCES Fach(kuerzel),
    FOREIGN KEY (name) REFERENCES Klasse (name)
)

CREATE TABLE Fach
(
    kuerzel VARCHAR(10) PRIMARY KEY,
    name VARCHAR(20)
)

CREATE TABLE Lehrkraft
(
    persnr INT PRIMARY KEY,
    name VARCHAR(20),
    geschl VARCHAR(1),
    wohnort VARCHAR(20),
    geb_jarh INT
)

CREATE TABLE Klasse
(
    name VARCHAR(10) PRIMARY KEY,
    zimmer VARCHAR(10),
    persnr INT
)