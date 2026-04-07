-- ÜBUNG 3: STAR QUERY



-- 1) Star Query (S. 459): Region Nord, Q1+Q2, Marken Gourmet-Pizza/Good&Cheap
SELECT O.Region,
       Z.Quartal,
       P.Marke,
       SUM(V.Anzahl) AS Anzahl
FROM Verkaufszahl V
JOIN Ort     O ON V.Filiale = O.Filiale
JOIN Zeit    Z ON V.Tag     = Z.Tag
JOIN Produkt P ON V.Produkt = P.Produkt
WHERE O.Region = 'Nord'
  AND Z.Quartal IN ('2006-Q1', '2006-Q2')
  AND P.Marke IN ('Gourmet-Pizza', 'Good&Cheap')
GROUP BY O.Region, Z.Quartal, P.Marke;


-- 2) Bitvektoren: pro Bedingung ein Bit (0/1) je Zeile
SELECT V.Filiale, V.Produkt, V.Tag, V.Anzahl,
       CASE WHEN P.Marke = 'Gourmet-Pizza' THEN 1 ELSE 0 END AS B_GourmetPizza,
       CASE WHEN P.Marke = 'Good&Cheap'    THEN 1 ELSE 0 END AS B_GoodCheap,
       CASE WHEN O.Region = 'Nord'          THEN 1 ELSE 0 END AS B_Nord,
       CASE WHEN Z.Quartal = '2006-Q1'      THEN 1 ELSE 0 END AS B_Q1_2006,
       CASE WHEN Z.Quartal = '2006-Q2'      THEN 1 ELSE 0 END AS B_Q2_2006
FROM Verkaufszahl V
JOIN Ort     O ON V.Filiale = O.Filiale
JOIN Zeit    Z ON V.Tag     = Z.Tag
JOIN Produkt P ON V.Produkt = P.Produkt
ORDER BY V.Tag, V.Filiale;


-- 3) Bitvektoren verknüpfen: BRes = (B_Gourmet OR B_Good) AND B_Nord AND (B_Q1 OR B_Q2)
SELECT V.Filiale, V.Produkt, V.Tag, V.Anzahl,
       CASE WHEN P.Marke = 'Gourmet-Pizza' THEN 1 ELSE 0 END AS B_GourmetPizza,
       CASE WHEN P.Marke = 'Good&Cheap'    THEN 1 ELSE 0 END AS B_GoodCheap,
       CASE WHEN O.Region = 'Nord'          THEN 1 ELSE 0 END AS B_Nord,
       CASE WHEN Z.Quartal = '2006-Q1'      THEN 1 ELSE 0 END AS B_Q1_2006,
       CASE WHEN Z.Quartal = '2006-Q2'      THEN 1 ELSE 0 END AS B_Q2_2006,
       -- Ergebnis-Bitvektor
       CASE
           WHEN P.Marke IN ('Gourmet-Pizza', 'Good&Cheap')
            AND O.Region = 'Nord'
            AND Z.Quartal IN ('2006-Q1', '2006-Q2')
           THEN 1 ELSE 0
       END AS B_Res
FROM Verkaufszahl V
JOIN Ort     O ON V.Filiale = O.Filiale
JOIN Zeit    Z ON V.Tag     = Z.Tag
JOIN Produkt P ON V.Produkt = P.Produkt
ORDER BY V.Tag, V.Filiale;


-- 4) Faktentabelle filtern via Semi-Joins (simuliert Bitmap-Filterung)
SELECT V.Filiale, V.Produkt, V.Tag, V.Anzahl
FROM Verkaufszahl V
WHERE V.Filiale IN (SELECT Filiale FROM Ort     WHERE Region  = 'Nord')
  AND V.Produkt IN (SELECT Produkt FROM Produkt WHERE Marke   IN ('Gourmet-Pizza', 'Good&Cheap'))
  AND V.Tag     IN (SELECT Tag     FROM Zeit    WHERE Quartal IN ('2006-Q1', '2006-Q2'));


-- 5) Aggregation -> gleiches Ergebnis wie Schritt 1
SELECT O.Region,
       Z.Quartal,
       P.Marke,
       SUM(V.Anzahl) AS Anzahl
FROM Verkaufszahl V
JOIN Ort     O ON V.Filiale = O.Filiale
JOIN Zeit    Z ON V.Tag     = Z.Tag
JOIN Produkt P ON V.Produkt = P.Produkt
WHERE V.Filiale IN (SELECT Filiale FROM Ort     WHERE Region  = 'Nord')
  AND V.Produkt IN (SELECT Produkt FROM Produkt WHERE Marke   IN ('Gourmet-Pizza', 'Good&Cheap'))
  AND V.Tag     IN (SELECT Tag     FROM Zeit    WHERE Quartal IN ('2006-Q1', '2006-Q2'))
GROUP BY O.Region, Z.Quartal, P.Marke;


-- ÜBUNG 4: PARTITIONIEREN

-- a) Doku: Oracle VLDB and Partitioning Guide
-- b) Doku: CREATE TABLE ... PARTITION BY RANGE|LIST|HASH, VALUES LESS THAN, MAXVALUE
-- c) Bereichspartitionierung nach Tag (S. 455)
CREATE TABLE Verkaufszahl_ (
    Filiale VARCHAR2(50) NOT NULL,
    Produkt VARCHAR2(50) NOT NULL,
    Tag     DATE         NOT NULL,
    Anzahl  NUMBER(6)    NOT NULL,
    CONSTRAINT pk_verkaufszahl_  PRIMARY KEY (Filiale, Produkt, Tag),
    CONSTRAINT fk_vk_p_filiale  FOREIGN KEY (Filiale) REFERENCES Ort(Filiale),
    CONSTRAINT fk_vk_p_produkt  FOREIGN KEY (Produkt) REFERENCES Produkt(Produkt),
    CONSTRAINT fk_vk_p_tag      FOREIGN KEY (Tag)     REFERENCES Zeit(Tag)
)
PARTITION BY RANGE (Tag) (
    PARTITION VerkaufVor2005   VALUES LESS THAN (DATE '2005-01-01'),
    PARTITION Verkauf2005      VALUES LESS THAN (DATE '2006-01-01'),
    PARTITION Verkauf2006      VALUES LESS THAN (DATE '2007-01-01'),
    PARTITION VerkaufNach2006  VALUES LESS THAN (MAXVALUE)
) TABLESPACE dwh;

-- d) Daten übernehmen
INSERT INTO Verkaufszahl_ (Filiale, Produkt, Tag, Anzahl)
SELECT Filiale, Produkt, Tag, Anzahl
FROM Verkaufszahl;

COMMIT;

-- e) Partitionen im Systemkatalog
SELECT table_name,
       partition_name,
       high_value,
       partition_position,
       num_rows
FROM user_tab_partitions
WHERE table_name = 'VERKAUFSZAHL_'
ORDER BY partition_position;

-- Anzahl pro Partition
SELECT 'VerkaufVor2005'  AS Partition_Name, COUNT(*) AS Anzahl FROM Verkaufszahl_ PARTITION (VerkaufVor2005)
UNION ALL
SELECT 'Verkauf2005',     COUNT(*) FROM Verkaufszahl_ PARTITION (Verkauf2005)
UNION ALL
SELECT 'Verkauf2006',     COUNT(*) FROM Verkaufszahl_ PARTITION (Verkauf2006)
UNION ALL
SELECT 'VerkaufNach2006', COUNT(*) FROM Verkaufszahl_ PARTITION (VerkaufNach2006);
