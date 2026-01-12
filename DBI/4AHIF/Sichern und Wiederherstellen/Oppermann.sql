use master;
go


-- Aufgabe A3: Erstellen eines Sicherungsmediums
EXEC sp_addumpdevice @devtype = 'disk',
                    @logicalname = 'BakCatalog',
                    @physicalname = 'C:\Users\fabia\Documents\Catalog Backup\catalog.bak';
GO

SELECT * FROM sys.backup_devices;


-- Aufgabe A4: Vollst�ndige Sicherung durchf�hren
BACKUP DATABASE Catalog TO BakCatalog WITH NAME = 'Full_MO';
GO

BACKUP LOG Catalog TO DISK = 'C:\Users\fabia\Documents\Catalog Backup\catalog_log.bak' WITH NORECOVERY;
GO

-- Liste der Sicherungss�tze anzeigen
RESTORE HEADERONLY FROM BakCatalog;
GO

-- Aufgabe A5: Differenzielle Sicherung nach Dateneinf�gung
USE Catalog;
GO
INSERT INTO Parts VALUES ('T7', 'Feder', 'lila', 2.00, 'Paris');
GO
BACKUP DATABASE Catalog TO DISK = 'C:\Users\fabia\Documents\Catalog Backup\catalog_diff.bak' WITH DIFFERENTIAL;
GO
USE master;
GO

-- Vergleichen der Dateigr��e mit der vorherigen Sicherung
RESTORE HEADERONLY FROM DISK = 'C:\Users\fabia\Documents\Catalog Backup\catalog_diff.bak';
GO

-- Aufgabe A6: Wiederherstellung aus vollst�ndiger und differenzieller Sicherung
USE Catalog;
GO
drop table SupplierParts;
USE master;
GO

-- Wiederherstellung der vollst�ndigen Sicherung
RESTORE DATABASE Catalog FROM BakCatalog WITH REPLACE, NORECOVERY;
GO

-- Wiederherstellung der differenziellen Sicherung
RESTORE DATABASE Catalog FROM DISK = 'C:\Users\fabia\Documents\Catalog Backup\catalog_diff.bak' WITH NORECOVERY;
GO

-- Wiederherstellung des Transaktionsprotokolls
RESTORE LOG Catalog FROM DISK = 'C:\Users\fabia\Documents\Catalog Backup\catalog_log.bak' WITH RECOVERY;
GO

-- �berpr�fen, ob die Daten vorhanden sind
USE Catalog;
GO
SELECT * FROM SupplierParts
USE master;
GO


-- Aufgabe A7: Wiederherstellung �ber SSMS
DROP DATABASE IF EXISTS Catalog;
GO
SELECT name FROM sys.databases;

-- Datenbank wiederherstellen �ber SSMS und �berpr�fen
SELECT * FROM sys.tables;


-- Aufgabe A8: Tabelleninhalt l�schen und wiederherstellen
USE Catalog;
GO
TRUNCATE TABLE SupplierParts;
SELECT * FROM SupplierParts;
USE master;
GO

RESTORE DATABASE Catalog FROM BakCatalog WITH RECOVERY;
GO
SELECT * FROM SupplierParts;


-- Aufgabe A9: Wiederherstellung in ein anderes Verzeichnis
DROP DATABASE IF EXISTS Catalog;
GO
RESTORE DATABASE Catalog FROM BakCatalog 
WITH REPLACE, NORECOVERY,
MOVE 'catalog' TO 'C:\Users\fabia\Documents\Neuer Pfad\catalog.mdf',
MOVE 'catalog_log' TO 'C:\Users\fabia\Documents\Neuer Pfad\catalog.ldf';
GO

USE Catalog;
GO
SELECT * FROM SupplierParts;
USE master;
go


-- Aufgabe A10: Datenaustausch mit Mitsch�ler
CREATE TABLE Matrikelnummer (Name VARCHAR(50));
INSERT INTO Matrikelnummer VALUES ('Max Mustermann');
SELECT * FROM Matrikelnummer;
-- Screenshot erstellen: a10_1.png

BACKUP DATABASE Catalog TO DISK = 'C:\Users\fabia\Documents\Catalog Backup\exchange.bak' WITH INIT;
GO

-- Wiederherstellen der Mitsch�ler-Datenbank
RESTORE FILELISTONLY FROM DISK = 'C:\Users\fabia\Documents\Catalog Backup\exchange.bak';
GO

-- Then restore with MOVE to specify new file locations
RESTORE DATABASE CatalogCopy FROM DISK = 'C:\Users\fabia\Documents\Catalog Backup\exchange.bak'
WITH MOVE 'catalog' TO 'C:\Users\fabia\Documents\Catalog Backup\CatalogCopy.mdf',
     MOVE 'catalog_log' TO 'C:\Users\fabia\Documents\Catalog Backup\CatalogCopy.ldf',
     RECOVERY;
GO

SELECT name FROM sys.databases;
-- Screenshot erstellen: a10_2.png
SELECT * FROM Matrikelnummer;
-- Screenshot erstellen: a10_3.png

-- Aufgabe B1-B6: Transaktionsprotokollsicherung
-- Erstellung der Testdatenbank
drop database if exists testdb;
GO
CREATE DATABASE testdb ON (NAME = 'testdb_dat', FILENAME = 'C:\sqlsrv\testdb.mdf')
LOG ON (NAME = 'testdb_log', FILENAME = 'C:\sqlsrv\testdb.ldf');
GO

SELECT name, physical_name FROM sys.master_files WHERE database_id = DB_ID('testdb');
-- Screenshot erstellen: b01_1.png

ALTER DATABASE testdb SET RECOVERY FULL;
GO
SELECT name, recovery_model_desc FROM sys.databases WHERE name = 'testdb';
-- Screenshot erstellen: b02_1.png

BACKUP DATABASE testdb TO DISK = 'C:\sqlbak\testdb.bak' WITH INIT;
GO
RESTORE HEADERONLY FROM DISK = 'C:\sqlbak\testdb.bak';
-- Screenshot erstellen: b03_1.png

-- F�llen der Datenbank mit Testdaten
USE testdb;
GO

CREATE TABLE dbo.LogTest (
    SomeID INT IDENTITY(1,1) PRIMARY KEY,
    SomeInt INT,
    SomeLetters2 CHAR(2),
    SomeMoney MONEY,
    SomeDate DATETIME,
    SomeHex12 CHAR(12)
);
GO

-- Then fill it with data
INSERT INTO dbo.LogTest (SomeInt, SomeLetters2, SomeMoney, SomeDate, SomeHex12)
SELECT TOP 1000000 
    ABS(CHECKSUM(NEWID())) % 50000 + 1,
    CHAR(ABS(CHECKSUM(NEWID())) % 26 + 65) + CHAR(ABS(CHECKSUM(NEWID())) % 26 + 65),
    CAST(ABS(CHECKSUM(NEWID())) % 10000 / 100.0 AS MONEY),
    CAST(RAND(CHECKSUM(NEWID())) * 3653.0 + 36524.0 AS DATETIME),
    RIGHT(NEWID(), 12)
FROM sys.all_columns ac1 CROSS JOIN sys.all_columns ac2;
GO

SELECT COUNT(*) FROM dbo.LogTest;
-- Screenshot erstellen: b04_1.png

BACKUP LOG testdb TO DISK = 'C:\sqlbak\testdb.trn' WITH INIT;
GO
-- Screenshot erstellen: b05_1.png

-- Aufgabe C1-C8: Vollst�ndiges Backupszenario
BACKUP DATABASE Catalog TO DISK = 'C:\backup\catalog_full.bak' WITH INIT, NAME = 'CATALOG_FULL_WE';
GO

BACKUP DATABASE Catalog TO DISK = 'C:\backup\catalog_diff.bak' WITH DIFFERENTIAL, INIT, NAME = 'CATALOG_MO_DIFF';
GO

BACKUP LOG Catalog TO DISK = 'C:\backup\catalog_mi_log.bak' WITH INIT, NAME = 'CATALOG_MI_LOG';
GO

-- Simulierte Wiederherstellung nach Datenverlust
RESTORE DATABASE Catalog FROM DISK = 'C:\backup\catalog_full.bak' WITH REPLACE, NORECOVERY;
GO
RESTORE DATABASE Catalog FROM DISK = 'C:\backup\catalog_diff.bak' WITH NORECOVERY;
GO
RESTORE LOG Catalog FROM DISK = 'C:\backup\catalog_mi_log.bak' WITH RECOVERY;
GO
-- Screenshot erstellen: c08_1.png
