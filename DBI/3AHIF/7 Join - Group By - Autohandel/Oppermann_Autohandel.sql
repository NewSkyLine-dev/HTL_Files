USE Autohandel;

-- Ermitteln Sie die Anzahl an verkauften Fahrzeugen pro Hersteller absteigend nach der Anzahl der Verkauften Fahrzeuge
SELECT h.Herstellername, COUNT(*) AS [Anzahl]
FROM Verkauf v
    JOIN Fahrzeug f ON f.FahrzeugID = v.FahrzeugID
    JOIN Modell m ON m.ModellID = f.ModellID
    JOIN Hersteller h ON h.HerstellerID = m.HerstellerID
GROUP BY h.Herstellername
ORDER BY Anzahl DESC;

-- Ermitteln Sie den Gesamtwert aller Fahrzeuge und die Summe der Verkaufserlöse
SELECT m.Modellname, SUM(f.Preis), SUM(v.Verkaufspreis - f.Preis)
FROM Verkauf v
    JOIN Fahrzeug f ON v.FahrzeugID = f.FahrzeugID
    JOIN Modell m ON m.ModellID = f.ModellID
GROUP BY m.Modellname;

-- Ermitteln Sie den durchschnittlichen Verkaufspreis pro Herkunftsland aufsteigend nach dem Verkaufspreis ab einem Baujahr größer gleich 2020.
SELECT h.Land, AVG(v.Verkaufspreis)
FROM Verkauf v
    JOIN Fahrzeug f ON v.FahrzeugID = f.FahrzeugID
    JOIN Modell m ON m.ModellID = f.ModellID
    JOIN Hersteller h ON h.HerstellerID = m.HerstellerID
WHERE f.Baujahr >= 2020
GROUP BY h.Land;

-- Ermitteln Sie die Fahrzeugtypkategorie mit dem höchsten Durchschnittspreis
SELECT TOP 1
    m.Modellname, AVG(v.Verkaufspreis) as [Durchschnittspreis]
FROM Verkauf v
    JOIN Fahrzeug f ON f.FahrzeugID = v.FahrzeugID
    JOIN Modell m ON f.ModellID = m.ModellID
GROUP BY m.Modellname
ORDER BY Durchschnittspreis DESC;

-- Ermitteln Sie pro Autofarbe die Anzahl an Verkäufe und den Gewinn
SELECT f.Farbe, COUNT(*), SUM(v.Verkaufspreis - f.Preis)
FROM Verkauf v
    JOIN Fahrzeug f ON v.FahrzeugID = f.FahrzeugID
GROUP BY f.Farbe;

-- Ermitteln Sie die Anzahl an gekauften Autos pro Kunde und die Summe, die er dafür ausgegeben hat
SELECT k.Name, COUNT(*), SUM(v.Verkaufspreis)
FROM Verkauf v
    JOIN Kunde k ON v.KundenID = k.KundenID
GROUP BY k.Name;

-- Ermitteln Sie die Gesamtanzahl der verkauften Fahrzeuge pro Herkunftsland mit mehr als 5 Verkäufen und das Herstellerland mehr als 5 Zeichen aufweist
SELECT h.Land, COUNT(*)
FROM Verkauf v
    JOIN Fahrzeug f ON v.FahrzeugID = f.FahrzeugID
    JOIN Modell m ON f.ModellID = m.ModellID
    JOIN Hersteller h ON m.HerstellerID = h.HerstellerID
GROUP BY h.Land
HAVING COUNT(*) > 5 AND len(h.Land) > 5;