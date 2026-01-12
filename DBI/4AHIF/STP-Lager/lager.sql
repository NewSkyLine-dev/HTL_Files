-- 1
go
create proc Anlieferung @ANr int, @Datum datetime, @Stueck int
as
begin
    -- Table variable to store the results
    declare @ErgbTabelle table (
        LNr int,
        ANr int,
        Datum datetime,
        Stueck int
    );

    -- Declare variables for the cursor
    declare @LNr int, @Ort varchar(50), @MaxKap int, @CurrentStueck int;

    -- Cursor to iterate over all warehouses
    declare LagerCursor cursor for
    select l.LNr, l.Ort, l.StueckKap
    from Lager l;

    open LagerCursor;

    fetch next from LagerCursor into @LNr, @Ort, @MaxKap;

    while @@FETCH_STATUS = 0 and @Stueck > 0
    begin
        -- Calculate the current usage of the warehouse for the given product
        select @CurrentStueck = isnull(sum(Stueck), 0)
        from Lieferung
        where LNr = @LNr and ANr = @ANr;

        -- Check available capacity
        if @CurrentStueck < @MaxKap
        begin
            -- Determine how much can be delivered to this warehouse
            declare @DeliverStueck int;
            set @DeliverStueck = 
                case 
                    when @Stueck <= (@MaxKap - @CurrentStueck) then @Stueck
                    else (@MaxKap - @CurrentStueck)
                end;

            -- Insert into the result table
            insert into @ErgbTabelle (LNr, ANr, Datum, Stueck)
            values (@LNr, @ANr, @Datum, @DeliverStueck);

            -- Reduce remaining delivery amount
            set @Stueck = @Stueck - @DeliverStueck;
        end;

        -- Fetch the next warehouse
        fetch next from LagerCursor into @LNr, @Ort, @MaxKap;
    end;

    close LagerCursor;
    deallocate LagerCursor;

    -- Output the results
    select * from @ErgbTabelle;
end;
go


-- 4
drop proc if exists Bestand;

go
create proc Bestand as
begin
    declare @ErgbTabelle table (Bezeichnung varchar(50), Ort varchar(50), Datum date, Stueck int)
    declare @Beschreibung varchar(50)

    declare ProduktCursor cursor for
    select distinct p.description
    from Lieferung lf
        join product p on p.prodnr = lf.ANr

    open ProduktCursor
    fetch next from ProduktCursor into @Beschreibung

    while @@FETCH_STATUS = 0
    begin
        declare @LetzterOrt varchar(50)

        insert into @ErgbTabelle
        select 
            case when ROW_NUMBER() over (order by l.Ort, lf.Datum) = 1 then @Beschreibung else '' end as Bezeichnung,
            case when Ort = @LetzterOrt then '' else Ort end as Ort,
            Datum,
            Stueck
        from Lieferung lf
            join Lager l on lf.LNr = l.LNr
            join product p on p.prodnr = lf.ANr
        where p.description = @Beschreibung
        order by l.Ort, lf.Datum

        select @LetzterOrt = Ort from Lieferung lf             
            join Lager l on lf.LNr = l.LNr
            join product p on p.prodnr = lf.ANr
        where p.description = @Beschreibung
        order by l.Ort, lf.Datum

        insert into @ErgbTabelle(Bezeichnung, Stueck) values
        ('*** Sume', (
            select sum(stueck) 
            from Lieferung lf 
                join product p on p.prodnr = lf.ANr 
            where p.description = @Beschreibung))

        fetch next from ProduktCursor into @Beschreibung
    end

    close ProduktCursor
    deallocate ProduktCursor

    select * from @ErgbTabelle
end
go

exec Bestand

-- 5
drop proc if exists lagbest

go
create proc lagbest @LNr int
as
begin
    select Ort, StueckKap
    from Lager
    where LNr = @LNr

    select p.description, sum(lf.Stueck) over (partition by p.prodnr)
    from Lieferung lf
        join Lager l on lf.LNr = l.LNr
        join product p on p.prodnr = lf.ANr
    where lf.LNr = @LNr
    -- group by p.description
end
go

exec lagbest 1

-- 6
drop proc if exists lagbestmulti

go
create proc lagbestmulti @ANr int
as
begin
    declare @LNr int, @ProdNr int

    declare LieferungCursor cursor for
    select LNr, ANr
    from Lieferung lf
    where lf.ANr = @ANr

    open LieferungCursor
    fetch next from LieferungCursor into @LNr, @ProdNr

    while @@FETCH_STATUS = 0
    begin
        select l.Ort, p.description, sum(lf.stueck) Bestand
        from Lieferung lf
            join lager l on l.LNr = lf.LNr
            join product p on lf.ANr = p.prodnr
        where lf.ANr = @ANr and lf.LNr = @LNr
        group by l.Ort, p.description

        fetch next from LieferungCursor into @LNr, @ProdNr
    end

    close LieferungCursor
    deallocate LieferungCursor
end
go

exec lagbestmulti 1