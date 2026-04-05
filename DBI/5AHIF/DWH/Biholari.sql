-- ÜBUNG 2: Schneeflockenschema
CREATE TABLE Land_SF (
    Land VARCHAR2(2) NOT NULL,
    CONSTRAINT pk_land_sf PRIMARY KEY (Land)
) TABLESPACE dwh;

CREATE TABLE Region_SF (
    Region VARCHAR2(50) NOT NULL,
    Land   VARCHAR2(2)  NOT NULL,
    CONSTRAINT pk_region_sf PRIMARY KEY (Region),
    CONSTRAINT fk_region_sf_land FOREIGN KEY (Land) REFERENCES Land_SF (Land)
) TABLESPACE dwh;

CREATE TABLE Stadt_SF (
    Stadt  VARCHAR2(50) NOT NULL,
    Region VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_stadt_sf PRIMARY KEY (Stadt),
    CONSTRAINT fk_stadt_sf_region FOREIGN KEY (Region) REFERENCES Region_SF (Region)
) TABLESPACE dwh;

CREATE TABLE Filiale_SF (
    Filiale VARCHAR2(50) NOT NULL,
    Stadt   VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_filiale_sf PRIMARY KEY (Filiale),
    CONSTRAINT fk_filiale_sf_stadt FOREIGN KEY (Stadt) REFERENCES Stadt_SF (Stadt)
) TABLESPACE dwh;

CREATE TABLE Produktgruppe_SF (
    Produktgruppe VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_produktgruppe_sf PRIMARY KEY (Produktgruppe)
) TABLESPACE dwh;

CREATE TABLE Hersteller_SF (
    Hersteller    VARCHAR2(50) NOT NULL,
    Produktgruppe VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_hersteller_sf PRIMARY KEY (Hersteller),
    CONSTRAINT fk_hersteller_sf_pg FOREIGN KEY (Produktgruppe) REFERENCES Produktgruppe_SF (Produktgruppe)
) TABLESPACE dwh;

CREATE TABLE Marke_SF (
    Marke      VARCHAR2(50) NOT NULL,
    Hersteller VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_marke_sf PRIMARY KEY (Marke),
    CONSTRAINT fk_marke_sf_hersteller FOREIGN KEY (Hersteller) REFERENCES Hersteller_SF (Hersteller)
) TABLESPACE dwh;

CREATE TABLE Produkt_SF (
    Produkt VARCHAR2(50) NOT NULL,
    Marke   VARCHAR2(50) NOT NULL,
    CONSTRAINT pk_produkt_sf PRIMARY KEY (Produkt),
    CONSTRAINT fk_produkt_sf_marke FOREIGN KEY (Marke) REFERENCES Marke_SF (Marke)
) TABLESPACE dwh;

CREATE TABLE Jahr_SF (
    Jahr VARCHAR2(4) NOT NULL,
    CONSTRAINT pk_jahr_sf PRIMARY KEY (Jahr)
) TABLESPACE dwh;

CREATE TABLE Quartal_SF (
    Quartal VARCHAR2(10) NOT NULL,
    Jahr    VARCHAR2(4)  NOT NULL,
    CONSTRAINT pk_quartal_sf PRIMARY KEY (Quartal),
    CONSTRAINT fk_quartal_sf_jahr FOREIGN KEY (Jahr) REFERENCES Jahr_SF (Jahr)
) TABLESPACE dwh;

CREATE TABLE Monat_SF (
    Monat   VARCHAR2(10) NOT NULL,
    Quartal VARCHAR2(10) NOT NULL,
    CONSTRAINT pk_monat_sf PRIMARY KEY (Monat),
    CONSTRAINT fk_monat_sf_quartal FOREIGN KEY (Quartal) REFERENCES Quartal_SF (Quartal)
) TABLESPACE dwh;

CREATE TABLE Woche_SF (
    Woche VARCHAR2(10) NOT NULL,
    Monat VARCHAR2(10) NOT NULL,
    CONSTRAINT pk_woche_sf PRIMARY KEY (Woche),
    CONSTRAINT fk_woche_sf_monat FOREIGN KEY (Monat) REFERENCES Monat_SF (Monat)
) TABLESPACE dwh;

CREATE TABLE Tag_SF (
    Tag   DATE         NOT NULL,
    Woche VARCHAR2(10) NOT NULL,
    CONSTRAINT pk_tag_sf PRIMARY KEY (Tag),
    CONSTRAINT fk_tag_sf_woche FOREIGN KEY (Woche) REFERENCES Woche_SF (Woche)
) TABLESPACE dwh;

CREATE TABLE Verkauf_SF (
    Filiale VARCHAR2(50) NOT NULL,
    Produkt VARCHAR2(50) NOT NULL,
    Tag     DATE         NOT NULL,
    Anzahl  NUMBER(6)    NOT NULL,
    CONSTRAINT pk_verkauf_sf PRIMARY KEY (Filiale, Produkt, Tag),
    CONSTRAINT fk_verkauf_sf_filiale FOREIGN KEY (Filiale) REFERENCES Filiale_SF (Filiale),
    CONSTRAINT fk_verkauf_sf_produkt FOREIGN KEY (Produkt) REFERENCES Produkt_SF (Produkt),
    CONSTRAINT fk_verkauf_sf_tag FOREIGN KEY (Tag) REFERENCES Tag_SF (Tag)
) TABLESPACE dwh;

-- Daten aus dem Sternschema uebernehmen
INSERT INTO Land_SF (Land)
SELECT DISTINCT o.Land
FROM Ort o;

INSERT INTO Region_SF (Region, Land)
SELECT DISTINCT o.Region, o.Land
FROM Ort o;

INSERT INTO Stadt_SF (Stadt, Region)
SELECT DISTINCT o.Stadt, o.Region
FROM Ort o;

INSERT INTO Filiale_SF (Filiale, Stadt)
SELECT DISTINCT o.Filiale, o.Stadt
FROM Ort o;

INSERT INTO Produktgruppe_SF (Produktgruppe)
SELECT DISTINCT p.Produktgruppe
FROM Produkt p;

INSERT INTO Hersteller_SF (Hersteller, Produktgruppe)
SELECT DISTINCT p.Hersteller, p.Produktgruppe
FROM Produkt p;

INSERT INTO Marke_SF (Marke, Hersteller)
SELECT DISTINCT p.Marke, p.Hersteller
FROM Produkt p;

INSERT INTO Produkt_SF (Produkt, Marke)
SELECT DISTINCT p.Produkt, p.Marke
FROM Produkt p;

INSERT INTO Jahr_SF (Jahr)
SELECT DISTINCT z.Jahr
FROM Zeit z;

INSERT INTO Quartal_SF (Quartal, Jahr)
SELECT DISTINCT z.Quartal, z.Jahr
FROM Zeit z;

INSERT INTO Monat_SF (Monat, Quartal)
SELECT DISTINCT z.Monat, z.Quartal
FROM Zeit z;

INSERT INTO Woche_SF (Woche, Monat)
SELECT DISTINCT z.Woche, z.Monat
FROM Zeit z;

INSERT INTO Tag_SF (Tag, Woche)
SELECT DISTINCT z.Tag, z.Woche
FROM Zeit z;

INSERT INTO Verkauf_SF (Filiale, Produkt, Tag, Anzahl)
SELECT v.Filiale, v.Produkt, v.Tag, v.Anzahl
FROM Verkaufszahl v;

COMMIT;

-- Beispielabfrage im Schneeflockenschema
SELECT r.Region,
       m.Marke,
       j.Jahr,
       SUM(v.Anzahl) AS Anzahl
FROM Verkauf_SF v
JOIN Filiale_SF f ON f.Filiale = v.Filiale
JOIN Stadt_SF s ON s.Stadt = f.Stadt
JOIN Region_SF r ON r.Region = s.Region
JOIN Produkt_SF p ON p.Produkt = v.Produkt
JOIN Marke_SF m ON m.Marke = p.Marke
JOIN Hersteller_SF h ON h.Hersteller = m.Hersteller
JOIN Produktgruppe_SF pg ON pg.Produktgruppe = h.Produktgruppe
JOIN Tag_SF t ON t.Tag = v.Tag
JOIN Woche_SF w ON w.Woche = t.Woche
JOIN Monat_SF mo ON mo.Monat = w.Monat
JOIN Quartal_SF q ON q.Quartal = mo.Quartal
JOIN Jahr_SF j ON j.Jahr = q.Jahr
GROUP BY r.Region, m.Marke, j.Jahr
ORDER BY r.Region, m.Marke, j.Jahr;

-- Kurzvergleich
-- Sternschema: Dimensionen sind eher denormalisiert, daher weniger Joins und oft schneller.
-- Schneeflockenschema: Dimensionen sind normalisiert, daher mehr Joins aber weniger Redundanz.


-- ÜBUNG 7:
-- a):
CREATE TABLE ort_produkt_monat_verkauf (
    ort     VARCHAR2(50) NOT NULL,
    produkt VARCHAR2(50) NOT NULL,
    monat   VARCHAR2(10) NOT NULL,
    verkauf NUMBER(10)   NOT NULL,
    CONSTRAINT pk_opmv PRIMARY KEY (ort, produkt, monat)
) TABLESPACE dwh;

INSERT INTO ort_produkt_monat_verkauf (ort, produkt, monat, verkauf)
SELECT o.Stadt AS ort,
       p.Produkt AS produkt,
       z.Monat AS monat,
       SUM(v.Anzahl) AS verkauf
FROM Verkaufszahl v
JOIN Ort o ON o.Filiale = v.Filiale
JOIN Produkt p ON p.Produkt = v.Produkt
JOIN Zeit z ON z.Tag = v.Tag
GROUP BY o.Stadt, p.Produkt, z.Monat;

COMMIT;

-- b):
SELECT t.ort,
       t.produkt,
       t.monat,
       SUM(t.verkauf) AS summe_verkauf
FROM ort_produkt_monat_verkauf t
GROUP BY t.ort, t.produkt, t.monat
ORDER BY t.ort, t.produkt, t.monat;

-- c):
SELECT DECODE(GROUPING(t.ort), 1, 'ALLE_ORTE', t.ort) AS ort,
       DECODE(GROUPING(t.produkt), 1, 'ALLE_PRODUKTE', t.produkt) AS produkt,
       DECODE(GROUPING(t.monat), 1, 'ALLE_MONATE', t.monat) AS monat,
       SUM(t.verkauf) AS summe_verkauf
FROM ort_produkt_monat_verkauf t
GROUP BY ROLLUP (t.ort, t.produkt, t.monat)
ORDER BY t.ort, t.produkt, t.monat;

-- d):
SELECT DECODE('A', 'A', 'Treffer', 'Kein Treffer') AS decode_beispiel
FROM dual;

SELECT CASE
           WHEN GROUPING(t.ort) = 1 THEN 'ALLE_ORTE'
           ELSE t.ort
       END AS ort,
       CASE
           WHEN GROUPING(t.produkt) = 1 THEN 'ALLE_PRODUKTE'
           ELSE t.produkt
       END AS produkt,
       CASE
           WHEN GROUPING(t.monat) = 1 THEN 'ALLE_MONATE'
           ELSE t.monat
       END AS monat,
       SUM(t.verkauf) AS summe_verkauf
FROM ort_produkt_monat_verkauf t
GROUP BY ROLLUP (t.ort, t.produkt, t.monat)
ORDER BY t.ort, t.produkt, t.monat;

-- e):
SELECT t.ort,
       t.produkt,
       t.monat,
       GROUPING(t.ort) AS grp_ort,
       GROUPING(t.produkt) AS grp_produkt,
       GROUPING(t.monat) AS grp_monat,
       SUM(t.verkauf) AS summe_verkauf
FROM ort_produkt_monat_verkauf t
GROUP BY ROLLUP (t.ort, t.produkt, t.monat)
ORDER BY t.ort, t.produkt, t.monat;