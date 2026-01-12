USE Mitarbeiter;

-- Listen Sie den Vor- und Nachnamen inkl. Geburtsdatum aller Mitarbeiter auf
SELECT Vorname, Nachname, Geburtsdatum
FROM Mitarbeiter;

-- Listen Sie alle Mitarbeiter mit ihrem Alter sowie dem Vor- und Nachnamen auf
SELECT Vorname, Nachname, DATEDIFF(YEAR, Geburtsdatum, GETDATE())
FROM Mitarbeiter;

-- Listen Sie alle Mitarbeiter mit ihrem monatlichen Gehalt auf und stellen Sie den Nachnamen in Großbuchstaben dar
SELECT UPPER(Nachname) AS [Nachname], (Gehalt / 12) AS [Monatsgehalt]
FROM Mitarbeiter;

-- Listen Sie alle Mitarbeiter mit ihrem Vor- und Nachnamen, der Anzahl an Tagen und die Anzahl an Monaten die jeder im Unternehmen ist auf
SELECT
    *,
    DATEDIFF(DAY, Eintrittsdatum, GETDATE()) AS [Tage im Unternehmen],
    DATEDIFF(MONTH, Eintrittsdatum, GETDATE()) AS [Monate im Unternehmen]
FROM Mitarbeiter;

-- Ermitteln Sie das Durchschnittsgehalt aller Mitarbeiter. Verwenden Sie für das Ergebnisattribut einen geeigneten Alias
SELECT AVG(Gehalt) AS [Durchschnittsgehalt]
FROM Mitarbeiter;

-- Ermitteln Sie welche Abteilungen es im Unternehmen gibt
SELECT DISTINCT Abteilung
FROM Mitarbeiter;

-- Listen Sie alle Mitarbeiter inkl. einem um 10% erhöhten Gehalt auf
SELECT *, (Gehalt * 1.1) AS [Gehalt + 10%]
FROM Mitarbeiter;

-- Ermitteln Sie für jeden Mitarbeiter den Namen und das Jahresgehalt nach Steuern an. Nehmen wir an, dass die Steuer 20% des Gehalts beträgt
SELECT *, ((Gehalt * 20) / 120) AS [Jahresgehalt nach Steuern]
FROM Mitarbeiter;

-- Erstelle eine Abfrage, die den vollständigen Namen (Vorname und Nachname) sowie die Position und die Abteilung in einem formatierten String für jeden Mitarbeiter zurückgibt
SELECT Vorname + ' ' + Nachname AS [Name], Position + ' in der Abteilung ' + Abteilung AS [Position]
FROM Mitarbeiter;