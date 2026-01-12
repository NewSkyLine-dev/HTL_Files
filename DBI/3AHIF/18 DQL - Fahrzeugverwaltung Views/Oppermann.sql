USE Fahrzeugverwaltung;

-- 1
GO
CREATE VIEW vVermietungen
AS
    SELECT k.Name AS KundenName, k.Email, h.Name AS HerstellerName, h.Land, f.Modell, f.Baujahr, f.Kilometerstand, v.Startdatum, v.Enddatum, v.Preis
    FROM Kunden K
        JOIN Vermietungen v ON v.KundenID = k.KundenID
        JOIN Fahrzeuge f ON f.FahrzeugID = v.FahrzeugID
        JOIN Hersteller h ON h.HerstellerID = f.HerstellerID;
GO;
-- 2
GO
CREATE VIEW vInspektionen
AS
    SELECT h.Name, h.Land, f.Modell, f.Baujahr, f.Kilometerstand, i.Datum, i.Kosten, i.Beschreibung
    FROM Fahrzeuge f
        JOIN Inspektionen i ON i.FahrzeugID = f.FahrzeugID
        JOIN Hersteller h ON h.HerstellerID = f.HerstellerID;
GO;

-- 1
SELECT vV.HerstellerName, COUNT(*) AS Anzahl, SUM(vv.Preis) AS Preis
FROM vVermietungen vV
GROUP BY vv.HerstellerName;

-- 2
SELECT vV.KundenName, vV.Email, vV.Modell, vV.Startdatum, vV.Enddatum
FROM vVermietungen vV
WHERE DATEDIFF(DAY, vV.Startdatum, vV.Enddatum) >= 7;

-- 3
SELECT vI.Name, COUNT(*) AS Anzahl, SUM(vI.Kosten) AS Kosten
FROM vInspektionen vI
GROUP BY vI.Name;

-- 4
SELECT TOP 3
    vV.KundenName,
    COUNT(*)
FROM vVermietungen vV
WHERE vV.Kilometerstand > 4999 AND vV.Land IN ('USA', 'Deutschland')
GROUP BY vV.KundenName
ORDER BY COUNT(*);

-- 5
-- a
SELECT f.Modell, f.Baujahr, f.Kilometerstand
FROM Fahrzeuge f
WHERE NOT EXISTS (
    SELECT *
FROM Vermietungen v
WHERE v.FahrzeugID = f.FahrzeugID
);

-- b (JOIN)
SELECT f.Modell, f.Baujahr, f.Kilometerstand
FROM Fahrzeuge f
    LEFT JOIN Vermietungen v ON v.FahrzeugID = f.FahrzeugID
WHERE v.FahrzeugID IS NULL;

-- c
/*
Ich würde sagen NEIN. Da die VIEW's zu viele Informationen enthalten, die nicht benötigt werden.
*/

-- 6
-- a
SELECT vV.KundenName, vI.Beschreibung
FROM vVermietungen vV
    JOIN vInspektionen vI ON vI.Modell = vV.Modell;

-- b
/*
Ja, da die View's die benötigten Informationen enthalten.
*/

-- 7
SELECT v1.Startdatum, v1.Enddatum, STRING_AGG(k.Name, ', ') AS KundenName
FROM Vermietungen v1
    CROSS JOIN Vermietungen v2
    JOIN Kunden k ON k.KundenID = v2.KundenID
WHERE 
    v1.Startdatum BETWEEN v2.Startdatum AND v2.Enddatum OR
    v1.Enddatum BETWEEN v2.Startdatum AND v2.Enddatum
GROUP BY v1.Startdatum, v1.Enddatum;