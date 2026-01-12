USE Schule;

-- 1
SELECT k.Klassenbezeichnung, COUNT(*) AS [Schueleranzahl], l.Name
FROM Schueler s
    JOIN Klasse k ON s.KlasseID = k.KlasseID
    JOIN Lehrer l ON k.KlassenlehrerID = l.LehrerID
GROUP BY k.Klassenbezeichnung, l.Name;

-- 2
SELECT k.Klassenbezeichnung, f.Fachbezeichnung, s.Name, n.Note
FROM Schueler s
    JOIN Klasse K on S.KlasseID = s.KlasseID
    JOIN Note n ON n.SchuelerID = s.SchuelerID
    JOIN Fach f ON f.FachID = n.FachID
WHERE n.Datum <= '2023-06-30';

-- 3
SELECT f.Fachbezeichnung, STRING_AGG(l.Name, ', ')
FROM Fach f
    JOIN Lehrer l ON f.LehrerID = l.LehrerID
GROUP BY f.Fachbezeichnung;

-- 4
SELECT s.Geburtsjahr, k.Klassenbezeichnung, COUNT(*) AS Anzahl
FROM Schueler s
    JOIN Klasse k ON k.KlasseID = s.KlasseID
GROUP BY s.Geburtsjahr, k.Klassenbezeichnung;

-- 5
SELECT f.Fachbezeichnung, n.Note, COUNT(*)
FROM Fach f
    JOIN Note n ON n.FachID = f.FachID
WHERE n.Note IN (1, 2, 3)
GROUP BY f.Fachbezeichnung, n.Note;