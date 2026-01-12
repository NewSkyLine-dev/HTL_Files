-- Datum: 23.04.2024
-- Name: Fabian Oppermann
-- Aufgabe: Übung DQL - Schulungsfirma 41-50 - Wiederholung

-- 41
SELECT p.pnr
FROM person p
    JOIN besucht b ON b.pnr = p.pnr
    JOIN kveranst kv ON kv.knr = b.knr AND kv.knrlfnd = b.knrlfnd
GROUP BY p.pnr
HAVING COUNT(p.pnr) >= 2;

-- 42
SELECT kv.knr, kv.knrlfnd
FROM kveranst kv
WHERE NOT EXISTS (
    SELECT *
    FROM besucht b
    WHERE b.knr = kv.knr AND b.knrlfnd = kv.knrlfnd
);

-- 43 ???
SELECT kv.knr, kv.knrlfnd,
    CASE
        WHEN DATEPART(WEEKDAY, kv.von) IN (1, 7) THEN k.Preis * 1.1
        ELSE k.preis
    END AS Preis
FROM kveranst kv
    JOIN kurs k ON k.knr = kv.knr
ORDER BY kv.knr, kv.knrlfnd;

-- 44
SELECT kv.knr, kv.knrlfnd
FROM kveranst kv
WHERE kv.plaetze > (
    SELECT COUNT(*)
    FROM besucht b
    WHERE b.knr = kv.knr AND b.knrlfnd = kv.knrlfnd
);

-- 45 ???
SELECT COUNT(DISTINCT kv.knr)
FROM kveranst kv
WHERE NOT EXISTS (
    SELECT *
    FROM referent r
        JOIN geeignet g ON r.pnr = g.pnr
    WHERE g.knr = kv.knr
)
GROUP BY kv.knr, kv.knrlfnd;

-- 46
SELECT r.pnr
FROM referent r
WHERE NOT EXISTS (
    SELECT *
    FROM kveranst kv
    WHERE kv.pnr = r.pnr
);

-- 47
SELECT p.pnr
FROM person p
WHERE NOT EXISTS (
    SELECT *
    FROM besucht b
    WHERE b.pnr = p.pnr
);

-- 48 ??? 
SELECT k1.bezeichn, k2.bezeichn
FROM kurs k1
    JOIN setztvor v ON k1.knr = v.knr
    JOIN kurs k2 ON v.knrvor = k2.knr
ORDER BY k1.bezeichn, k2.bezeichn;


-- 49
SELECT *
FROM kurs
WHERE knr NOT IN (
    SELECT kv.knr
    FROM kveranst kv
    WHERE kv.ort = 'Wien' AND DATEDIFF(DAY, kv.von, kv.bis) > 1
)

-- 50 ???
SELECT p.pnr
FROM person p
WHERE NOT EXISTS (
    SELECT *
    FROM kurs k
    WHERE NOT EXISTS (
        SELECT *
        FROM besucht b
        WHERE b.pnr = p.pnr AND b.knr = k.knr
    )
);