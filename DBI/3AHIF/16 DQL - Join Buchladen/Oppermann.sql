-- Oppermann
-- Reisner

USE Buchladen;

-- 1
INSERT INTO Kunde
    (KundeID, Name, Email, Ort)
VALUES
    (11, 'Bucherwurm', 'buchewurm@example.com', 'Beispielstadt');


INSERT INTO Bestellung
    (BestellungID, KundeID, Bestelldatum)
VALUES
    (11, 11, GETDATE());

INSERT INTO Buch_Bestellung
    (Buch_BestellungID, BestellungID, BuchID, Anzahl)
VALUES
    (20, 11, 1, 1),
    (21, 11, 2, 1),
    (22, 11, 3, 1),
    (23, 11, 4, 1),
    (24, 11, 5, 1),
    (25, 11, 6, 1),
    (26, 11, 7, 1),
    (27, 11, 8, 1),
    (28, 11, 9, 1),
    (29, 11, 10, 1);

-- 2
UPDATE Kunde
SET Name = 'Bücherwurm'
WHERE Name = 'Bucherwurm';

-- 3.1
SELECT k.Name,
    (
        SELECT be.Bestelldatum
        FROM Bestellung be
        WHERE be.KundeID = k.KundeID
    ) AS Datum
FROM Kunde k
WHERE k.KundeID IN (
        SELECT be.KundeID
        FROM Buch_Bestellung bb
            JOIN Buch b ON bb.BuchID = b.BuchID
            JOIN Bestellung be ON be.BestellungID = bb.BestellungID
        WHERE b.Preis > 20
    );

-- 3.2
SELECT k.Ort,
    (
        SELECT STRING_AGG(k1.Name, ', ')
        FROM Kunde k1
        WHERE k1.Ort = k.Ort
    ) AS Names
FROM Kunde k
GROUP BY k.Ort

-- 3.3
SELECT k.Name, be.Bestelldatum
FROM Kunde k
    JOIN Bestellung be ON be.KundeID = k.KundeID
WHERE EXISTS (
        SELECT *
        FROM Buch_Bestellung bb
            JOIN Buch b ON bb.BuchID = b.BuchID
            JOIN Bestellung be ON be.BestellungID = bb.BestellungID
        WHERE b.Autor = 'Franz Kafka' AND be.KundeID = k.KundeID
    );

-- 3.4 
SELECT *
FROM Kunde k
WHERE k.KundeID IN (
        SELECT be.KundeID
        FROM Buch_Bestellung bbe
            JOIN Bestellung be ON be.BestellungID = bbe.BestellungID
        GROUP BY be.KundeID
        HAVING COUNT(*) >= 10
    );

-- 3.5
SELECT *
FROM Buch b
WHERE b.BuchID IN (
        SELECT bbe.BuchID
        FROM Buch_Bestellung bbe
            JOIN Bestellung be ON bbe.BestellungID = be.BestellungID
        WHERE be.Bestelldatum > '2023-10-04'
    );

-- 3.6
SELECT be.*
FROM Buch_Bestellung bbe
    JOIN Bestellung be ON bbe.BestellungID = be.BestellungID
WHERE bbe.BuchID IN (
        SELECT BuchID
        FROM Buch b
        WHERE b.Titel LIKE 'die unendliche Geschichte'
    );

-- 3.7
SELECT *
FROM Bestellung be
WHERE be.BestellungID IN (
    SELECT be.BestellungID
FROM Buch_Bestellung bbe
    JOIN Bestellung be ON bbe.BestellungID = be.BestellungID
    JOIN Buch b ON bbe.BuchID = b.BuchID
    JOIN Kunde k ON be.KundeID = k.KundeID
WHERE k.Ort = 'München' AND b.Titel = 'Der Prozess'
);

SELECT *
FROM (
    SELECT be.*
    FROM Buch_Bestellung bbe
        JOIN Bestellung be ON bbe.BestellungID = be.BestellungID
        JOIN Buch b ON bbe.BuchID = b.BuchID
        JOIN Kunde k ON be.KundeID = k.KundeID
    WHERE k.Ort = 'München' AND b.Titel = 'Der Prozess'
) AS inline_view;

-- 3.8 
/*
Nein, weil ich immer mit einem 
SELECT * beginne. Da ich die Daten sehen will, die raus kommen. Überhaupt wenn ein OUTER JOIN da ist.
*/

-- 4
DELETE FROM Buch_Bestellung
WHERE BestellungID = (
    SELECT BestellungID
FROM Bestellung
WHERE KundeID = (
        SELECT KundeID
FROM Kunde
WHERE Name = 'Bücherwurm'
));

DELETE FROM Bestellung
WHERE KundeID = (SELECT KundeID
FROM Kunde
WHERE Name = 'Bücherwurm');

DELETE FROM Kunde
WHERE Name = 'Bücherwurm';