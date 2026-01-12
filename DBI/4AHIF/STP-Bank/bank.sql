-- a
go
create proc prcFaelligeTANBlaetter @BLZ int
as
begin
	print 'Nicht möglich!'
end
go


-- b
go
create proc prcTANERZEUGEN @blz int, @KontoNr int
as
begin
	update TANBlatt
	set TAN_Status = 2

	declare @i int
	set @i = 0

	declare @KundenNr int;
	select @KundenNr = Kunde.KundenNr from Konto join Kunde on Konto.KontoNr = @KontoNr and Konto.BLZ = @blz

	while @i < 50
	begin
		declare @Tan int;
		select @Tan = FLOOR(rand() * power(cast(6 as bigint), 6))

		insert into TANBlatt values (@KundenNr, coalesce((select count(*) from TANBlatt), 0)+1, @i, cast(@Tan as varchar(6)))
		
		select @i = @i + 1
	end
end


-- c
go
create proc prcUEBERWEISEN
	@BLZAbsender int,
	@KontoNrAbsender int,
	@BLZEmpf int,
	@KontoNrEmpf int,
	@Betrag int,
	@Buchungstext varchar(100),
	@TAN varchar(8)
as
begin
	declare @KundenNrEmpf int
	declare @KundenNrAbsender int
	declare @ziffern varchar(6)
	declare @index int

	select @ziffern = cast(right(@TAN, 6) as varchar(6))
	select @index = cast(left(@TAN, 2) as int)

	select @KundenNrEmpf = KundenNr from Konto where BLZ = @BLZEmpf and KontoNr = @KontoNrEmpf
	select @KundenNrAbsender = KundenNr from Konto where BLZ = @BLZAbsender and KontoNr = @KontoNrAbsender

	if not exists (
            select 1 from TANBlatt
            where KundenNr = @KundenNrAbsender
              and BlattIndex = @index
              and Ziffernfolge = @ziffern
              and TAN_Status = 0
        )
        begin
            raiserror('Ungültige TAN', 1, 51000)
        end

        -- Prüfe Konten
        if @KundenNrAbsender is null or @KundenNrEmpf is null
        begin
            raiserror('Ungültige Konten', 1, 51000)
        end

	update TANBlatt
        set TAN_Status = 1
        where KundenNr = @KundenNrAbsender
          and BlattIndex = @index
          and Ziffernfolge = @ziffern;

	-- Buche Lastschrift
    insert into Kontobewegung (Blz, KontoNr, Buchungszeile, Betrag, Datum)
    values (@BLZAbsender, @KontoNrAbsender, newid(), -@Betrag, getdate());

    -- Buche Gutschrift
    insert into Kontobewegung (Blz, KontoNr, Buchungszeile, Betrag, Datum)
    values (@BLZEmpf, @KontoNrEmpf, newid(), @Betrag, getdate());
end
go


-- d
go
create proc prcKontostand 
	@BLZ int,
	@Kontonummer int
as
begin
	declare @Ergebnis table (		
		Blz int,
		KontoNr int,
		Betrag int,
		Datum date)

	insert into @Ergebnis
	select 
		@BLZ, 
		kbwg.KontoNr, 
		SUM(kbwg.Betrag) OVER (ORDER BY kbwg.Datum ROWS UNBOUNDED PRECEDING),
		kbwg.Datum
	from Kontobewegung kbwg
	where Blz = @BLZ and kbwg.KontoNr = @Kontonummer
	order by kbwg.Datum;

	select * from @Ergebnis
end
go