CREATE TABLESPACE dwh
    EXTENT MANAGEMENT LOCAL
    SEGMENT SPACE MANAGEMENT AUTO;

CREATE TEMPORARY TABLESPACE dwh_temp;

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
