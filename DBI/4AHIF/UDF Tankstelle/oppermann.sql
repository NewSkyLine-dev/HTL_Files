use Tankstelle
go


-- a
drop function if exists udfTag_mit_MaxUmsatz

go
create or alter function udfTag_mit_MaxUmsatz(@Monat int)
returns datetime
as
begin
	declare @maxtag date

    select top 1 @maxtag = convert(date, VerkaufsZeitpunkt)
    from Verkauf v
        join Zapfsaeule z on z.ZNr = v.ZNr
        join Tagespreis t on t.KName = z.KName
    where month(v.VerkaufsZeitpunkt) = @Monat
    group by convert(date, VerkaufsZeitpunkt)
    order by sum(v.MengeL * t.Preis) desc

    return @maxtag
end
go

go
select dbo.udfTag_mit_MaxUmsatz (10)
go


-- b
drop function if exists udfPreisaenderung_in_Prozent

go
create or alter function udfPreisaenderung_in_Prozent(@AlterPreis decimal(10,4), @NeuerPreis decimal(10,4))
returns decimal(10,4)
as
begin
	return ((@NeuerPreis - @AlterPreis) / @AlterPreis) * 100
end
go

go
declare @res decimal(10,4)
select @res = dbo.udfPreisaenderung_in_Prozent(10.22, 10.44)
select @res
go

-- c
drop proc if exists stpPreisentwicklung

go
create or alter proc stpPreisentwicklung 
    @KName nvarchar(50), 
    @Monat int
as
begin
    if not exists (select 1 from Kraftstoff where KName = @KName)
        raiserror ('Der angegebene Kraftstoff exisitiert nicht', 16, 1)

    if @Monat not between 1 and 12
        raiserror ('Monat muss zwischen 1 und 12 sein', 16, 1)
    
    declare @Entwicklung table (Tagesdatum date, preis decimal(10,4), Veraenderun decimal(10,4), ProzentuelleAenderung decimal(10,4))

    insert into @Entwicklung (Tagesdatum, Preis)
    select Tagesdatum, Preis
    from Tagespreis
    where KName = @KName and month(Tagesdatum) = @Monat
    order by Tagesdatum;

    with PrevPrices as (
        select 
            Tagesdatum,
            Preis,
            coalesce(lag(Preis) over (order by Tagesdatum),0) as PrevPrice
        from @Entwicklung
    )
    update e
    set 
        e.Veraenderun = e.Preis - coalesce(p.PrevPrice, 0),
        e.ProzentuelleAenderung = case 
            when p.PrevPrice > 0
            then dbo.udfPreisaenderung_in_Prozent(p.PrevPrice, e.Preis)
            else p.PrevPrice
        end
    from @Entwicklung e
        join PrevPrices p on e.Tagesdatum = p.Tagesdatum
       
    select *
    from @Entwicklung
end
go

go
exec dbo.stpPreisentwicklung 'Benzin', 10
go

-- d
drop proc if exists stpBetrag

go
create or alter proc stpBetrag 
    @ZNr int, 
    @Menge decimal(10,2), 
    @ZahlenderBetrag decimal(10,2) output
as
begin
    if not exists (select 1 from Zapfsaeule where ZNr = @ZNr)
        raiserror ('Die angegebene Zapfsäule exisitiert nicht', 16, 1)

    declare @Preis decimal(10,4), @KName nvarchar(50), @aktMenge decimal(10,2)

    select @KName = KName, @aktMenge = aktMengeL
    from Zapfsaeule
    where ZNr = @ZNr
    
    if @aktMenge < @Menge
    begin
        raiserror('Nicht genügend Kraftstoff in der Zapfsäule', 16, 1)
        return
    end

    select top 1 @Preis = Preis
    from Tagespreis
    where KName = @KName
    order by Tagesdatum desc

    set @ZahlenderBetrag = @Menge * @Preis

    insert into Verkauf (MengeL, VerkaufsZeitpunkt, ZNr) values (@Menge, getdate(), @ZNr)

    update Zapfsaeule set aktMengeL = aktMengeL - @Menge where ZNr = @ZNr
end
go