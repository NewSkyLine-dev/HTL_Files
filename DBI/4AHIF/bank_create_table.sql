
drop table istKunde
drop table betreut
drop table Kontobewegung
drop table TANBlatt
drop table konto
drop table bank
drop table Kunde
go

create table Bank(
    BLZ int identity(1,1) primary key,
	Adresse varchar(60),
)
go

create table Kunde(
	KundenNr int primary key,
	KName varchar(35),
	KVName varchar(35),	 
	KAdresse varchar(35)
)
go

create table Konto(
	BLZ int  references bank,
	KontoNr int,		
	KundenNr int references Kunde,
    primary key(BLZ,KontoNr)
)
go

create table Kontobewegung(
	Blz int ,
	KontoNr int,
	Buchungszeile int,
    primary key(BLZ,KontoNr,Buchungszeile),
    Buchungstext varchar(35),
    Betrag decimal(8,2),
    Datum datetime 
)
go

create table betreut(
	BLZ int,
	KundenNr bit,	
	Primary key(BLZ,KundenNr)
)
go

create table TANBlatt(
	KundenNr int references Kunde,
	BlattNR int,
	BlattIndex char(2),
    primary key(KundenNr,BlattNr,BlattIndex),
    Ziffernfolge char(6),
    TAN_Status int check (tan_status in(0,1,2))
)
go