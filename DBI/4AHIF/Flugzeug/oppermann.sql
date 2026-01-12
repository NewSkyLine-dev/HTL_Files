use Flughafen
go

-- 1
go
create or alter function udfSVNRpruefen(@SVNR nvarchar(10))
returns bit
as
begin
    declare @z1 int, @z2 int, @z3 int, @z4 int, @z5 int, @z6 int, @z7 int, @z8 int, @z9 int, @z10 int
	declare @Summe int, @Pruefwert int

	set @z1 = cast(substring(@SVNR, 1, 1) as int)
    set @z2 = cast(substring(@SVNR, 2, 1) as int)
    set @z3 = cast(substring(@SVNR, 3, 1) as int)
    set @z4 = cast(substring(@SVNR, 4, 1) as int)
    set @z5 = cast(substring(@SVNR, 5, 1) as int)
    set @z6 = cast(substring(@SVNR, 6, 1) as int)
    set @z7 = cast(substring(@SVNR, 7, 1) as int)
    set @z8 = cast(substring(@SVNR, 8, 1) as int)
    set @z9 = cast(substring(@SVNR, 9, 1) as int)
    set @z10 = cast(substring(@SVNR, 10, 1) as int)

    SET @Summe = (3*@z1 + 7*@z2 + 9*@z3 + 5*@z5 + 8*@z6 + 4*@z7 + 2*@z8 + @z9 + 6*@z10)
    SET @Pruefwert = @Summe % 11

    if @Pruefwert < 10 and @Pruefwert = @z4
        return 1
    else
        return 0

    return 0
end
go

select dbo.udfSVNRpruefen('5104140350') 

-- 2
go
create or alter function udfGebdat(@SVNR nvarchar(10))
returns date
as
begin
	declare @Geburtstag int, @Geburtsmonat int, @Geburtsjahr int
    declare @Geburtsdatum date

    set @Geburtstag = cast(substring(@SVNR, 5, 2) as int)
    set @Geburtsmonat = cast(substring(@SVNR, 7, 2) as int)
    set @Geburtsjahr = cast(substring(@SVNR, 9, 2) as int)

    if @Geburtsjahr >= 24
        set @Geburtsjahr = 1900 + @Geburtsjahr
    else
        set @Geburtsjahr = 2000 + @Geburtsjahr

    set @Geburtsdatum = datefromparts(@Geburtsjahr, @Geburtsmonat, @Geburtstag)

    return @Geburtsdatum
end
go

select dbo.udfGebdat('5104140350')

-- 3
go
create or alter proc stpGebDatSpeichern(@SVNR nvarchar(10))
as
begin
    declare @Valid bit, @Geburtsdatum date

    set @Valid = dbo.udfSVNRpruefen(@SVNR)

    if @Valid = 0
    begin
        raiserror('Ungültige SVNR', 16, 16)
        return
    end

    set @Geburtsdatum = dbo.udfGebdat(@SVNR)

    update pilot
    set pgebdat = @Geburtsdatum
    where svnr = @SVNR
end
go

-- 4
go
create or alter function odfFlugminute (@Fznr int)
returns table
as
return (
    select
        @Fznr as Fznr,
        sum(datediff(minute, startzeit, landezeit)) as Gesamt
    from fliegt
    where fznr = @Fznr
)
go

select *
from odfFlugminute(1)

-- 5
go
create or alter function udfFluegeProTyp()
returns table
as
return (
    select
        p.pnr,
        t.tnr,
        count(f.fnr) as Anzahl
    from fliegt f
        join flugzeug z on z.fznr = f.fznr
        join ftype t on t.tnr = z.tnr
        join pilot p on p.pnr = f.pnr
    group by p.pnr, t.tnr
)
go

select *
from udfFluegeProTyp()

-- 6
go
create or alter function udfFlugaufkommen (@Fhnr int = -1)
returns table
as
return (
    select
        fh.fhnr as FHNr, 
        fh.Bezeichnung as Bezeich, 
        fh.land as Land, 
        count(case when fl.splatz = fh.fhnr then 1 end) as Abflüge,
        count(case when fl.lplatz = fh.fhnr then 1 end) as Ankünfte
    from flughafen fh
        left join flug fl on fh.fhnr in (fl.splatz, fl.lplatz)
    where (@Fhnr = -1 or fh.fhnr = @Fhnr)
    group by fh.fhnr, fh.Bezeichnung, fh.land
)
go

select *
from udfFlugaufkommen(default)