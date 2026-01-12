create database raid5;
use raid5;
go

create table DiskArray (
	BlockNo int primary key,
	Disk1 tinyint,
	Disk2 tinyint,
	Disk3 tinyint,
	ParityDisk tinyint
);

insert into DiskArray (Disk1, Disk2, Disk3, ParityDisk) values
(ASCII('W'), ASCII('F'), ASCII('D'), null),
(ASCII('i'), ASCII('i'), ASCII('a'), null),
(ASCII('c'), ASCII('r'), ASCII('t'), null),
(ASCII('h'), ASCII(' '), ASCII('m'), null),
(ASCII('e'), ASCII('t'), ASCII('n'), null),
(ASCII('i'), ASCII('n'), NULL, null),
(ASCII('e'), NULL, NULL, null);

update DiskArray
set ParityDisk = coalesce(Disk1, 0) ^ coalesce(Disk2, 0) ^ coalesce(Disk3, 0);

select BlockNo,
	char(Disk1) as Disk1_Char,
	char(Disk2) as Disk2_Char,
	char(Disk3) as Disk3_Char,
	char(ParityDisk) as Parity_Char
from DiskArray;

update DiskArray set Disk1 = null;
select * from DiskArray;

update DiskArray
set Disk1 = ParityDisk ^ coalesce(Disk2, 0) ^ coalesce(Disk3, 0);
select * from DiskArray;

update DiskArray set Disk2 = null;
update DiskArray
set Disk2 = ParityDisk ^ coalesce(Disk2, 0) ^ coalesce(Disk3, 0);
select * from DiskArray;

UPDATE DiskArray SET Disk3 = NULL;
UPDATE DiskArray 
SET Disk3 = ParityDisk ^ COALESCE(Disk1, 0) ^ COALESCE(Disk2, 0);
SELECT * FROM DiskArray;

UPDATE DiskArray SET ParityDisk = NULL;
UPDATE DiskArray 
SET ParityDisk = COALESCE(Disk1, 0) ^ COALESCE(Disk2, 0) ^ COALESCE(Disk3, 0);
SELECT * FROM DiskArray;
