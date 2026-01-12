#pragma once

#include <QString>

class Drivers
{
public:
	Drivers() : driverId(0) { ; }
	Drivers(long driverId, const QString &driverRef, const QString &number, const QString &code, const QString &forename, const QString &surname,
			const QString &dob, const QString &nationality, const QString &url) :
		driverId(driverId),
		driverRef(driverRef),
		number(number),
		code(code),
		forename(forename),
		surname(surname),
		dob(dob),
		nationality(nationality),
		url(url) { ; }
		
	virtual ~Drivers() { ; }
	long driverId;
	QString driverRef;
	QString number;
	QString code;
	QString forename;
	QString surname;
	QString dob;
	QString nationality;
	QString url;
};


QX_REGISTER_HPP_QX_FORMEL_1(Drivers, qx::trait::no_base_class_defined, 1)