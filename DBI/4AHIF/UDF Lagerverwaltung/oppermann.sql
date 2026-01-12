use lagerverwaltung
go

go
alter table dbo.artikel
add constraint DF_Artikel_Preis default 12 for preis
go

-- 1
go
create or alter function Lagerwert(@Anr int)
returns decimal(10,2)
as
begin
	return (
		select count(*) * (select preis from artikel where anr = @anr)
		from dbo.lieferung l
			join dbo.artikel ar on ar.anr = l.anr
		where l.anr = @Anr
		group by l.anr)
end
go

select dbo.Lagerwert(1)

-- 2
go
create or alter function LetzterMonatstag(@Datum datetime)
returns date
as
begin
	return eomonth(@Datum)
end
go

select dbo.LetzterMonatstag('02-17-2025')

-- 3
go
create or alter function Artikelliste(@Lnr int)
returns @Ausgabe table (Bezeichnung nvarchar(50), Stueckzahl int)
as
begin
	insert @Ausgabe
	select a.bezeichnung, sum(li.stueck)
	from lager l
		join lieferung li on li.lnr = l.lnr
		join artikel a on a.anr = li.anr
	where l.lnr = @Lnr
	group by li.anr, a.bezeichnung

	return
end
go

select * from dbo.Artikelliste(1)

-- 4
go
create or alter function Uebersicht(@Artbezeichnung nvarchar(20))
returns @Ausgabe table (LNr int, Ort nvarchar(50), Stueckzahl int, FreieKapazitaet int)
as
begin
	insert @Ausgabe
	select la.lnr, la.ort, sum(l.stueck), 
		case when (la.stueckkap - isnull(sum(l.stueck), 0)) < 0 
			then 0 
			else (la.stueckkap - isnull(sum(l.stueck), 0)) 
		end as FreieKapazitaet
	from artikel a
		join lieferung l on l.anr = a.anr
		join lager la on la.lnr = l.lnr
	where a.bezeichnung = @Artbezeichnung
	group by la.lnr, la.ort, la.stueckkap

	return
end
go

select * from dbo.Uebersicht('Artikel1')