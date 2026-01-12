#include "stdafx.h"
#include "alliance.h"
#include "Airline.h"
#include "QxOrm_Impl.h"

QX_REGISTER_CPP_QX_FLUGPLANER(alliance)

// Destruktor-Implementierung
alliance::~alliance()
{
}

namespace qx {
	template <> void register_class(QxClass<alliance>& t)
	{
		t.id(&alliance::id, "id");
		t.data(&alliance::name, "name");

		// Foreign-Key-Spalte in der Airline-Tabelle heiﬂt "alliance"
		t.relationOneToMany(&alliance::airlines, "list_airline", "alliance");
	}
}
