delete from geschult;
delete from flug;
delete from pilot;
delete from flugzeug;
delete from typ;
/

insert into typ(tnr,tbezeichnung,sitzplaetze) values(12,'Boeing 737',100);
insert into typ(tnr,tbezeichnung,sitzplaetze) values(13,'Cessna ',20);
insert into typ(tnr,tbezeichnung,sitzplaetze) values(14,'Draken ',2);
insert into typ(tnr,tbezeichnung,sitzplaetze) values(15,'Airbus ',140);
/

/
insert into pilot(pnr,pname,pvorname,gebdat) values
     (100,'Winner','Otto',To_Date('11.1.1980','dd.mm.yyyy'));
insert into pilot(pnr,pname,pvorname,gebdat) values
     (200,'Winner','Otto',To_Date('11.1.1980','dd.mm.yyyy'));
insert into pilot(pnr,pname,pvorname,gebdat) values
     (300,'Winner','Otto',To_Date('11.1.1980','dd.mm.yyyy'));
/
select * from pilot;
/
insert into flugzeug (fznr,kennzeichen,kaufdatum,tnr) values 
     (1234,'AB 123',To_Date('11.1.2021','dd.mm.yyyy'),12);
insert into flugzeug (fznr,kennzeichen,kaufdatum,tnr) values 
     (1235,'AB 123',To_Date('11.1.2022','dd.mm.yyyy'),12);
insert into flugzeug (fznr,kennzeichen,kaufdatum,tnr) values 
     (1236,'AB 123',To_Date('11.11.2021','dd.mm.yyyy'),12);
insert into flugzeug (fznr,kennzeichen,kaufdatum,tnr) values 
     (5235,'AB 123',To_Date('11.1.2022','dd.mm.yyyy'),13);
insert into flugzeug (fznr,kennzeichen,kaufdatum,tnr) values 
     (6236,'AB 123',To_Date('11.11.2021','dd.mm.yyyy'),14);
insert into flugzeug (fznr,kennzeichen,kaufdatum,tnr) values 
     (9235,'AB 123',To_Date('11.1.2022','dd.mm.yyyy'),15);
insert into flugzeug (fznr,kennzeichen,kaufdatum,tnr) values 
     (9236,'AB 123',To_Date('11.11.2021','dd.mm.yyyy'),15);
/

insert into geschult (pilot_pnr,typ_tnr) values(100,12);
insert into geschult (pilot_pnr,typ_tnr) values(100,13);
insert into geschult (pilot_pnr,typ_tnr) values(100,14);
insert into geschult (pilot_pnr,typ_tnr) values(100,15); --Fehler, geschult
insert into geschult (pilot_pnr,typ_tnr) values(200,12);
insert into geschult (pilot_pnr,typ_tnr) values(200,15);
insert into geschult (pilot_pnr,typ_tnr) values(300,13);
insert into geschult (pilot_pnr,typ_tnr) values(300,14);
/
/
update geschult set typ_tnr=12 where pilot_pnr=300 and typ_tnr=14; --Fehler, Primärschlüssel
delete from geschult where pilot_pnr=300 and typ_tnr=14;
insert into geschult (pilot_pnr,typ_tnr) values(300,14);
/

insert into flug (fnr,abflug,ankunft,von,nach,pnr,fznr) values
   (100,To_Date('11.1.2020 12:30','dd.mm.yyyy hh24:mi'),To_Date('11.1.2020 14:55','dd.mm.yyyy hh24:mi'),
    'Wien','Moskau',100,1234); 
insert into flug (fnr,abflug,ankunft,von,nach,pnr,fznr) values
   (100,To_Date('12.2.2020 12:30','dd.mm.yyyy hh24:mi'),To_Date('12.2.2020 14:55','dd.mm.yyyy hh24:mi'),
    'Wien','Moskau',100,1234); 
insert into flug (fnr,abflug,ankunft,von,nach,pnr,fznr) values
   (200,To_Date('7.1.2020 19:30','dd.mm.yyyy hh24:mi'),To_Date('7.1.2020 21:55','dd.mm.yyyy hh24:mi'),
    'Wien','Paris',200,9236);
    
insert into flug (fnr,abflug,ankunft,von,nach,pnr,fznr) values
   (200,To_Date('27.3.2020 19:30','dd.mm.yyyy hh24:mi'),To_Date('27.3.2020 21:55','dd.mm.yyyy hh24:mi'),
    'Wien','Paris',200,6236); --Fehler, Pilot nicht geschult
/    
insert into flug (fnr,abflug,ankunft,von,nach,pnr,fznr) values
   (300,To_Date('27.4.2020 19:30','dd.mm.yyyy hh24:mi'),To_Date('27.4.2020 21:55','dd.mm.yyyy hh24:mi'),
    'Wien','Paris',200,1235);
update flug set pnr=100 where fnr=300 and abflug =To_Date('27.4.2020 19:30','dd.mm.yyyy hh24:mi');
/
insert into flug (fnr,abflug,ankunft,von,nach,pnr,fznr) values
   (200,To_Date('27.3.2021 19:30','dd.mm.yyyy hh24:mi'),To_Date('27.3.2021 21:55','dd.mm.yyyy hh24:mi'),
    'Wien','Paris',200,9235); 
delete from geschult where PILOT_PNR=200 and typ_tnr=15; --Fehler, Pilot ist für Flugzeugtyp eingeteilt
delete from geschult where PILOT_PNR=200 and typ_tnr=12;
/

select * from geschult;
select * from typ;
select * from flug;
select * from flugzeug;
select * from pilot;
select * from geschult;



