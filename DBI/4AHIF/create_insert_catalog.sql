create database Catalog;
go

use Catalog;
go

drop table if exists SupplierParts;
drop table if exists Parts;
drop table if exists Suppliers;

create table Suppliers (
  SupplierID       char(2)      not null primary key
, SupplierName     char(6)
, SupplierCity     char(6)
, SupplierDiscount decimal(5, 2) not null 
);

create table Parts (
  PartID    char(2)      not null primary key
, PartName  char(8)      not null
, PartColor char(5)      not null
, PartPrice decimal(8, 2) not null
, PartCity  char(6)      not null  
);

create table SupplierParts (
  SupplierID char(2)   not null references Suppliers(SupplierID)
, PartID     char(2)   not null references Parts(PartID)
, Amount     decimal(4) not null
, primary key (SupplierID, PartID)
);


-- DML
delete from SupplierParts;
delete from Suppliers;
delete from Parts;

insert into Suppliers 
               (SupplierID, SupplierName, SupplierCity, SupplierDiscount)
        select  'L1'      , 'Schmid'    , 'London'    ,               20 from dual
  union select  'L2'      , 'Jonas'     , 'Paris'     ,               10 from dual
  union select  'L3'      , 'Berger'    , 'Paris'     ,               30 from dual
  union select  'L4'      , 'Klein'     , 'London'    ,               20 from dual
  union select  'L5'      , 'Adam'      , 'Athen'     ,               30 from dual
;

insert into Parts
               (PartID, PartName  , PartColor, PartPrice, PartCity)
        select  'T1'  , 'Mutter'  , 'rot'    ,        12, 'London' from dual
  union select  'T2'  , 'Bolzen'  , 'gelb'   ,        17, 'Paris'  from dual
  union select  'T3'  , 'Schraube', 'blau'   ,        17, 'Rom'    from dual
  union select  'T4'  , 'Schraube', 'rot'    ,        14, 'London' from dual
  union select  'T5'  , 'Welle'   , 'blau'   ,        12, 'Paris'  from dual
  union select  'T6'  , 'Zahnrad' , 'rot'    ,        19, 'London' from dual
;

insert into SupplierParts
               (SupplierID, PartID, Amount)
        select  'L1'      , 'T1'  ,    300 from dual
  union select  'L1'      , 'T2'  ,    200 from dual
  union select  'L1'      , 'T3'  ,    400 from dual
  union select  'L1'      , 'T4'  ,    200 from dual
  union select  'L1'      , 'T5'  ,    100 from dual
  union select  'L1'      , 'T6'  ,    100 from dual
  union select  'L2'      , 'T1'  ,    300 from dual
  union select  'L2'      , 'T2'  ,    400 from dual
  union select  'L3'      , 'T2'  ,    200 from dual
  union select  'L4'      , 'T2'  ,    200 from dual
  union select  'L4'      , 'T4'  ,    300 from dual
  union select  'L4'      , 'T5'  ,    400 from dual
;

commit;