USE Formel1;

-- 1.a
SELECT *
FROM Rennen r
    LEFT JOIN Resultate res ON res.RennenNr = r.RennenNr
WHERE res.RID IS NULL;

    SELECT r.RennenNr, r.Ort
    FROM Rennen r
EXCEPT
    SELECT res.RennenNr, r.Ort
    FROM Resultate res
        JOIN Rennen r ON r.RennenNr = res.RennenNr;

-- 1.b
SELECT *
FROM Rennen r
    LEFT JOIN Resultate res ON res.RennenNr = r.RennenNr AND r.Jahr = 2023
WHERE res.RID IS NULL;

    SELECT *
    FROM Rennen r
EXCEPT
    SELECT r.*
    FROM Resultate res
        JOIN Rennen r ON res.RennenNr = r.RennenNr
    WHERE r.Jahr = 2023;


-- 1.c
SELECT res.RennenNr
FROM Resultate res
    JOIN Fahrer f ON res.FahrerID = f.FahrerNr
WHERE f.VName IN ('Max', 'Lando') AND f.NName IN ('Verstappen', 'Norris')
GROUP BY res.RennenNr
HAVING COUNT(RennenNr) = 2;

    SELECT res.RennenNr
    FROM Resultate res
        JOIN Fahrer f ON res.FahrerID = f.FahrerNr
    WHERE f.VName IN ('Max', 'Lando') AND f.NName IN ('Verstappen', 'Norris')
    GROUP BY res.RennenNr
    HAVING COUNT(RennenNr) = 2
INTERSECT
    SELECT res.RennenNr
    FROM Resultate res
        JOIN Fahrer f ON res.FahrerID = f.FahrerNr
    WHERE f.VName NOT IN ('Max', 'Lando') OR f.NName NOT IN ('Verstappen', 'Norris');