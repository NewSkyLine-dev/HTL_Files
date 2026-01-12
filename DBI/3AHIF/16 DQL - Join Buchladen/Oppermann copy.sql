USE Buchladen;

-- 3.1

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
SELECT k.*
FROM Kunde k
WHERE k.KundeID IN (
    SELECT bes.KundeID
    FROM Bestellung bes
        JOIN Buch_Bestellung bbes ON bes.BestellungID = bbes.BestellungID
        JOIN Buch b ON b.BuchID = bbes.BuchID
    WHERE b.Autor = 'Franz Kafka'
);

-- 3.4

-- 3.5
SELECT *
FROM Buch b
WHERE b.BuchID IN (
    SELECT bbes.BuchID
    FROM Buch_Bestellung bbes
        JOIN Bestellung bes ON bes.BestellungID = bes.BestellungID
    WHERE bes.Bestelldatum > '2023-10-04'
)

-- 3.6
SELECT *
FROM Buch_Bestellung bbes
WHERE bbes.BuchID IN (
    SELECT b.BuchID
    FROM Buch b
    WHERE b.Titel LIKE 'die unendliche Geschichte'
);

-- 3.7
SELECT *
FROM Bestellung bes
WHERE bes.BestellungID IN (
    SELECT bbes.BestellungID
    FROM Buch_Bestellung bbes
        JOIN Buch b ON b.BuchID = bbes.BuchID
        JOIN Bestellung bes ON bes.BestellungID = bbes.BestellungID
        JOIN Kunde k ON k.KundeID = bes.KundeID
    WHERE k.Ort = 'München' AND b.Titel = 'Der Prozess'
);

WITH PROZESS_BESTELLUNGEN AS (
    SELECT *
    FROM Buch
    WHERE Titel = 'Der Prozess'
)
SELECT bes.*
FROM Buch_Bestellung
    JOIN Bestellung bes ON Buch_Bestellung.BestellungID = bes.BestellungID
WHERE BuchID IN (
    SELECT BuchID
    FROM PROZESS_BESTELLUNGEN
)
