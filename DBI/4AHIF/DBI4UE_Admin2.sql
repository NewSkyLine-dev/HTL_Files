-- Aufgabe 1: Erstellen einer Datenbank "Catalog"
CREATE DATABASE Catalog
ON 
(
    NAME = 'Catalog_Data',      
    FILENAME = 'C:\Users\fabia\SQLData\Catalog_Data.mdf', 
    SIZE = 10MB,                
    MAXSIZE = UNLIMITED,         
    FILEGROWTH = 10%             
)
LOG ON
(
    NAME = 'Catalog_Log',       
    FILENAME = 'C:\Users\fabia\SQLData\Catalog_Log.ldf', 
    SIZE = 2MB,                 
    MAXSIZE = UNLIMITED,        
    FILEGROWTH = 10%            
);
GO

-- Aufgabe 2: Erstellen der Tabellen
CREATE TABLE Suppliers 
(
	SupplierID VARCHAR(2) PRIMARY KEY,
	SupplierName VARCHAR(6),
	SupplierCity VARCHAR(6),
	SupplierDiscount INT
);
GO
CREATE TABLE Parts 
(
	PartID VARCHAR(2) PRIMARY KEY,
	PartName VARCHAR(8),
	PartColor VARCHAR(6),
	PartPrice DECIMAL(6, 2),
	PartCity VARCHAR(6)
);
GO
CREATE TABLE SupplierParts
(
	SupplierID VARCHAR(2),
	PartID VARCHAR(2),
	Amount INT,
	PRIMARY KEY (SupplierID, PartID),
	FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
	FOREIGN KEY (PartID) REFERENCES Parts(PartID)
);
GO

-- Aufgabe 3: Einfügen von Daten
INSERT INTO Suppliers VALUES
	('L1', 'Schmid', 'London', 20),
	('L2', 'Jonas' , 'Paris' , 10),
	('L3', 'Berger', 'Paris' , 30),
	('L4', 'Klein' , 'London', 20),
	('L5', 'Adam' , 'Athen' , 30);
INSERT INTO Parts VALUES
	('T1', 'Mutter' , 'rot' , 12, 'London'),
	('T2', 'Bolzen' , 'gelb', 17, 'Paris' ),
	('T3', 'Schraube', 'blau', 17, 'Rom' ),
	('T4', 'Schraube', 'rot' , 14, 'London'),
	('T5', 'Welle' , 'blau', 12, 'Paris' ),
	('T6', 'Zahnrad' , 'rot' , 19, 'London');
INSERT INTO SupplierParts VALUES
	('L1', 'T1', 300),
	('L1', 'T2', 200),
	('L1', 'T3', 400),
	('L1', 'T4', 200),
	('L1', 'T5', 100),
	('L1', 'T6', 100),
	('L2', 'T1', 300),
	('L2', 'T2', 400),
	('L3', 'T2', 200),
	('L4', 'T2', 200),
	('L4', 'T4', 300),
	('L4', 'T5', 400);

-- Aufgabe 4a: Teile mit einem Preis unter dem Durchschnittspreis
SELECT PartName, PartPrice
FROM Parts
WHERE PartPrice < (SELECT AVG(PartPrice) FROM Parts);
GO
-- 4b: Gesamtumsatz der Lieferanten, inkl. Rabatt
SELECT sp.SupplierID, SUM(sp.Amount * p.PartPrice * (1 - s.SupplierDiscount / 100.0)) AS Total
FROM SupplierParts sp
	JOIN Suppliers s ON sp.SupplierID = s.SupplierID
	JOIN Parts p ON sp.PartID = p.PartID
GROUP BY sp.SupplierID;
GO
-- 4c: Teile, die von L3 geliefert werden
SELECT p.PartName
FROM SupplierParts sp
	JOIN Parts p ON sp.PartID = p.PartID
WHERE sp.SupplierID = 'L3';
GO

-- Aufgabe 5: Umbenennen der Datenbank in "WhoSuppliesWhat"
ALTER DATABASE Catalog 
MODIFY NAME = WhoSuppliesWhat;
GO

-- Aufgabe 6: Hinzufügen eines neuen Datenfiles zur Datenbank
ALTER DATABASE WhoSuppliesWhat
ADD FILE 
(
    NAME = who_dat2,
    FILENAME = 'C:\Users\fabia\SQLData\who_dat2.ndf',
    SIZE = 10MB,
    MAXSIZE = 100MB,
    FILEGROWTH = 5MB
);
GO

-- Aufgabe 7a: Das Batchtrennzeichen GO
-- GO ist kein SQL-Befehl, sondern ein Befehl des SQL Server Management Studios (SSMS).
-- Es signalisiert das Ende eines SQL-Batchs, der an den Server gesendet wird.
-- Es ist wichtig, um SQL-Anweisungen in mehreren Batches auszuführen.
-- Aufgabe 7b: Möglichkeit 1 - Abfrage der Systemkataloge
SELECT name, physical_name
FROM sys.master_files
WHERE database_id = DB_ID('WhoSuppliesWhat');
GO

-- Möglichkeit 2 - Verwenden von sp_helpdb
EXEC sp_helpdb 'WhoSuppliesWhat';
GO

-- Aufgabe 8: Löschen der Datenbank
DROP DATABASE WhoSuppliesWhat;
GO