CREATE TABLESPACE dwh
    DATAFILE '/Users/fabianoppermann/Developer/dwh01.dbf' SIZE 100M AUTOEXTEND ON
    EXTENT MANAGEMENT LOCAL
    SEGMENT SPACE MANAGEMENT AUTO;

CREATE TEMPORARY TABLESPACE dwh_temp
    TEMPFILE '/Users/fabianoppermann/Developer/dwh_temp01.dbf' SIZE 50M AUTOEXTEND ON;

CREATE USER dwh
    IDENTIFIED BY dwh_passwort
    DEFAULT   TABLESPACE dwh
    TEMPORARY TABLESPACE dwh_temp
    QUOTA UNLIMITED ON dwh;

-- Rechte vergeben
GRANT CREATE SESSION     TO dwh;
GRANT CREATE TABLE       TO dwh;
GRANT CREATE VIEW        TO dwh;
GRANT CREATE MATERIALIZED VIEW TO dwh;
GRANT CREATE SEQUENCE    TO dwh;
GRANT QUERY REWRITE      TO dwh;
GRANT GLOBAL QUERY REWRITE TO dwh;

-- DDL
CREATE TABLE Ort (
    Filiale  VARCHAR2(50) NOT NULL,
    Stadt    VARCHAR2(50) NOT NULL,
    Region   VARCHAR2(50) NOT NULL,
    Land     VARCHAR2(2)  NOT NULL,
    CONSTRAINT pk_ort PRIMARY KEY (Filiale)
) TABLESPACE dwh;

CREATE TABLE Zeit (
    Tag     DATE         NOT NULL,
    Woche   VARCHAR2(10) NOT NULL,  
    Monat   VARCHAR2(10) NOT NULL,  
    Quartal VARCHAR2(10) NOT NULL,  
    Jahr    VARCHAR2(4)  NOT NULL,  
    CONSTRAINT pk_zeit PRIMARY KEY (Tag)
) TABLESPACE dwh;

CREATE TABLE Produkt (
    Produkt       VARCHAR2(50) NOT NULL,
    Marke         VARCHAR2(50) NOT NULL,
    Hersteller    VARCHAR2(50) NOT NULL,
    Produktgruppe VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_produkt PRIMARY KEY (Produkt)
) TABLESPACE dwh;

CREATE TABLE Verkaufszahl (
    Filiale VARCHAR2(50) NOT NULL,
    Produkt VARCHAR2(50) NOT NULL,
    Tag     DATE         NOT NULL,
    Anzahl  NUMBER(6)    NOT NULL,
    CONSTRAINT pk_verkaufszahl  PRIMARY KEY (Filiale, Produkt, Tag),
    CONSTRAINT fk_vk_filiale    FOREIGN KEY (Filiale) REFERENCES Ort(Filiale),
    CONSTRAINT fk_vk_produkt    FOREIGN KEY (Produkt) REFERENCES Produkt(Produkt),
    CONSTRAINT fk_vk_tag        FOREIGN KEY (Tag)     REFERENCES Zeit(Tag)
) TABLESPACE dwh;


-- DML
INSERT INTO Ort VALUES ('Hamburg',     'Hamburg',  'Nord', 'D');
INSERT INTO Ort VALUES ('Leipzig',     'Leipzig',  'Ost',  'D');
INSERT INTO Ort VALUES ('Stuttgart',   'Stuttgart','Süd',  'D');
INSERT INTO Ort VALUES ('Bremen-Nord', 'Bremen',   'Nord', 'D');
INSERT INTO Ort VALUES ('Bremen-Süd',  'Bremen',   'Nord', 'D');
INSERT INTO Ort VALUES ('München',     'München',  'Süd',  'D');

INSERT INTO Zeit VALUES (DATE '2006-01-05', '2006-1',  '2006-1', '2006-Q1', '2006');
INSERT INTO Zeit VALUES (DATE '2006-01-12', '2006-2',  '2006-1', '2006-Q1', '2006');
INSERT INTO Zeit VALUES (DATE '2006-02-13', '2006-7',  '2006-2', '2006-Q1', '2006');
INSERT INTO Zeit VALUES (DATE '2006-02-23', '2006-8',  '2006-2', '2006-Q1', '2006');
INSERT INTO Zeit VALUES (DATE '2006-03-04', '2006-9',  '2006-3', '2006-Q1', '2006');
INSERT INTO Zeit VALUES (DATE '2006-04-07', '2006-14', '2006-4', '2006-Q2', '2006');
INSERT INTO Zeit VALUES (DATE '2006-04-25', '2006-17', '2006-4', '2006-Q2', '2006');

INSERT INTO Produkt VALUES ('Pizza Funghi',   'Gourmet-Pizza', 'Frost GmbH',  'Tiefkühlkost');
INSERT INTO Produkt VALUES ('Pizza Hawaii',   'Gourmet-Pizza', 'Frost GmbH',  'Tiefkühlkost');
INSERT INTO Produkt VALUES ('Pizza Napoli',   'Pizza',         'TK-Pizza AG', 'Tiefkühlkost');
INSERT INTO Produkt VALUES ('Pizza Vegetale', 'Good&Cheap',    'Frost GmbH',  'Tiefkühlkost');
INSERT INTO Produkt VALUES ('Pizza Calzione', 'Pizza',         'TK-Pizza AG', 'Tiefkühlkost');

INSERT INTO Verkaufszahl VALUES ('Hamburg',     'Pizza Funghi',   DATE '2006-01-05', 78);
INSERT INTO Verkaufszahl VALUES ('Hamburg',     'Pizza Funghi',   DATE '2006-01-12', 67);
INSERT INTO Verkaufszahl VALUES ('Leipzig',     'Pizza Hawaii',   DATE '2006-01-12', 42);
INSERT INTO Verkaufszahl VALUES ('München',     'Pizza Calzione', DATE '2006-02-13', 53);
INSERT INTO Verkaufszahl VALUES ('Stuttgart',   'Pizza Napoli',   DATE '2006-02-23', 23);
INSERT INTO Verkaufszahl VALUES ('Bremen-Nord', 'Pizza Funghi',   DATE '2006-03-04', 69);
INSERT INTO Verkaufszahl VALUES ('Bremen-Süd',  'Pizza Vegetale', DATE '2006-04-07', 45);
INSERT INTO Verkaufszahl VALUES ('Stuttgart',   'Pizza Hawaii',   DATE '2006-04-25', 92);

COMMIT;


-- Ohne Materialized view
SELECT O.Region,
       P.Marke,
       Z.Jahr,
       SUM(V.Anzahl) AS Anzahl
FROM Verkaufszahl V
JOIN Ort     O ON V.Filiale = O.Filiale
JOIN Zeit    Z ON V.Tag     = Z.Tag
JOIN Produkt P ON V.Produkt = P.Produkt
GROUP BY O.Region, P.Marke, Z.Jahr
ORDER BY O.Region, P.Marke;


-- Mit Materialized view
CREATE MATERIALIZED VIEW Region_Marke_Jahr
    TABLESPACE dwh
    BUILD IMMEDIATE
    REFRESH COMPLETE ON DEMAND
    ENABLE QUERY REWRITE
AS
SELECT O.Region,
       P.Marke,
       Z.Jahr,
       SUM(V.Anzahl) AS Anzahl
FROM Verkaufszahl V
JOIN Ort     O ON V.Filiale = O.Filiale
JOIN Zeit    Z ON V.Tag     = Z.Tag
JOIN Produkt P ON V.Produkt = P.Produkt
GROUP BY O.Region, P.Marke, Z.Jahr;

SELECT * FROM Region_Marke_Jahr ORDER BY Region, Marke;


-- ============================================================
-- TEIL 6: ABFRAGE MIT QUERY REWRITE

-- Query Rewrite funktioniert nur, wenn
--   1. ENABLE QUERY REWRITE gesetzt ist
--   2. Die Session den Parameter QUERY_REWRITE_ENABLED = TRUE hat
SELECT O.Region,
       Z.Jahr,
       P.Hersteller,
       SUM(RMJ.Anzahl) AS Anzahl
FROM Region_Marke_Jahr RMJ
JOIN Ort     O ON RMJ.Region = O.Region
JOIN Zeit    Z ON RMJ.Jahr   = Z.Jahr
JOIN Produkt P ON RMJ.Marke  = P.Marke
WHERE O.Region = 'Nord'
  AND Z.Jahr   = '2006'
GROUP BY O.Region, Z.Jahr, P.Hersteller
ORDER BY P.Hersteller;

-- Ausführungsplan prüfen
EXPLAIN PLAN FOR
SELECT O.Region,
       Z.Jahr,
       P.Hersteller,
       SUM(V.Anzahl) AS Anzahl
FROM Verkaufszahl V
JOIN Ort     O ON V.Filiale = O.Filiale
JOIN Zeit    Z ON V.Tag     = Z.Tag
JOIN Produkt P ON V.Produkt = P.Produkt
WHERE O.Region = 'Nord'
  AND Z.Jahr   = '2006'
GROUP BY O.Region, Z.Jahr, P.Hersteller;

SELECT * FROM TABLE(DBMS_XPLAN.DISPLAY);

-- ÜBUNG 2: SCHNEEFLOCKENSCHEMA

CREATE TABLE Land_SF (
    Land VARCHAR2(2) NOT NULL,
    CONSTRAINT pk_land_sf PRIMARY KEY (Land)
) TABLESPACE dwh;

CREATE TABLE Region_SF (
    Region VARCHAR2(50) NOT NULL,
    Land   VARCHAR2(2)  NOT NULL,
    CONSTRAINT pk_region_sf PRIMARY KEY (Region),
    CONSTRAINT fk_region_sf_land FOREIGN KEY (Land) REFERENCES Land_SF(Land)
) TABLESPACE dwh;

CREATE TABLE Stadt_SF (
    Stadt  VARCHAR2(50) NOT NULL,
    Region VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_stadt_sf PRIMARY KEY (Stadt),
    CONSTRAINT fk_stadt_sf_region FOREIGN KEY (Region) REFERENCES Region_SF(Region)
) TABLESPACE dwh;

CREATE TABLE Filiale_SF (
    Filiale VARCHAR2(50) NOT NULL,
    Stadt   VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_filiale_sf PRIMARY KEY (Filiale),
    CONSTRAINT fk_filiale_sf_stadt FOREIGN KEY (Stadt) REFERENCES Stadt_SF(Stadt)
) TABLESPACE dwh;

CREATE TABLE Produktgruppe_SF (
    Produktgruppe VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_produktgruppe_sf PRIMARY KEY (Produktgruppe)
) TABLESPACE dwh;

CREATE TABLE Hersteller_SF (
    Hersteller    VARCHAR2(50) NOT NULL,
    Produktgruppe VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_hersteller_sf PRIMARY KEY (Hersteller),
    CONSTRAINT fk_hersteller_sf_pg FOREIGN KEY (Produktgruppe) REFERENCES Produktgruppe_SF(Produktgruppe)
) TABLESPACE dwh;

CREATE TABLE Marke_SF (
    Marke       VARCHAR2(50) NOT NULL,
    Hersteller  VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_marke_sf PRIMARY KEY (Marke),
    CONSTRAINT fk_marke_sf_hersteller FOREIGN KEY (Hersteller) REFERENCES Hersteller_SF(Hersteller)
) TABLESPACE dwh;

CREATE TABLE Produkt_SF (
    Produkt VARCHAR2(50) NOT NULL,
    Marke   VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_produkt_sf PRIMARY KEY (Produkt),
    CONSTRAINT fk_produkt_sf_marke FOREIGN KEY (Marke) REFERENCES Marke_SF(Marke)
) TABLESPACE dwh;

CREATE TABLE Jahr_SF (
    Jahr VARCHAR2(4) NOT NULL,
    CONSTRAINT pk_jahr_sf PRIMARY KEY (Jahr)
) TABLESPACE dwh;

CREATE TABLE Quartal_SF (
    Quartal VARCHAR2(10) NOT NULL,
    Jahr    VARCHAR2(4)  NOT NULL,
    CONSTRAINT pk_quartal_sf PRIMARY KEY (Quartal),
    CONSTRAINT fk_quartal_sf_jahr FOREIGN KEY (Jahr) REFERENCES Jahr_SF(Jahr)
) TABLESPACE dwh;

CREATE TABLE Monat_SF (
    Monat   VARCHAR2(10) NOT NULL,
    Quartal VARCHAR2(10) NOT NULL,
    CONSTRAINT pk_monat_sf PRIMARY KEY (Monat),
    CONSTRAINT fk_monat_sf_quartal FOREIGN KEY (Quartal) REFERENCES Quartal_SF(Quartal)
) TABLESPACE dwh;

CREATE TABLE Woche_SF (
    Woche VARCHAR2(10) NOT NULL,
    Monat VARCHAR2(10) NOT NULL,
    CONSTRAINT pk_woche_sf PRIMARY KEY (Woche),
    CONSTRAINT fk_woche_sf_monat FOREIGN KEY (Monat) REFERENCES Monat_SF(Monat)
) TABLESPACE dwh;

CREATE TABLE Tag_SF (
    Tag   DATE         NOT NULL,
    Woche VARCHAR2(10) NOT NULL,
    CONSTRAINT pk_tag_sf PRIMARY KEY (Tag),
    CONSTRAINT fk_tag_sf_woche FOREIGN KEY (Woche) REFERENCES Woche_SF(Woche)
) TABLESPACE dwh;

CREATE TABLE Verkauf_SF (
    Filiale VARCHAR2(50) NOT NULL,
    Produkt VARCHAR2(50) NOT NULL,
    Tag     DATE         NOT NULL,
    Anzahl  NUMBER(6)    NOT NULL,
    CONSTRAINT pk_verkauf_sf PRIMARY KEY (Filiale, Produkt, Tag),
    CONSTRAINT fk_verkauf_sf_filiale FOREIGN KEY (Filiale) REFERENCES Filiale_SF(Filiale),
    CONSTRAINT fk_verkauf_sf_produkt FOREIGN KEY (Produkt) REFERENCES Produkt_SF(Produkt),
    CONSTRAINT fk_verkauf_sf_tag     FOREIGN KEY (Tag)     REFERENCES Tag_SF(Tag)
) TABLESPACE dwh;


INSERT INTO Land_SF (Land)
SELECT DISTINCT Land
FROM Ort;

INSERT INTO Region_SF (Region, Land)
SELECT DISTINCT Region, Land
FROM Ort;

INSERT INTO Stadt_SF (Stadt, Region)
SELECT DISTINCT Stadt, Region
FROM Ort;

INSERT INTO Filiale_SF (Filiale, Stadt)
SELECT DISTINCT Filiale, Stadt
FROM Ort;

INSERT INTO Produktgruppe_SF (Produktgruppe)
SELECT DISTINCT Produktgruppe
FROM Produkt;

INSERT INTO Hersteller_SF (Hersteller, Produktgruppe)
SELECT DISTINCT Hersteller, Produktgruppe
FROM Produkt;

INSERT INTO Marke_SF (Marke, Hersteller)
SELECT DISTINCT Marke, Hersteller
FROM Produkt;

INSERT INTO Produkt_SF (Produkt, Marke)
SELECT DISTINCT Produkt, Marke
FROM Produkt;

INSERT INTO Jahr_SF (Jahr)
SELECT DISTINCT Jahr
FROM Zeit;

INSERT INTO Quartal_SF (Quartal, Jahr)
SELECT DISTINCT Quartal, Jahr
FROM Zeit;

INSERT INTO Monat_SF (Monat, Quartal)
SELECT DISTINCT Monat, Quartal
FROM Zeit;

INSERT INTO Woche_SF (Woche, Monat)
SELECT DISTINCT Woche, Monat
FROM Zeit;

INSERT INTO Tag_SF (Tag, Woche)
SELECT DISTINCT Tag, Woche
FROM Zeit;

INSERT INTO Verkauf_SF (Filiale, Produkt, Tag, Anzahl)
SELECT Filiale, Produkt, Tag, Anzahl
FROM Verkaufszahl;

COMMIT;


SELECT r.Region,
       m.Marke,
       j.Jahr,
       SUM(v.Anzahl) AS Anzahl
FROM Verkauf_SF v
JOIN Filiale_SF f        ON v.Filiale = f.Filiale
JOIN Stadt_SF s          ON f.Stadt   = s.Stadt
JOIN Region_SF r         ON s.Region  = r.Region
JOIN Produkt_SF p        ON v.Produkt  = p.Produkt
JOIN Marke_SF m          ON p.Marke   = m.Marke
JOIN Hersteller_SF h     ON m.Hersteller = h.Hersteller
JOIN Produktgruppe_SF pg ON h.Produktgruppe = pg.Produktgruppe
JOIN Tag_SF t            ON v.Tag     = t.Tag
JOIN Woche_SF w          ON t.Woche   = w.Woche
JOIN Monat_SF mo         ON w.Monat   = mo.Monat
JOIN Quartal_SF q        ON mo.Quartal = q.Quartal
JOIN Jahr_SF j           ON q.Jahr    = j.Jahr
GROUP BY r.Region, m.Marke, j.Jahr
ORDER BY r.Region, m.Marke;


-- Teil 4: Unterschied Sternschema vs. Schneeflockenschema:
-- Sternschema: Dimensionen sind eher denormalisiert, deshalb weniger Joins und oft schneller.
-- Schneeflockenschema: Dimensionen sind normalisiert, deshalb mehr Joins aber weniger Redundanz.
-- Also: Stern ist eher praktisch für Auswertungen, Schneeflocke ist sauberer aufgebaut.
