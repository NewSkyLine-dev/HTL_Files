USE Wichtel;

-- Ermitteln Sie alle Schüler, die anderen Schülern etwas schenken. Ausgegebene Spalten: Secret Santa (Vorname + Nachname), Schüler (Vorname + Nachname), Geschenk
SELECT s1.Vorname + ' ' + s1.Nachname AS [Secret Santa], s2.Vorname + ' ' + s2.Nachname AS [Schüler], g.Name
FROM Wichtel w
    JOIN Schueler s1 on w.VonID = s1.ID
    JOIN Schueler s2 on w.FuerID = s2.ID
    JOIN Geschenk g on w.GeschenkID = g.ID
WHERE s1.ID <> s2.ID;

-- Überprüfen Sie, ob es einen Schüler gibt, der sich selbst etwas schenkt.
SELECT s.Vorname + ' ' + s.Nachname AS [Name]
FROM Wichtel w
    JOIN Schueler s ON w.FuerID = s.ID AND w.VonID = s.ID;

-- Überprüfen Sie, ob es einen Schüler gibt, der niemanden etwas schenkt.
SELECT s.Vorname + ' ' + s.Nachname AS [Name]
FROM Schueler s
    LEFT JOIN Wichtel w ON s.ID = w.VonID
WHERE w.VonID IS NULL OR w.VonID IS NULL;

-- Ermitteln Sie alle Schüler, die mindestens 2 anderen Schülern etwas schenken.
SELECT s.Vorname + ' ' + s.Nachname AS [Secret Santa]
FROM Wichtel w
    JOIN Schueler s ON w.VonID = s.ID
GROUP BY s.Vorname, s.Nachname
HAVING COUNT(w.VonID) >= 2;

-- Ermitteln Sie wie oft ein Geschenk verschenkt wurde.
SELECT g.Name, COUNT(g.ID) AS Anzahl
FROM Wichtel w
    JOIN Geschenk g ON w.GeschenkID = g.ID
GROUP BY g.Name;

-- Ermitteln Sie welche Schüler klassenübergreifend anderen Schülern etwas schenken. Gruppieren Sie dabei nach der Klasse und zählen Sie die Anzahl der Schüler.
SELECT k.Name, COUNT(*) AS Anzahl
FROM Wichtel w
    JOIN Schueler s ON w.VonID = s.ID
    JOIN Klasse k ON s.KlasseID = k.ID
GROUP BY k.Name;

-- Ermitteln Sie die Top 3 unbeliebteren Geschenke.
SELECT TOP 3
    g.Name, COUNT(*) AS Anzahl
FROM Wichtel w
    JOIN Geschenk g ON w.GeschenkID = g.ID
GROUP BY g.Name
ORDER BY Anzahl ASC;