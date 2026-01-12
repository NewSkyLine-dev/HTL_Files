create database trigger_uebg;
go
use trigger_uebg
go

DROP TRIGGER IF EXISTS TR_ValidateSSNFormat;
DROP TRIGGER IF EXISTS TR_ValidateMaritalStatus;
DROP TRIGGER IF EXISTS TR_VlidateMaterialStatusTransition;
DROP TRIGGER IF EXISTS TR_ValidateSpouseRequirement;
DROP TRIGGER IF EXISTS TR_ValidateMarriage;

-- Übung 1
-- 1
DROP TABLE IF EXISTS ResidentRegistration;
GO
CREATE TABLE ResidentRegistration (
    SocialSecurityNumber CHAR(10) PRIMARY KEY,
    FirstName VARCHAR(32) NOT NULL,
    Surname VARCHAR(32) NOT NULL,
    YearOfBirth INT NOT NULL,
    MonthOfBirth INT NOT NULL,
    DayOfBirth INT NOT NULL,
    MaritalStatus VARCHAR(16) NOT NULL 
        CHECK (MaritalStatus IN ('ledig', 'verheiratet', 'geschieden', 'verwitwet')),
    Spouse CHAR(10) NULL,
    FOREIGN KEY (Spouse) REFERENCES ResidentRegistration(SocialSecurityNumber)
);
GO

-- 2
-- Trigger 1
GO
CREATE OR ALTER TRIGGER TR_ValidateSSNFormat
ON ResidentRegistration
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted
        WHERE RIGHT(SocialSecurityNumber, 6) <> 
              RIGHT('0' + CAST(DayOfBirth AS VARCHAR(2)), 2) + 
              RIGHT('0' + CAST(MonthOfBirth AS VARCHAR(2)), 2) + 
              RIGHT('0' + CAST(YearOfBirth % 100 AS VARCHAR(2)), 2)
    )
    BEGIN
        ROLLBACK;
        THROW 50001, 'SSN must end with birth date in format DDMMYY.', 1;
    END
END;
GO

-- Trigger 2
GO
CREATE OR ALTER TRIGGER TR_ValidateMaritalStatus
ON ResidentRegistration
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted 
        WHERE MaritalStatus NOT IN ('ledig', 'verheiratet', 'geschieden', 'verwitwet')
    )
    BEGIN
        ROLLBACK;
        THROW 50002, 'MaritalStatus must be one of: ledig, verheiratet, geschieden, verwitwet.', 1;
    END
END;
GO

-- Trigger 3
GO
CREATE OR ALTER TRIGGER TR_VlidateMaterialStatusTransition
ON ResidentRegistration
AFTER UPDATE
AS
BEGIN
    IF UPDATE(MaritalStatus)
    BEGIN
        IF EXISTS(
            SELECT 1 FROM inserted i
                JOIN deleted d ON i.SocialSecurityNumber = d.SocialSecurityNumber
            WHERE
                (d.MaritalStatus = 'ledig' AND i.MaritalStatus NOT IN ('ledig', 'verheiratet')) OR
                (d.MaritalStatus = 'verheiratet' AND i.MaritalStatus NOT IN ('verheiratet', 'geschieden', 'verwitwet')) OR
                (d.MaritalStatus = 'geschieden' AND i.MaritalStatus NOT IN ('geschieden', 'verheiratet')) OR
                (d.MaritalStatus = 'verwitwet' AND i.MaritalStatus NOT IN ('verwitwet', 'verheiratet'))
        )
        BEGIN
            ROLLBACK;
            THROW 50003, 'Invalid marital status transition.', 1;
        END
    END
END;
GO

-- Trigger 4
GO
CREATE OR ALTER TRIGGER TR_ValidateSpouseRequirement
ON ResidentRegistration
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted
        WHERE (MaritalStatus = 'verheiratet' AND Spouse IS NULL) OR
            (MaritalStatus <> 'verheiratet' AND Spouse IS NOT NULL)
    )
    BEGIN
        ROLLBACK;
        THROW 50004, 'Spouse must be set only for married persons.', 1;
    END
END;
GO

-- Trigger 5
GO
CREATE OR ALTER TRIGGER TR_ValidateMarriage
ON ResidentRegistration
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted i
        JOIN ResidentRegistration r ON i.Spouse = r.SocialSecurityNumber
        WHERE 
            i.MaritalStatus = 'verheiratet' AND
            (r.MaritalStatus <> 'verheiratet' OR r.Spouse <> i.SocialSecurityNumber)
    )
    BEGIN
        ROLLBACK;
        THROW 50005, 'Both spouses must reference each other.', 1;
    END
END;
GO

-- Testen
-- Test Case: 1
INSERT INTO ResidentRegistration (SocialSecurityNumber, FirstName, Surname, YearOfBirth, MonthOfBirth, DayOfBirth, MaritalStatus, Spouse)
VALUES ('1234120595', 'Hans', 'Müller', 1995, 5, 12, 'ledig', NULL);

INSERT INTO ResidentRegistration 
VALUES ('1234123456', 'Maria', 'Schmidt', 1990, 8, 15, 'ledig', NULL);

-- Test Case: 2
-- Setup for transition tests
INSERT INTO ResidentRegistration 
VALUES ('1234010190', 'Klaus', 'Weber', 1990, 1, 1, 'ledig', NULL);
INSERT INTO ResidentRegistration 
VALUES ('1234020290', 'Lisa', 'Bauer', 1990, 2, 2, 'ledig', NULL);

-- Test 2.1: Should succeed - ledig -> verheiratet
BEGIN TRANSACTION;
UPDATE ResidentRegistration 
SET MaritalStatus = 'verheiratet', Spouse = '1234020290'
WHERE SocialSecurityNumber = '1234010190';

UPDATE ResidentRegistration 
SET MaritalStatus = 'verheiratet', Spouse = '1234010190'
WHERE SocialSecurityNumber = '1234020290';
COMMIT;

-- Tes 2.2: Should fail - verheiratet -> ledig (invalid transition)
UPDATE ResidentRegistration 
SET MaritalStatus = 'ledig', Spouse = NULL
WHERE SocialSecurityNumber = '1234010190';

-- Test Case: 3 - not valid spouse
INSERT INTO ResidentRegistration 
VALUES ('1234030390', 'Anna', 'Fischer', 1990, 3, 3, 'verheiratet', NULL);

INSERT INTO ResidentRegistration 
VALUES ('1234040490', 'Thomas', 'Becker', 1990, 4, 4, 'geschieden', '1234010190');

-- Test Case 4: married status
INSERT INTO ResidentRegistration 
VALUES ('1234050590', 'Julia', 'Schmidt', 1990, 5, 5, 'ledig', NULL);

INSERT INTO ResidentRegistration 
VALUES ('1234060690', 'Michael', 'Meyer', 1990, 6, 6, 'ledig', NULL);

-- Test 4.1
UPDATE ResidentRegistration 
SET MaritalStatus = 'verheiratet', Spouse = '1234060690'
WHERE SocialSecurityNumber = '1234050590';

-- Test 4.2
BEGIN TRANSACTION;
UPDATE ResidentRegistration 
SET MaritalStatus = 'verheiratet', Spouse = '1234060690'
WHERE SocialSecurityNumber = '1234050590';


-- Test Case 5: Polygamous relationship
INSERT INTO ResidentRegistration 
VALUES ('1234070790', 'Sophie', 'Koch', 1990, 7, 7, 'ledig', NULL);

UPDATE ResidentRegistration 
SET MaritalStatus = 'verheiratet', Spouse = '1234050590'
WHERE SocialSecurityNumber = '1234070790';