USE Schulungsfirma;

-- Gibt es Referenten, die für keinen Kurs geegnet sind


-- 32
SELECT k.*
FROM kurs k
    JOIN kurs k2 ON k2.bezeichn = 'Dirigieren'
WHERE k2.preis > k.preis;

-- 33
SELECT ku.bezeichn
FROM kurs ku
WHERE ku.preis > ALL (
        SELECT preis
        FROM kurs k
            JOIN besucht be ON be.knr = k.knr
            JOIN kveranst kv ON k.knr = kv.knr AND kv.knrlfnd = be.knrlfnd
        WHERE kv.ort = 'Paris'
);

-- 34
SELECT k.*
FROM setztvor sv
    RIGHT JOIN kurs k ON sv.knr = k.knr
where SV.knrvor IS NULL;

SELECT k.*
FROM kurs k
WHERE k.knr NOT IN (
    SELECT sv.knr
    FROM setztvor sv
    WHERE k.knr = sv.knr
);

-- 35
SELECT p.vname + ' ' + p.fname AS [Name]
FROM person p
WHERE pnr NOT IN (
        SELECT pnr
        FROM kveranst kv
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
)

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


-- 40
