use Fluege;
go

-- 1
go
create or alter function udfFlugminuten(@FzNr int)
returns time
as
begin
    declare @TotalMinutes int
    
    select @TotalMinutes = sum(datediff(minute, startzeit, landezeit))
    from fliegt
    where fznr = @FzNr
    
    return dateadd(minute, @TotalMinutes, '00:00:00')
end
go

select dbo.udfFlugminuten(1)

-- 2
go
create or alter function udfFluegeProTyp()
returns table
as
return
    select 
        p.pnr,
        p.pnname,
        p.pvname,
        ft.bezeichnung as Flugzeugtyp,
        count(*) as AnzahlFluege
    from pilot p
		join fliegt fl on p.pnr = fl.pnr
		join flugzeug fz on fl.fznr = fz.fznr
		join ftype ft on fz.tnr = ft.tnr
    group by p.pnr, p.pnname, p.pvname, ft.bezeichnung;
go

select *
from dbo.udfFluegeProTyp()

-- 3
go
create or alter function udfFlugaufkommen(@ANr int = -1)
returns @result table (
    ANr int,
    ABezeich char(3),
    Abfluege int,
    Ankuenfte int
)
as
begin
    insert into @result
    select 
        fh.fhnr as ANr,
        fh.Bezeichnung as ABezeich,
        count(distinct f1.fnr) as Abfluege,
        count(distinct f2.fnr) as Ankuenfte
    from flughafen fh
        left join flug f1 on fh.fhnr = f1.splatz
        left join flug f2 on fh.fhnr = f2.lplatz
    where @ANr = -1 or fh.fhnr = @ANr
    group by fh.fhnr, fh.Bezeichnung
    order by fh.fhnr;
    
    return;
end;
go

select * from dbo.udfFlugaufkommen(default);