USE Buchladen;

-- Ermitteln Sie alle Bücher die mehr als 20 Euro kosten
SELECT *
FROM Buch b
WHERE b.Preis >= 20;

-- Ermitteln Sie alle Kunden, die im selben Ort wohnen
SELECT k.Name, k2.Name, k.Ort
FROM Kunde k
    JOIN Kunde k2 ON k.KundeID <> k2.KundeID AND k.Ort = k2.Ort
GROUP BY k.Name, k2.Name, k.Ort

-- Ermitteln Sie die Anzahl an Bestellen Büchern für die Bestellung 2
SELECT COUNT(Anzahl)
FROM Buch_Bestellung
WHERE BestellungID = 2;

-- Ermitteln Sie alle Kunden (Kundenname) die Bücher (Buchtitel) von Franz Kafka bestellt (Bestelldatum) haben
SELECT k.Name, b.Titel, be.Bestelldatum
FROM Buch_Bestellung bbe
    JOIN Buch b ON bbe.BuchID = b.BuchID
    JOIN Bestellung be ON be.BestellungID = bbe.BestellungID
    JOIN Kunde k ON be.KundeID = k.KundeID
WHERE b.Autor = 'Franz Kafka';

-- Ermitteln Sie den Gesamtwert (Summe) aller Bestellten Bücher
SELECT SUM(b.Preis * bbe.Anzahl)
FROM Buch_Bestellung bbe
    JOIN Buch b ON bbe.BuchID = b.BuchID;

-- Ermitteln Sie alle Bestellungen Kundenname, Bestelldatum die nach dem 4.10.2023 eingegangen sind
SELECT k.Name, be.Bestelldatum
FROM Kunde k
    JOIN Bestellung be ON k.KundeID = be.KundeID
WHERE be.Bestelldatum > '2023-10-04';

-- Ermitteln Sie alle Bestellungen mit dem Buchtitel „Die unendliche Geschichte“
SELECT be.*
FROM Buch_Bestellung bbe
    JOIN Bestellung be ON be.BestellungID = bbe.BestellungID
    JOIN Buch b ON bbe.BuchID = b.BuchID
WHERE b.Titel = 'Die unendliche Geschichte';

-- Ermitteln Sie alle Kunden mit der E-Mail - Domain gmail.com
SELECT *
FROM Kunde k
WHERE k.Email LIKE '%@gmail.com';

-- Ermitteln Sie alle Bestellungen, die nur Bücher im Preisbereich zwischen 10 und 15 Euro
SELECT * 
FROM Buch_Bestellung bbe
    JOIN Bestellung be ON bbe.BestellungID = be.BestellungID
    JOIN Buch b ON bbe.BuchID = b.BuchID
WHERE b.Preis BETWEEN 10 AND 15;

-- Ermitteln Sie alle Bestellungen, der Kunden aus „München“ die das Buch „Der Prozess“ bestellt haben
SELECT be.*
FROM Buch_Bestellung bbe
    JOIN Bestellung be ON bbe.BestellungID = be.BestellungID
    JOIN Buch b ON b.BuchID = bbe.BuchID
    JOIN Kunde k ON k.KundeID = be.KundeID AND k.Ort = 'München'
WHERE b.Titel = 'Der Prozess';
