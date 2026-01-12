USE Schulungsfirma;

-- 32
SELECT *
FROM kurs k1
WHERE k1.preis <= ANY(
        SELECT preis
        FROM kurs k2
        WHERE k2.bezeichn = 'Dirigieren'
            AND k2.knr <> k1.knr
);

-- 33
SELECT ku.bezeichn
FROM kurs ku
WHERE ku.preis > ALL (
        SELECT preis
        FROM kurs k
            JOIN kveranst kv ON k.knr = kv.knr
        WHERE kv.ort = 'Paris'
);

-- 34
SELECT *
FROM setztvor sv
    RIGHT JOIN kurs k ON sv.knr = k.knr
where sv.knrvor IS NULL;

-- 35
SELECT *
FROM person p
WHERE p.pnr NOT IN (
    SELECT be.pnr
    FROM besucht be
        JOIN kveranst kv ON kv.knr = be.knr AND be.knrlfnd = kv.knrlfnd
        WHERE YEAR(kv.von) BETWEEN 2003 AND 2005
);

-- 37
SELECT p.fname, p.fname
FROM person p
    JOIN besucht be ON be.pnr = p.pnr
    JOIN kurs k ON k.knr = be.knr
WHERE k.preis < ANY(
        SELECT preis
        FROM kurs
            JOIN kveranst KV on KV.knr = KURS.knr
        where kv.ort = 'Moskau'
);

-- 38
SELECT kv.*
FROM kveranst kv
    JOIN kurs k ON kv.knr = k.knr
WHERE kv.pnr NOT IN (
    SELECT pnr
    FROM geeignet
    WHERE k.knr = knr
);

-- 39
/*
Welche Personen (PNr und FName) besuchen Kurse in 'Wien' und in 'Paris'?
*/
SELECT p.pnr, p.fname
FROM person p
    JOIN besucht be ON be.pnr = p.pnr
    JOIN kveranst kv ON kv.knr = be.knr AND kv.knrlfnd = be.knrlfnd
WHERE kv.ort = 'Wien'
    AND p.pnr IN (
        SELECT KV.pnr
        FROM kveranst kv
            JOIN besucht be ON kv.knr = be.knr AND kv.knrlfnd = be.knrlfnd
        WHERE kv.ort = 'Paris'
    );