use Airport;
go

-- 1
go
create or alter function udffindData(@startDate date, @endDate date)
returns @res table 
	(Airline_code int, 
	Airline_name nvarchar(20), 
	Aircraft_code nvarchar(5), 
	Flight_number nvarchar(5), 
	Leg_number nvarchar(10),
	Passport_number nvarchar(10),
	First_name nvarchar(20),
	Minit nvarchar(5),
	Last_name varchar(20),
	Air_ticket_number nvarchar(5))
as
begin
	insert into @res
	select al.Airline_code, al.Airline_name, 
		fla.Aircraft_code, fl.Flight_number,
		fla.Leg_number,
		psa.Passport_number,
		psa.First_name,
		psa.Minit,
		psa.Last_name,
		psa.Air_ticket_number
	from Flight fl
		join Airline al on al.Airline_code = fl.Airline_code
		join Aircraft ac on ac.Airline_code = al.Airline_code
		join Flight_leg_A fla on fla.Aircraft_code = ac.Aircraft_code
		join Passenger_A psa on psa.Leg_number = fla.Leg_number
	where fla.Date_of_flight between @startDate and @endDate

	return
end
go

select * from dbo.udffindData('2018-1-1', '2018-12-21')

-- 2
go
create or alter function udfstatusInfo(@status nvarchar(50), @date date)
returns @res table 
	(Status nvarchar(10), 
	Remark nvarchar(50), 
	Arrival_terminal_number nvarchar(8),
	Staff_ID nvarchar(5),
	Leg_number nvarchar(10),
	Flight_number nvarchar(5),
	Passport_number nvarchar(10),
	Passenger_name nvarchar(40),
	Passenger_category nvarchar(30),
	Passenger_Requirement nvarchar(50),
	Shedule_date date)
as
begin
	insert into @res
	select 
		flb.Status,
		flb.Remark,
		flb.Arrival_teminal_number,
		aic.Staff_ID,
		fla.Leg_number,
		fl.Flight_number,
		psa.Passport_number,
		concat(psa.First_name, ' ', psa.Last_name),
		psc.Passenger_catogary,
		psr.Requirement,
		fsd.Date
	from Flight fl
		join Aircraft ac on ac.Airline_code = fl.Airline_code
		join Flight_leg_A fla on fla.Aircraft_code = ac.Aircraft_code
		join Flight_leg_B flb on flb.Arrival_teminal_number = fla.Arrival_teminal_number
		join Aircrew aic on aic.Airline_code = ac.Airline_code
		join Passenger_A psa on psa.Leg_number = fla.Leg_number
		join Passenger_requirements psr on psr.Passport_number = psa.Passport_number
		join Flight_shedule_date fsd on fsd.Flight_number = fl.Flight_number
		join Passenger_catogary psc on psc.Passport_number = psa.Passport_number
	where flb.Status = @status and fsd.Date = @date
	
	return
end
go

select * from dbo.udfstatusInfo('Canceled', '2018-12-21')

-- 3
go
create or alter proc stp_passExpire(@PassportNumber nvarchar(10))
as
begin
	if not exists (select 1 from Passenger_A psa where psa.Passport_number = @PassportNumber and psa.Date_of_Expire <= getdate())
		raiserror('Passengers Passport has been expired', 16, 1)
	else
		print('This Passenger has a valid Passport')
end
go

exec stp_passExpire @PassportNumber = 'M100123155'