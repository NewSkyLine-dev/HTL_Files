USE Formel1;

-- 1. Ermitteln Sie alle Rennen, bei denen der Sieger für Ferrari fuhr
SELECT r.*
FROM Resultate res
    JOIN Rennen r ON r.RennenNr = res.RennenNr
    JOIN Team t ON t.FahrerNr1 = res.FahrerID OR t.FahrerNr2 = res.FahrerID
WHERE res.Platz = 1 AND t.Motorenhersteller = 'Ferrari';

SELECT r.*
FROM Rennen r
WHERE r.RennenNr IN (
        SELECT res.RennenNr
        FROM Resultate res
            JOIN Team t ON t.FahrerNr1 = res.FahrerID
            OR t.FahrerNr2 = res.FahrerID
        WHERE res.Platz = 1
            AND t.Motorenhersteller = 'Ferrari'
    );

-- 2. Ermitteln Sie die Fahrer, die nie ein Rennen gewonnen haben
SELECT *
FROM Fahrer f
    LEFT JOIN Resultate res ON f.FahrerNr = res.FahrerID AND res.Platz = 1
WHERE res.Platz IS NULL;

SELECT f.FahrerNr
FROM Fahrer f
WHERE NOT EXISTS (
        SELECT res.FahrerID
        FROM Resultate res
        WHERE f.FahrerNr = res.FahrerID
            AND res.Platz = 1
    );

-- 3. Ermitteln Sie alle Fahrer, die für ein Team mit Mercedes-Motoren fahren
SELECT F.VName + ' ' + F.NName
FROM Team t
    JOIN Fahrer f ON f.FahrerNr = t.FahrerNr1 OR f.FahrerNr = t.FahrerNr2
WHERE t.Motorenhersteller = 'Mercedes';

SELECT F.VName + ' ' + F.NName
FROM Fahrer f
WHERE f.FahrerNr IN (
        SELECT t.FahrerNr1
        FROM Team t
        WHERE t.Motorenhersteller = 'Mercedes'
    )
    OR f.FahrerNr IN (
        SELECT t.FahrerNr2
        FROM Team t
        WHERE t.Motorenhersteller = 'Mercedes'
    );

-- 4. Ermittle das Team mit den meisten Rennsiegen
SELECT TOP 1
    COUNT(res.Platz) AS Anzahl
FROM Resultate res
    JOIN Team t ON res.FahrerID = t.FahrerNr1 OR res.FahrerID = t.FahrerNr2
WHERE res.Platz = 1
GROUP BY t.TeamName
ORDER BY Anzahl DESC;


SELECT TOP 1 t.TeamName,
    (
        SELECT COUNT(*)
        FROM Resultate res
        WHERE res.Platz = 1
            AND (
                t.FahrerNr1 = res.FahrerID
                OR res.FahrerID = t.FahrerNr2
            )
    ) AS Anzahl
FROM Team t
ORDER BY Anzahl DESC;