USE [ltp]
GO

ALTER TABLE dbo.ltp
ADD Wert decimal(10, 2) NULL;
GO

UPDATE ltp
SET Wert = ltp.menge * t.preis
FROM ltp JOIN t ON ltp.tnr = t.tnr;
GO

ALTER TABLE dbo.l
ADD Gesamtmenge decimal(10, 0) NULL;
GO

UPDATE l
SET Gesamtmenge = ISNULL((SELECT SUM(menge) FROM ltp WHERE ltp.lnr = l.lnr), 0);
GO

-- Uebg 1
-- Trigger 1
go
create or alter trigger tr_ltp_ins_upd_wert
on dbo.ltp
after insert, update
as
begin
    if update(menge) or exists (select 1 from inserted)
    begin
        update ltp
        set Wert = i.menge * t.preis
        from ltp
            join inserted i on ltp.lnr = i.lnr and ltp.tnr = i.tnr and ltp.pnr = i.pnr
            join t on ltp.tnr = t.tnr;
    end
end;
go

-- Trigger 2
go
create or alter trigger tr_t_update_preis
on dbo.t
after update
as
begin
    if update(preis)
    begin
        update ltp
        set Wert = ltp.menge * i.preis
        from ltp
            join inserted i on ltp.tnr = i.tnr;
    end
end;
go

-- Uebg 2
-- Trigger 1
CREATE OR ALTER TRIGGER tr_ltp_insert_gesamtmenge
ON dbo.ltp
AFTER INSERT
AS
BEGIN
    UPDATE l
    SET Gesamtmenge = ISNULL(l.Gesamtmenge, 0) + i.menge
    FROM l
        JOIN inserted i ON l.lnr = i.lnr;
END;
GO

-- Trigger 2
CREATE OR ALTER TRIGGER tr_ltp_update_gesamtmenge
ON dbo.ltp
AFTER UPDATE
AS
BEGIN
    IF UPDATE(menge) OR UPDATE(lnr)
    BEGIN
        UPDATE l
        SET Gesamtmenge = ISNULL(l.Gesamtmenge, 0) - d.menge
        FROM l
            JOIN deleted d ON l.lnr = d.lnr;

        UPDATE l
        SET Gesamtmenge = ISNULL(l.Gesamtmenge, 0) + i.menge
        FROM l
            JOIN inserted i ON l.lnr = i.lnr;
    END
END;
GO

-- Trigger 3
CREATE OR ALTER TRIGGER tr_ltp_delete_gesamtmenge
ON dbo.ltp
AFTER DELETE
AS
BEGIN
    UPDATE l
    SET Gesamtmenge = ISNULL(l.Gesamtmenge, 0) - d.menge
    FROM l
        JOIN deleted d ON l.lnr = d.lnr;
END;
GO

-- Tests
USE [ltp]
GO

-- Delete everything for the tables
delete from ltp
delete from l
delete from t
delete from p
go

IF NOT EXISTS (SELECT 1 FROM dbo.l WHERE lnr = 'L1')
    INSERT INTO dbo.l (lnr, lname, rabatt, stadt) VALUES ('L1', 'Smith', 10, 'Berlin');
GO
    
IF NOT EXISTS (SELECT 1 FROM dbo.l WHERE lnr = 'L2')
    INSERT INTO dbo.l (lnr, lname, rabatt, stadt) VALUES ('L2', 'Jones', 5, 'Paris');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.t WHERE tnr = 'T1')
    INSERT INTO dbo.t (tnr, tname, farbe, preis, stadt) VALUES ('T1', 'Screw', 'Red', 50, 'Berlin');
GO
    
IF NOT EXISTS (SELECT 1 FROM dbo.t WHERE tnr = 'T2')
    INSERT INTO dbo.t (tnr, tname, farbe, preis, stadt) VALUES ('T2', 'Bolt', 'Blue', 75, 'London');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.p WHERE pnr = 'P1')
    INSERT INTO dbo.p (pnr, pname, stadt) VALUES ('P1', 'Bridge', 'Munich');
GO
    
IF NOT EXISTS (SELECT 1 FROM dbo.p WHERE pnr = 'P2')
    INSERT INTO dbo.p (pnr, pname, stadt) VALUES ('P2', 'Tower', 'Hamburg');
GO

DELETE FROM dbo.ltp;
GO

-- Reset Gesamtmenge in L
UPDATE l SET Gesamtmenge = 0 WHERE lnr IN ('L1', 'L2');

PRINT '========== TESTING ÜBUNG 1: Wert Column ==========';

-- Test Case 1: INSERT into LTP
PRINT 'Test Case 1: INSERT into LTP';
INSERT INTO dbo.ltp (lnr, tnr, pnr, menge) VALUES ('L1', 'T1', 'P1', 100);

SELECT 'After INSERT: Expected Wert = 5000 (100 * 50)' AS TestDesc;
SELECT lnr, tnr, pnr, menge, Wert FROM dbo.ltp WHERE lnr = 'L1' AND tnr = 'T1' AND pnr = 'P1';

-- Test Case 2: UPDATE menge in LTP
PRINT 'Test Case 2: UPDATE menge in LTP';
UPDATE dbo.ltp SET menge = 200 WHERE lnr = 'L1' AND tnr = 'T1' AND pnr = 'P1';

SELECT 'After UPDATE menge: Expected Wert = 10000 (200 * 50)' AS TestDesc;
SELECT lnr, tnr, pnr, menge, Wert FROM dbo.ltp WHERE lnr = 'L1' AND tnr = 'T1' AND pnr = 'P1';

-- Test Case 3: UPDATE preis in T
PRINT 'Test Case 3: UPDATE preis in T';
UPDATE dbo.t SET preis = 60 WHERE tnr = 'T1';

SELECT 'After UPDATE preis: Expected Wert = 12000 (200 * 60)' AS TestDesc;
SELECT lnr, tnr, pnr, menge, Wert FROM dbo.ltp WHERE lnr = 'L1' AND tnr = 'T1' AND pnr = 'P1';

PRINT '========== TESTING ÜBUNG 2: Gesamtmenge Column ==========';

-- Test Case 4: Check Gesamtmenge after initial INSERT
PRINT 'Test Case 4: Check Gesamtmenge after initial INSERT';
SELECT 'After initial INSERTs: Expected Gesamtmenge for L1 = 200' AS TestDesc;
SELECT lnr, lname, Gesamtmenge FROM dbo.l WHERE lnr = 'L1';

-- Test Case 5: INSERT another record and check Gesamtmenge
PRINT 'Test Case 5: INSERT another record';
INSERT INTO dbo.ltp (lnr, tnr, pnr, menge) VALUES ('L1', 'T2', 'P2', 150);

SELECT 'After another INSERT: Expected Gesamtmenge for L1 = 350 (200 + 150)' AS TestDesc;
SELECT lnr, lname, Gesamtmenge FROM dbo.l WHERE lnr = 'L1';

-- Test Case 6: UPDATE menge in LTP
PRINT 'Test Case 6: UPDATE menge in LTP';
UPDATE dbo.ltp SET menge = 250 WHERE lnr = 'L1' AND tnr = 'T2' AND pnr = 'P2';

SELECT 'After UPDATE menge: Expected Gesamtmenge for L1 = 450 (200 + 250)' AS TestDesc;
SELECT lnr, lname, Gesamtmenge FROM dbo.l WHERE lnr = 'L1';

-- Test Case 7: UPDATE lnr in LTP (change supplier)
PRINT 'Test Case 7: UPDATE lnr in LTP (change supplier)';
UPDATE dbo.ltp SET lnr = 'L2' WHERE lnr = 'L1' AND tnr = 'T2' AND pnr = 'P2';

SELECT 'After UPDATE lnr: Expected Gesamtmenge for L1 = 200, for L2 = 250' AS TestDesc;
SELECT lnr, lname, Gesamtmenge FROM dbo.l WHERE lnr IN ('L1', 'L2') ORDER BY lnr;

-- Test Case 8: DELETE from LTP
PRINT 'Test Case 8: DELETE from LTP';
DELETE FROM dbo.ltp WHERE lnr = 'L2' AND tnr = 'T2' AND pnr = 'P2';

SELECT 'After DELETE: Expected Gesamtmenge for L2 = 0' AS TestDesc;
SELECT lnr, lname, Gesamtmenge FROM dbo.l WHERE lnr = 'L2';

PRINT 'All tests completed.';