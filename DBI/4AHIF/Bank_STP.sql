-- a)
go
create procedure prcFaelligeTANBlaetter (@BLZ INT)
as
begin
	select *
	from Konto ko
		join Kunde ku on ku.KundenNr = ko.KundenNr
		join TANBlatt tb on tb.KundenNr = ku.KundenNr
	where tb.TAN_Status = 0 and ko.BLZ = @BLZ;
end
go;

-- b)
go
create procedure prcTANERZEUGEN (@BLZ int, @KontoNr INT)
as
begin
	declare @Tan char(6);
	declare @Index INT;
	declare @KundenNr INT;

	-- Select the KundeNr
	select @KundenNr = KundenNr from Konto where KontoNr = @KontoNr;

	declare @i INT = 0;

	while @i < 50
	begin
		select @TAN = CONCAT(
			CHAR(48 + ABS(CHECKSUM(NEWID())) % 10),
			CHAR(48 + ABS(CHECKSUM(NEWID())) % 10),
			CHAR(48 + ABS(CHECKSUM(NEWID())) % 10),
			CHAR(48 + ABS(CHECKSUM(NEWID())) % 10),
			CHAR(48 + ABS(CHECKSUM(NEWID())) % 10),
			CHAR(48 + ABS(CHECKSUM(NEWID())) % 10)
		);

		-- Insert the TAN 
		insert into TANBlatt(BlattNR, KundenNr, BlattIndex, Ziffernfolge, TAN_Status) values 
			(isnull((select max(BlattNR) from TANBlatt), 0)+1, @KundenNr, @i, @Tan, 1);

		select @i = @i + 1;
	end;
end;
go

-- c)
-- TAN: 12345678
--   |> 12 => Index
--   |> 345678 => TAN
go
create procedure prcUEBERWEISEN (@BLZAbs int, @KtoNrAbs int, @BLZEmpf int, @KtoNrEmpf int, @Betrag decimal(5,2), @Buchungstext varchar(50), @TAN char(6))
as 
begin
	declare @Index INT;
	declare @TanNr char(6);

	select @Index = cast(left(@TAN, 2) as int);
	select @TanNr = right(@TAN, 6);

	if exists (
		select 1
		from TANBlatt tb 
		where tb.Ziffernfolge = @TanNr 
			and tb.KundenNr = (select KundenNr from Konto where KontoNr = @KtoNrAbs) 
			and tb.TAN_Status = 0)
	begin
		update TANBlatt
		set TAN_Status = 1
		where Ziffernfolge = @TAN;

		insert into Kontobewegung values
			(@BLZAbs, @KtoNrAbs, (isnull((select max(Buchungszeile) from Kontobewegung), 0)+1), @Buchungstext, @Betrag * -1, getdate());

		insert into Kontobewegung values
			(@BLZEmpf, @KtoNrEmpf, (isnull((select max(Buchungszeile) from Kontobewegung), 0)+1), @Buchungstext, @Betrag, getdate());
	end;
end;

-- d)
drop procedure if exists prcKontostand;

go
create procedure prcKontostand (@BLZ int, @Kontonummer int)
as
begin
	create table #Kontostand
	(
		Blz int,
		KontoNr int,
		Betrag int,
		Datum date
	);

	insert into #Kontostand (BLZ, KontoNr, Betrag, Datum)
	select 
		@BLZ, 
		kbwg.KontoNr, 
		SUM(kbwg.Betrag) OVER (ORDER BY kbwg.Datum ROWS UNBOUNDED PRECEDING),
		kbwg.Datum
	from Kontobewegung kbwg
	where Blz = @BLZ and kbwg.KontoNr = @Kontonummer
	order by kbwg.Datum;

	select *
	from #Kontostand;
end;
go

exec prcKontostand @BLZ=1, @Kontonummer=100;