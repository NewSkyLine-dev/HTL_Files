USE Schulungsfirma;

-- 21. Welche Personen besuchen Kursveranstaltungen, die in ihrem Wohnort abgehalten werden und länger als zwei Tage dauern?
SELECT p.*
FROM besucht b
    JOIN person p ON b.pnr = p.pnr
    JOIN kveranst kv ON b.knrlfnd = kv.knrlfnd AND b.knr = kv.knr
WHERE p.ort = kv.ort AND DATEDIFF(DAY, kv.von, kv.bis) > 1;

SELECT p.*
FROM besucht b
    JOIN person p ON b.pnr = p.pnr
WHERE p.ort IN (
        SELECT kv.ort
        FROM kveranst kv
        WHERE DATEDIFF(DAY, kv.von, kv.bis) > 1
            AND kv.knr = b.knr
            AND kv.knrlfnd = b.knrlfnd
    );

-- 22. Welche Kursveranstaltungen werden von Referenten gehalten, die für den Kurs auch geeignet sind?
SELECT *
FROM geeignet g
    JOIN referent r ON r.pnr = g.pnr
    JOIN kveranst kv ON kv.knr = g.knr AND g.pnr = kv.pnr;

SELECT *
FROM geeignet g
    JOIN referent r ON r.pnr = g.pnr
WHERE g.pnr IN (
        SELECT pnr
        FROM kveranst
    )
    AND g.knr IN (
        SELECT knr
        FROM kveranst
    );

-- 23. Alle Referenten, die Kursveranstaltungen gehalten haben bevor / nachdem sie in die Firma eingetreten sind.
SELECT *, IIF(kv.von > re.seit, 'Nach', 'Vor')
FROM kveranst kv
    JOIN referent re ON re.pnr = kv.pnr;

-- 24. Alle Personen (PNr, FName), die einen Kurs in 'Wien' besucht oder gehalten haben.
SELECT *
FROM kveranst kv
    LEFT JOIN referent r ON kv.pnr = r.pnr
    LEFT JOIN besucht b ON b.knr = kv.knr AND b.knrlfnd = kv.knrlfnd
    JOIN person p ON b.pnr = p.pnr
WHERE kv.ort = 'Wien';

-- 25. Dauer der Kursveranstaltungen im Vergleich mit der im Kurs angegebenen Dauer (geht die Veranstaltung über ein Wochenende / Sa,So?)
SELECT *
FROM kveranst kv
    JOIN kurs k ON k.knr = kv.knr
WHERE k.tage <> DATEDIFF(DAY, kv.von, kv.bis) AND DATEPART(dw, kv.bis) >= 6;

-- 26. Welche Referenten, haben Kursveranstaltungen in einem Alter von über 60 Jahren gehalten?
SELECT DATEDIFF(YEAR, r.gebdat, kv.von)
FROM kveranst kv
    JOIN referent r ON kv.pnr = r.pnr
WHERE DATEDIFF(YEAR, r.gebdat, kv.von) > 60;

-- 27. Welche Kursveranstaltungen gibt es, zu denen eine vorausgesetzte Kursveranstaltung zeitlich davor und am selben Ort abgehalten wird?
SELECT *
FROM setztvor sv
    JOIN kveranst kv ON sv.knr = kv.knr
    JOIN kveranst kvv ON sv.knrvor = kvv.knr
WHERE kv.von > kvv.von AND kvv.ort = kv.ort;

-- 28. Welche Kursveranstaltungen überschneiden einander terminlich?
SELECT k1.bezeichn, kv1.knrlfnd, kv1.von, kv1.bis,
       k2.bezeichn, kv2.knrlfnd, kv2.von, kv2.bis
FROM kveranst kv1
    CROSS JOIN kveranst kv2
    JOIN kurs k1 ON k1.knr = kv1.knr
    JOIN kurs k2 ON k2.knr = kv2.knr
WHERE kv1.knrlfnd <> kv2.knrlfnd AND
      (kv1.von BETWEEN kv2.von AND kv2.bis OR
       kv1.bis BETWEEN kv2.von AND kv2.bis OR
       kv2.von BETWEEN kv1.von AND kv1.bis OR
       kv2.bis BETWEEN kv1.von AND kv1.bis);

-- 29. Gibt es Personen, bei denen Kursbesuche einander terminlich überschneiden?


-- 30. Gibt es Referenten, bei denen Kursveranstaltungen, die sie halten, einander terminlich überschneiden?
