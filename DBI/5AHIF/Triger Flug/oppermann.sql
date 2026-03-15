-- Aufgabe 1
CREATE OR REPLACE TRIGGER trg_pilot_no_pk
BEFORE INSERT OR UPDATE OF PNr OR DELETE
ON PILOT
FOR EACH ROW
BEGIN
    RAISE_APPLICATION_ERROR(-20001, 'NO DML ON PK.');
END;
/

CREATE OR REPLACE TRIGGER trg_typ_no_pk
BEFORE INSERT OR UPDATE OF TNr OR DELETE
ON TYP
FOR EACH ROW
BEGIN
    RAISE_APPLICATION_ERROR(-20001, 'NO DML ON PK.');
END;
/

CREATE OR REPLACE TRIGGER trg_flugzeug_no_pk
BEFORE INSERT OR UPDATE OF TNr OR DELETE
ON FLUGZEUG
FOR EACH ROW
BEGIN
    RAISE_APPLICATION_ERROR(-20001, 'NO DML ON PK.');
END;
/

CREATE OR REPLACE TRIGGER trg_flug_no_pk
BEFORE INSERT OR UPDATE OF FNr, Abflug OR DELETE
ON Flug
FOR EACH ROW
BEGIN
    RAISE_APPLICATION_ERROR(-20001, 'NO DML ON PK.');
END;
/

-- Aufgabe 2
CREATE OR REPLACE TRIGGER trg_geschult_max3
FOR INSERT ON geschult
COMPOUND TRIGGER
  TYPE inserts_per_pilot_t IS TABLE OF SIMPLE_INTEGER INDEX BY Pilot.PNr%TYPE;
  g_inserts_per_pilot inserts_per_pilot_t;

  v_existing_count NUMBER;

  BEFORE STATEMENT IS
  BEGIN
    g_inserts_per_pilot.DELETE;
  END BEFORE STATEMENT;

  BEFORE EACH ROW IS
  BEGIN
    g_inserts_per_pilot(:NEW.Pilot_PNr) :=
      NVL(g_inserts_per_pilot(:NEW.Pilot_PNr), 0) + 1;

    SELECT COUNT(*) INTO v_existing_count
    FROM geschult
    WHERE Pilot_PNr = :NEW.Pilot_PNr;

    IF v_existing_count + g_inserts_per_pilot(:NEW.Pilot_PNr) > 3 THEN
      RAISE_APPLICATION_ERROR(-20001, 'Max 3 Typen pro Pilot.');
    END IF;
  END BEFORE EACH ROW;

END trg_geschult_max3;
/

-- Aufgabe 3
CREATE OR REPLACE TRIGGER trg_pilot_mod_audit
BEFORE UPDATE OF PName, PVorname, GebDat ON Pilot
FOR EACH ROW
BEGIN
  :NEW.ModDat  := SYSDATE;
  :NEW.ModUser := USER;
END;
/

-- Aufgabe 4
CREATE OR REPLACE TRIGGER trg_flug_pilot_qualifikation
BEFORE INSERT OR UPDATE OF PNr, FZNr ON Flug
FOR EACH ROW
DECLARE
  v_typ_tnr NUMBER;
BEGIN
  SELECT TNr INTO v_typ_tnr
  FROM Flugzeug
  WHERE FZNr = :NEW.FZNr;

  IF NOT EXISTS (
    SELECT 1 FROM geschult
    WHERE Pilot_PNr = :NEW.PNr
      AND Typ_TNr = v_typ_tnr
  ) THEN
    RAISE_APPLICATION_ERROR(-20002, 'Nicht geschult für Typ');
  END IF;
END;
/

-- Aufgabe 5
CREATE OR REPLACE TRIGGER trg_flugzeug_stunden
FOR INSERT OR UPDATE OR DELETE ON Flug
COMPOUND TRIGGER
  TYPE t_fz_hours IS TABLE OF NUMBER INDEX BY Flugzeug.FZNr%TYPE;
  g_delta_stunden t_fz_hours;

  BEFORE STATEMENT IS
  BEGIN
    g_delta_stunden.DELETE;
  END BEFORE STATEMENT;

  AFTER EACH ROW IS
    v_delta NUMBER;
  BEGIN
    IF INSERTING OR UPDATING THEN
      IF :NEW.Abflug IS NOT NULL AND :NEW.Ankunft IS NOT NULL THEN
        v_delta := EXTRACT(DAY FROM (:NEW.Ankunft - :NEW.Abflug)) * 24 +
                   EXTRACT(HOUR FROM (:NEW.Ankunft - :NEW.Abflug)) +
                   EXTRACT(MINUTE FROM (:NEW.Ankunft - :NEW.Abflug)) / 60 +
                   EXTRACT(SECOND FROM (:NEW.Ankunft - :NEW.Abflug)) / 3600;
        g_delta_stunden(:NEW.FZNr) := NVL(g_delta_stunden(:NEW.FZNr), 0) + v_delta;
      END IF;
    END IF;

    IF DELETING OR (UPDATING AND
                    (:OLD.Abflug IS NOT NULL AND :OLD.Ankunft IS NOT NULL)) THEN
      IF :OLD.Abflug IS NOT NULL AND :OLD.Ankunft IS NOT NULL THEN
        v_delta := EXTRACT(DAY FROM (:OLD.Ankunft - :OLD.Abflug)) * 24 +
                   EXTRACT(HOUR FROM (:OLD.Ankunft - :OLD.Abflug)) +
                   EXTRACT(MINUTE FROM (:OLD.Ankunft - :OLD.Abflug)) / 60 +
                   EXTRACT(SECOND FROM (:OLD.Ankunft - :OLD.Abflug)) / 3600;
        g_delta_stunden(:OLD.FZNr) := NVL(g_delta_stunden(:OLD.FZNr), 0) - v_delta;
      END IF;
    END IF;
  END AFTER EACH ROW;

  AFTER STATEMENT IS
  BEGIN
    FOR fz IN g_delta_stunden.FIRST .. g_delta_stunden.LAST LOOP
      UPDATE Flugzeug
      SET Flugstunden = Flugstunden + g_delta_stunden(fz)
      WHERE FZNr = fz;
    END LOOP;
  END AFTER STATEMENT;
END trg_flugzeug_stunden;
/
