DROP TABLE if exists ltp;
DROP TABLE if exists p;
DROP TABLE if exists t;
DROP TABLE if exists l;
go
 
CREATE TABLE l (
       lnr    CHAR(2) PRIMARY KEY,
       lname  VARCHAR(6),
       rabatt DECIMAL(2),
       stadt  VARCHAR(6));
CREATE TABLE t (
       tnr    CHAR(2) PRIMARY KEY,
       tname  VARCHAR(8),
       farbe  VARCHAR(5),
       preis  DECIMAL(3),
       stadt  VARCHAR(6));
CREATE TABLE p (
       pnr    CHAR(2) PRIMARY KEY,
       pname  CHAR(10),
       stadt  VARCHAR(6));
CREATE TABLE ltp (
       lnr    CHAR(2) REFERENCES l,
       tnr    CHAR(2) REFERENCES t,
       pnr    CHAR(2) REFERENCES p,
       menge  DECIMAL(4),
       PRIMARY KEY (lnr,tnr,pnr));
go

INSERT INTO l(lnr,lname,rabatt,stadt) VALUES ('L1','Schmid',20,'London');
INSERT INTO l(lnr,lname,rabatt,stadt) VALUES ('L2','Jonas', 10,'Paris' );
INSERT INTO l(lnr,lname,rabatt,stadt) VALUES ('L3','Berger',30,'Paris' );
INSERT INTO l(lnr,lname,rabatt,stadt) VALUES ('L4','Klein', 20,'London');
INSERT INTO l(lnr,lname,rabatt,stadt) VALUES ('L5','Adam',  30,'Athen' );


INSERT INTO t(tnr,tname,farbe,preis,stadt) VALUES ('T1','Mutter',  'rot',  12,'London');
INSERT INTO t(tnr,tname,farbe,preis,stadt) VALUES ('T2','Bolzen',  'gelb', 17,'Paris' );
INSERT INTO t(tnr,tname,farbe,preis,stadt) VALUES ('T3','Schraube','blau', 17,'Rom'   );
INSERT INTO t(tnr,tname,farbe,preis,stadt) VALUES ('T4','Schraube','rot',  14,'London');
INSERT INTO t(tnr,tname,farbe,preis,stadt) VALUES ('T5','Welle',   'blau', 12,'Paris' );
INSERT INTO t(tnr,tname,farbe,preis,stadt) VALUES ('T6','Zahnrad', 'rot',  19,'London');

INSERT INTO p(pnr,pname,stadt) VALUES ('P1','Flugzeug',  'Paris' );
INSERT INTO p(pnr,pname,stadt) VALUES ('P2','Schiff',    'Rom'   );
INSERT INTO p(pnr,pname,stadt) VALUES ('P3','Seilbahn',  'Athen' );
INSERT INTO p(pnr,pname,stadt) VALUES ('P4','Schiff',    'Athen' );
INSERT INTO p(pnr,pname,stadt) VALUES ('P5','Eisenbahn', 'London');
INSERT INTO p(pnr,pname,stadt) VALUES ('P6','Flugzeug',  'Oslo'  );
INSERT INTO p(pnr,pname,stadt) VALUES ('P7','Autobus',   'London');

INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L1','T1','P1',200);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L1','T1','P4',700);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T3','P1',400);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T3','P2',200);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T3','P3',200);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T3','P4',500);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T3','P5',600);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T3','P6',400);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T3','P7',800);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L2','T5','P2',100);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L3','T3','P1',200);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L3','T4','P2',500);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L4','T6','P3',300);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L4','T6','P7',300);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T1','P4',100);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T2','P2',200);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T2','P4',100);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T3','P4',200);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T4','P4',800);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T5','P4',400);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T5','P5',500);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T5','P7',100);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T6','P2',200);
INSERT INTO ltp(lnr,tnr,pnr,menge) VALUES ('L5','T6','P4',500);
go
