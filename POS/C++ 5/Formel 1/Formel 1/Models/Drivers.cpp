#include "stdafx.h"
#include "Drivers.h"
#include <QxOrm_Impl.h>

 QX_REGISTER_CPP_QX_FORMEL_1(Drivers)

namespace qx {
	template <> void register_class(QxClass<Drivers> &t)
	{
		t.id(&Drivers::driverId, "driverId");
		
		t.data(&Drivers::driverRef, "driverRef");
		t.data(&Drivers::number, "number");
		t.data(&Drivers::code, "code");
		t.data(&Drivers::forename, "forename");
		t.data(&Drivers::surname, "surname");
		t.data(&Drivers::dob, "dob");
		t.data(&Drivers::nationality, "nationality");
		t.data(&Drivers::url, "url");
	}
}