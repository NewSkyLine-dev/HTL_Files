CREATE TABLE log (
    empno NUMBER NOT NULL,
    log_message VARCHAR2(200),
    log_date DATE DEFAULT SYSDATE,
    CONSTRAINT log_pk PRIMARY KEY (empno, log_date)
);

-- Stufe 1
CREATE OR REPLACE TRIGGER trg_emp_sal_before_update
BEFORE UPDATE OF sal ON emp
FOR EACH ROW
WHEN (NEW.sal > 100000)
BEGIN
    INSERT INTO log (empno, log_message)
    VALUES (:OLD.id, 'Gehalt nicht geändert - kann nicht mehr als 100.000 sein!');
    RAISE_APPLICATION_ERROR(-20001, 'Gehalt darf nicht über 100.000 € liegen.');
END;
/

CREATE OR REPLACE TRIGGER trg_emp_sal_after_update
AFTER UPDATE OF sal ON emp
FOR EACH ROW
BEGIN
    INSERT INTO log (empno, log_message)
    VALUES (:OLD.id, 'Gehalt Update Erfolgreich - altes Gehalt: ' || :OLD.sal || ', neues Gehalt: ' || :NEW.sal);
END;
/

-- Stufe 2
CREATE OR REPLACE TRIGGER trg_emp_sal_before_update
BEFORE UPDATE OF sal ON emp
FOR EACH ROW
DECLARE
    v_max_sal NUMBER;
BEGIN
    SELECT MAX(sal) INTO v_max_sal FROM emp;

    IF :NEW.sal > v_max_sal THEN
        INSERT INTO log (empno, log_message)
        VALUES (:OLD.id, 'Gehalt nicht geändert - kann nicht mehr als MAX(' || v_max_sal || ') sein!');
        RAISE_APPLICATION_ERROR(-20001, 'Gehalt darf nicht höher als Tabellen-Max sein.');
    END IF;
END;
-- Mutating trigger Fehler. Die Tabelle auf dem der Trigger sitzt darf nicht selektiert werden
/
UPDATE emp
SET sal = 90000 WHERE id = 2;

-- Stufe 3
CREATE OR REPLACE PACKAGE ceo_sal_pkg AS
  g_max_sal NUMBER;
END ceo_sal_pkg;
/
CREATE OR REPLACE PACKAGE BODY ceo_sal_pkg AS
BEGIN
  NULL;
END ceo_sal_pkg;
/
CREATE OR REPLACE TRIGGER trg_emp_sal_stmt_before
BEFORE UPDATE ON emp
BEGIN
  SELECT MAX(sal) INTO ceo_sal_pkg.g_max_sal FROM emp;
END;
/
CREATE OR REPLACE TRIGGER trg_emp_sal_before_update
BEFORE UPDATE OF sal ON emp
FOR EACH ROW
BEGIN
  IF :NEW.sal > ceo_sal_pkg.g_max_sal THEN
    INSERT INTO log (empno, log_message)
    VALUES (:OLD.id, 'Gehalt nicht geändert - kann nicht mehr als MAX(' || ceo_sal_pkg.g_max_sal || ') sein!');
    RAISE_APPLICATION_ERROR(-20001, 'Gehalt darf nicht höher als Tabellen-Max sein.');
  END IF;
END;
/

-- Stufe 4
CREATE OR REPLACE TRIGGER trg_emp_sal_compound
FOR UPDATE OF sal ON emp
COMPOUND TRIGGER
  g_global_max NUMBER;

  BEFORE STATEMENT IS
  BEGIN
    SELECT MAX(sal)
    INTO g_global_max
    FROM emp;
  END BEFORE STATEMENT;

  BEFORE EACH ROW IS
  BEGIN
    IF g_global_max IS NOT NULL AND :NEW.sal > g_global_max THEN
      INSERT INTO log (empno, log_message)
      VALUES (:OLD.id, 'Gehalt nicht geändert - kann nicht mehr als MAX(' || g_global_max || ') sein!');
      RAISE_APPLICATION_ERROR(-20001, 'Gehalt darf nicht höher als Tabellen-Max(' || g_global_max || ') sein.');
    END IF;
  END BEFORE EACH ROW;

END trg_emp_sal_compound;
/
