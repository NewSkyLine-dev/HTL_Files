CREATE DATABASE ShopManagement;
USE ShopManagement;

-- Erstellung der Tabellen
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50),
    UnitPrice DECIMAL(10, 2),
    Stock INT
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATE
);

CREATE TABLE OrderDetails (
    DetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    ProductID INT,
    Quantity INT,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE Warehouses (
    WarehouseID INT PRIMARY KEY IDENTITY(1,1),
    Location NVARCHAR(100),
    Capacity INT
);

CREATE TABLE StockMovements (
    MovementID INT PRIMARY KEY IDENTITY(1,1),
    WarehouseID INT,
    ProductID INT,
    Quantity INT,
    MovementDate DATE,
    MovementType NVARCHAR(10),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(WarehouseID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Beispielhafte Daten einfügen
INSERT INTO Products (Name, UnitPrice, Stock) VALUES
('Apple', 0.50, 100),
('Banana', 0.30, 150),
('Orange', 0.80, 200);

INSERT INTO Warehouses (Location, Capacity) VALUES
('Warehouse A', 500),
('Warehouse B', 300);

INSERT INTO Orders (OrderDate) VALUES
('2025-01-01'),
('2025-01-02');

INSERT INTO OrderDetails (OrderID, ProductID, Quantity) VALUES
(1, 1, 20),
(1, 2, 15),
(2, 3, 30);