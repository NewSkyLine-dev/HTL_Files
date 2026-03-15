-- 1
CREATE OR REPLACE TRIGGER trg_commission_sales
BEFORE INSERT OR UPDATE OF job_id, commission_pct ON employees
FOR EACH ROW
BEGIN
  IF :NEW.job_id NOT IN ('SA_MAN', 'SA_REP') AND :NEW.commission_pct IS NOT NULL THEN
    RAISE_APPLICATION_ERROR(-20006, 'Provision nur für SA_MAN/SA_REP erlaubt.');
  END IF;
END;
/

-- 2
CREATE OR REPLACE TRIGGER trg_one_president
FOR INSERT OR UPDATE OF job_id ON employees
COMPOUND TRIGGER
  TYPE t_pres IS TABLE OF employee_id%TYPE;
  g_pres_ids t_pres := t_pres();

  AFTER EACH ROW IS
  BEGIN
    IF :NEW.job_id = 'AD_PRES' THEN
      g_pres_ids.EXTEND;
      g_pres_ids(g_pres_ids.COUNT) := :NEW.employee_id;
    END IF;
  END AFTER EACH ROW;

  AFTER STATEMENT IS
  BEGIN
    IF g_pres_ids.COUNT > 1 THEN
      RAISE_APPLICATION_ERROR(-20001, 'Nur ein Präsident erlaubt.');
    END IF;
  END AFTER STATEMENT;
END;
/

-- 3
CREATE OR REPLACE TRIGGER trg_max_15_employees
FOR INSERT OR UPDATE OF manager_id ON employees
COMPOUND TRIGGER
  TYPE t_mgr_count IS TABLE OF NUMBER INDEX BY NUMBER;
  g_mgr_counts t_mgr_count;

  AFTER EACH ROW IS
  BEGIN
    IF :NEW.manager_id IS NOT NULL THEN
      g_mgr_counts(:NEW.manager_id) := NVL(g_mgr_counts(:NEW.manager_id), 0) + 1;
    END IF;
  END AFTER EACH ROW;

  AFTER STATEMENT IS
    v_count NUMBER;
  BEGIN
    FOR mgr_id IN g_mgr_counts.FIRST .. g_mgr_counts.LAST LOOP
      SELECT COUNT(*)
      INTO v_count
      FROM employees
      WHERE manager_id = mgr_id;

      IF v_count > 15 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Manager ' || mgr_id || ' überschreitet 15.');
      END IF;
    END LOOP;
  END AFTER STATEMENT;
END;
/

-- 4
CREATE OR REPLACE TRIGGER trg_salary_increase_only
BEFORE UPDATE OF salary ON employees
FOR EACH ROW
BEGIN
  IF :NEW.salary < :OLD.salary THEN
    RAISE_APPLICATION_ERROR(-20003, 'Gehalt darf nicht gesenkt werden.');
  END IF;
END;
/

-- 5
CREATE OR REPLACE TRIGGER trg_dept_location_update
AFTER UPDATE OF location_id ON departments
FOR EACH ROW
BEGIN
  UPDATE employees
  SET salary = salary * 1.02
  WHERE department_id = :NEW.department_id;
END;
/

-- 6
CREATE OR REPLACE TRIGGER trg_business_hours
BEFORE INSERT OR UPDATE OR DELETE ON employees
FOR EACH ROW
DECLARE
  v_day VARCHAR2(10);
  v_time VARCHAR2(5);
BEGIN
  v_day := TO_CHAR(SYSDATE, 'DY', 'NLS_DATE_LANGUAGE=GERMAN');
  v_time := TO_CHAR(SYSDATE, 'HH24:MI');

  IF v_day IN ('SA', 'SO') OR v_time NOT BETWEEN '08:45' AND '17:30' THEN
    RAISE_APPLICATION_ERROR(-20205, 'DML nur Mo-Fr 08:45-17:30 erlaubt.');
  END IF;
END;
/

-- 7
CREATE OR REPLACE TRIGGER trg_job_min_salary_update
AFTER UPDATE OF min_salary ON jobs
FOR EACH ROW
BEGIN
  IF :NEW.min_salary > :OLD.min_salary THEN
    UPDATE employees
    SET salary = GREATEST(salary, :NEW.min_salary)
    WHERE job_id = :NEW.job_id AND salary = :OLD.min_salary;
  END IF;
END;
/

-- 8a/b
ALTER TABLE departments ADD total_salary NUMBER(10,2);

CREATE OR REPLACE TRIGGER trg_dept_total_salary
FOR INSERT OR UPDATE OR DELETE ON employees
COMPOUND TRIGGER
  TYPE t_dept_list IS TABLE OF department_id%TYPE;
  g_depts t_dept_list := t_dept_list();

  AFTER EACH ROW IS
  BEGIN
    g_depts.EXTEND;
    g_depts(g_depts.LAST) := COALESCE(:NEW.department_id, :OLD.department_id);
  END AFTER EACH ROW;

  AFTER STATEMENT IS
  BEGIN
    FOR i IN 1..g_depts.COUNT LOOP
      UPDATE departments d
      SET total_salary = (SELECT NVL(SUM(salary), 0) FROM employees e WHERE e.department_id = g_depts(i))
      WHERE d.department_id = g_depts(i);
    END LOOP;
  END AFTER STATEMENT;
END;
/
-- Initial
UPDATE departments d SET total_salary = (SELECT NVL(SUM(e.salary), 0) FROM employees e WHERE e.department_id = d.department_id);

-- 9
CREATE TABLE emp_audit (
  audit_id NUMBER PRIMARY KEY,
  employee_id NUMBER,
  old_salary NUMBER,
  new_salary NUMBER,
  change_date DATE DEFAULT SYSDATE,
  user_name VARCHAR2(30) DEFAULT USER
);
CREATE SEQUENCE audit_seq START WITH 1;

CREATE OR REPLACE TRIGGER trg_emp_audit
AFTER INSERT OR UPDATE OR DELETE ON employees
FOR EACH ROW
BEGIN
  INSERT INTO emp_audit (audit_id, employee_id, old_salary, new_salary)
  VALUES (audit_seq.NEXTVAL, :OLD.employee_id,:OLD.salary, :NEW.salary);
END;
/

-- 11
CREATE OR REPLACE TRIGGER trg_salary_job_range
BEFORE INSERT OR UPDATE OF job_id, salary ON employees
FOR EACH ROW
DECLARE
  v_min NUMBER; v_max NUMBER;
BEGIN
  SELECT min_salary, max_salary
  INTO v_min, v_max
  FROM jobs
  WHERE job_id = NVL(:NEW.job_id, :OLD.job_id);

  IF NVL(:NEW.salary, 0) < v_min OR NVL(:NEW.salary, 0) > v_max THEN
    RAISE_APPLICATION_ERROR(-20004, 'Gehalt außerhalb Range für Job ' || NVL(:NEW.job_id, :OLD.job_id));
  END IF;
END;
/
