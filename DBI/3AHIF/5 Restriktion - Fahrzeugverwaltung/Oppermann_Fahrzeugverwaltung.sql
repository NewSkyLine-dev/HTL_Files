USE Fahrzeugverwaltung;

-- Alle Hersteller aus Deutschland oder Japan stammen sortiert nach Herstellername
SELECT *
FROM Hersteller he
WHERE he.Land IN ('Deutschland', 'Japan');

-- Alle Fahrzeuge mit einem Baujahr größer gleich 2020 und einem Kilometerstand zwischen 5000 und 10000.
SELECT *
FROM Fahrzeuge f
WHERE f.Baujahr >= 2020 AND f.Kilometerstand BETWEEN 5000 AND 10000;

-- Alle Kunden deren Vorname „nn“ enthält
SELECT *
FROM Kunden k
WHERE k.Name LIKE '%nn%';

-- Alle Vermietungen deren Mietdauer kleiner als 7 Tage beträgt
SELECT *
FROM Vermietungen v
WHERE DATEDIFF(DAY, v.Startdatum, v.Enddatum) < 7;

-- Ermitteln Sie den durchschnittlichen Preis der Vermietungen
SELECT AVG(v.Preis)
FROM Vermietungen v;

-- Ermitteln Sie die täglichen Kosten der Vermietungen
SELECT v.Preis / (DAY(v.Enddatum) - DAY(v.Startdatum))
FROM Vermietungen v;

-- Alle Inspektionen bei denen etwas „erneuert“ oder „gewechselt“ wurde und unter 500 Euro kosteten sortiert nach dem Preis absteigend
SELECT *
FROM Inspektionen i
WHERE i.Beschreibung LIKE '%wechsel%' OR i.Beschreibung like '%erneuern%'
ORDER BY i.Kosten DESC;