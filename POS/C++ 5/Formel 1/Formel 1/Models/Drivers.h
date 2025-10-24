#pragma once

#include <QString>

class QX_FORMEL_1_DLL_EXPORT Drivers
{
public:
	Drivers() : driverId(0) { ; }
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

typedef std::shared_ptr<Drivers> driver_ptr;