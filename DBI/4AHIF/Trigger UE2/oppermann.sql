USE warehouse;
GO

/*
	========== W1 ==========
*/
CREATE TABLE Product (
    ProdNum INT PRIMARY KEY,
    Name NVARCHAR(100),
    Price DECIMAL(10,2),
    PriceSince DATE
);
GO

-- PriceChangeLog
CREATE TABLE PriceChangeLog (
    ProdNum INT,
    PriceSince DATE,
    Price DECIMAL(10,2),
    PRIMARY KEY (ProdNum, PriceSince),
    FOREIGN KEY (ProdNum) REFERENCES Product(ProdNum)
);
GO

-- Warehouse
CREATE TABLE Warehouse (
    WareNum INT PRIMARY KEY,
    Location NVARCHAR(100),
    Capacity INT,
    Amount INT
);
GO

-- Suitability
CREATE TABLE Suitability (
    ProdNum INT,
    WareNum INT,
    PRIMARY KEY (ProdNum, WareNum),
    FOREIGN KEY (ProdNum) REFERENCES Product(ProdNum),
    FOREIGN KEY (WareNum) REFERENCES Warehouse(WareNum)
);
GO

-- Shipment
CREATE TABLE Shipment (
    WareNum INT,
    OrderNum INT,
    ProdNum INT,
    DeliveryDate DATE,
    Amount INT,
    PRIMARY KEY (WareNum, OrderNum),
    FOREIGN KEY (WareNum) REFERENCES Warehouse(WareNum),
    FOREIGN KEY (ProdNum) REFERENCES Product(ProdNum)
);
GO


/*
	========== W2 ==========
*/
GO
CREATE TRIGGER trg_Prevent_PK_Change_Product
ON Product
AFTER UPDATE
AS
BEGIN
    IF UPDATE(ProdNum)
    BEGIN
        RAISERROR('Ändern des Primärschlüssels ist nicht erlaubt.', 16, 1);
        ROLLBACK;
    END
END
GO

GO
CREATE TRIGGER trg_Prevent_PK_Change_Warehouse
ON Warehouse
AFTER UPDATE
AS
BEGIN
    IF UPDATE(WareNum)
    BEGIN
        RAISERROR('Ändern des Primärschlüssels ist nicht erlaubt.', 16, 1);
        ROLLBACK;
    END
END
GO


/*
	========== W3 ==========
*/
GO
CREATE TRIGGER trg_Price_Cannot_Decrease
ON Product
AFTER UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT * FROM inserted i
        JOIN deleted d ON i.ProdNum = d.ProdNum
        WHERE i.Price < d.Price
    )
    BEGIN
        RAISERROR('Preis darf nicht verringert werden.', 16, 1);
        ROLLBACK;
    END
END
GO

GO
CREATE TRIGGER trg_Check_Suitability
ON Shipment
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT * FROM inserted i
        WHERE NOT EXISTS (
            SELECT 1 FROM Suitability s
            WHERE s.ProdNum = i.ProdNum AND s.WareNum = i.WareNum
        )
    )
    BEGIN
        RAISERROR('Das Lager ist für dieses Produkt nicht geeignet.', 16, 1);
        ROLLBACK;
    END
END
GO

GO
CREATE TRIGGER trg_Limit_Lageranzahl
ON Suitability
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT ProdNum FROM Suitability
        GROUP BY ProdNum
        HAVING COUNT(WareNum) > 2
    )
    BEGIN
        RAISERROR('Ein Produkt darf maximal in zwei Lagern vorkommen.', 16, 1);
        ROLLBACK;
    END
END
GO

GO
CREATE TRIGGER trg_Capacity_Not_Exceeded
ON Shipment
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT w.WareNum FROM inserted i
        JOIN Warehouse w ON i.WareNum = w.WareNum
        WHERE i.Amount + w.Amount > w.Capacity
    )
    BEGIN
        RAISERROR('Lagerkapazität überschritten.', 16, 1);
        ROLLBACK;
    END
END
GO


/*
	========== W4 ==========
*/
GO
CREATE TRIGGER trg_Update_Warehouse_Amount
ON Shipment
AFTER INSERT
AS
BEGIN
    UPDATE w
    SET w.Amount = w.Amount + i.Amount
    FROM Warehouse w
    JOIN inserted i ON w.WareNum = i.WareNum
END
GO


/*
	========== W5 ==========
*/
GO
CREATE TRIGGER trg_Update_PriceSince
ON Product
AFTER UPDATE
AS
BEGIN
    UPDATE Product
    SET PriceSince = GETDATE()
    FROM Product p
    JOIN inserted i ON p.ProdNum = i.ProdNum
    JOIN deleted d ON d.ProdNum = p.ProdNum
    WHERE i.Price <> d.Price
END
GO

GO
CREATE TRIGGER trg_Prevent_Manual_PriceSince_Update
ON Product
INSTEAD OF UPDATE
AS
BEGIN
    IF TRIGGER_NESTLEVEL() = 1 AND EXISTS (
        SELECT * FROM inserted i
        JOIN deleted d ON i.ProdNum = d.ProdNum
        WHERE i.PriceSince <> d.PriceSince
    )
    BEGIN
        RAISERROR('PriceSince darf nicht manuell geändert werden.', 16, 1);
        ROLLBACK;
    END
    ELSE
    BEGIN
        UPDATE Product
        SET Name = i.Name,
            Price = i.Price
        FROM Product p
        JOIN inserted i ON p.ProdNum = i.ProdNum
    END
END
GO

GO
CREATE TRIGGER trg_Prevent_PriceChangeLog_Manipulation
ON PriceChangeLog
INSTEAD OF INSERT, UPDATE, DELETE
AS
BEGIN
    RAISERROR('Manipulation der Tabelle PriceChangeLog ist nicht erlaubt.', 16, 1);
    ROLLBACK;
END
GO


/*
	========== Testfälle W2 ==========
*/
-- Setup
INSERT INTO Product (ProdNum, Name, Price, PriceSince)
VALUES (1, 'Testprodukt', 10.00, GETDATE());

-- Versuch, den Primärschlüssel zu ändern
UPDATE Product SET ProdNum = 2 WHERE ProdNum = 1;
-- Erwartet: Fehler "Ändern des Primärschlüssels ist nicht erlaubt."


/*
	========== Testfälle W3 ==========
*/
-- Setup
UPDATE Product SET Price = 12.00 WHERE ProdNum = 1;

UPDATE Product SET Price = 8.00 WHERE ProdNum = 1;
-- Erwartet: Fehler "Preis darf nicht verringert werden."


-- Setup für Lager und Eignung
INSERT INTO Warehouse (WareNum, Location, Capacity, Amount)
VALUES (100, 'Lager A', 500, 0);

INSERT INTO Shipment (WareNum, OrderNum, ProdNum, DeliveryDate, Amount)
VALUES (100, 1, 1, GETDATE(), 10);
-- Erwartet: Fehler "Das Lager ist für dieses Produkt nicht geeignet."


-- Setup für Suitability
INSERT INTO Suitability (ProdNum, WareNum) VALUES (1, 101);
INSERT INTO Warehouse (WareNum, Location, Capacity, Amount) VALUES (101, 'Lager B', 300, 0);
INSERT INTO Suitability (ProdNum, WareNum) VALUES (1, 102);
INSERT INTO Warehouse (WareNum, Location, Capacity, Amount) VALUES (102, 'Lager C', 300, 0);

INSERT INTO Warehouse (WareNum, Location, Capacity, Amount) VALUES (103, 'Lager D', 300, 0);
INSERT INTO Suitability (ProdNum, WareNum) VALUES (1, 103);
-- Erwartet: Fehler "Ein Produkt darf maximal in zwei Lagern vorkommen."


-- Setup
UPDATE Warehouse SET Amount = 490, Capacity = 500 WHERE WareNum = 100;
INSERT INTO Suitability (ProdNum, WareNum) VALUES (1, 100);

INSERT INTO Shipment (WareNum, OrderNum, ProdNum, DeliveryDate, Amount)
VALUES (100, 2, 1, GETDATE(), 20);
-- Erwartet: Fehler "Lagerkapazität überschritten."


/*
	========== Testfälle W4 ==========
*/
-- Setup: Lager hat 100 Stück
UPDATE Warehouse SET Amount = 100 WHERE WareNum = 101;
INSERT INTO Suitability (ProdNum, WareNum) VALUES (1, 101);

INSERT INTO Shipment (WareNum, OrderNum, ProdNum, DeliveryDate, Amount)
VALUES (101, 1, 1, GETDATE(), 30);

SELECT Amount FROM Warehouse WHERE WareNum = 101;
-- Erwartet: 130


/*
	========== Testfälle W5 ==========
*/
-- Setup: aktueller Preis 12.00
UPDATE Product SET Price = 15.00 WHERE ProdNum = 1;

SELECT PriceSince FROM Product WHERE ProdNum = 1;
SELECT * FROM PriceChangeLog WHERE ProdNum = 1 ORDER BY PriceSince DESC;