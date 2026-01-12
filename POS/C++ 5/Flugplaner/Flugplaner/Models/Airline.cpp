#include "stdafx.h"
#include "Airline.h"
#include "alliance.h"
#include "QxOrm_Impl.h"

QX_REGISTER_CPP_QX_FLUGPLANER(Airline)

Airline::~Airline()
{
}

namespace qx {
	template <> void register_class(QxClass<Airline>& t)
	{
		t.id(&Airline::id, "id");
		t.data(&Airline::name, "name");

		// WICHTIG: Foreign-Key-Spalte heißt "alliance" (nicht "alliance_id")
		t.relationManyToOne(&Airline::m_alliance, "alliance");
	}
}
