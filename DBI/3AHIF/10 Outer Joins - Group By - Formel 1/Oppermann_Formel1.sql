USE Formel1;

-- In welchen Heimatländern der Piloten finden keine Rennen statt?
SELECT *
FROM Fahrer f
    LEFT JOIN Rennen r ON r.Land = f.Land
WHERE r.Land IS NULL;

-- Welche Piloten haben, wie oft keinen Top 3 Platz belegt?
SELECT f.VName, f.NName, COUNT(r.Platz) AS 'Anzahl'
FROM Fahrer f
    LEFT JOIN Resultate r ON f.FahrerNr = r.FahrerID AND r.Platz > 3
GROUP BY f.VName, f.NName;

-- Welche Piloten haben keine schnellste Runde gefahren?
SELECT *
FROM Fahrer f
    LEFT JOIN Rennen r ON f.FahrerNr = r.SchnellsteRunde
WHERE r.SchnellsteRunde IS NULL;

-- Ermitteln Sie welche Fahrer wie oft kein Resultat erzielt haben?
SELECT f.FahrerNr, COUNT(*)
FROM Fahrer F
    LEFT JOIN Resultate R ON F.FahrerNr = R.FahrerID
WHERE R.FahrerID IS NULL
GROUP BY f.FahrerNr;

-- Welche Fahrer hatte 2022 ein Resultat und ist 2023 in keinem Team?

-- In welchen Ländern wurde 2023 kein Rennen mehr gefahren?