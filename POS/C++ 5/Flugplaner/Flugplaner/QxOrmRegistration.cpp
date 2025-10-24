#include <QxOrm_Impl.h>

#include "Models/Airport.h"
#include "Models/Airline.h"
#include "Models/Route.h"
#include "Models/Alliance.h"

// This file explicitly instantiates the QxOrm template registrations
// to ensure the models are properly registered at link time

namespace qx
{

    // Airport registration
    template <>
    void register_class(QxClass<Airport> &t)
    {
        t.id(&Airport::m_id, "id");
        t.data(&Airport::m_latitude, "latitude");
        t.data(&Airport::m_longitude, "longitude");
        t.data(&Airport::m_name, "name");
        t.data(&Airport::m_iata, "iata");
    }

    // Airline registration
    template <>
    void register_class(QxClass<Airline> &t)
    {
        t.id(&Airline::m_id, "id");
        t.data(&Airline::m_name, "name");
        t.data(&Airline::m_alliance, "alliance");
    }

    // Route registration
    template <>
    void register_class(QxClass<Route> &t)
    {
        t.id(&Route::m_airline, "airline");
        t.data(&Route::m_airport1, "airport1");
        t.data(&Route::m_airport2, "airport2");
    }

    // Alliance registration
    template <>
    void register_class(QxClass<Alliance> &t)
    {
        t.id(&Alliance::m_id, "id");
        t.data(&Alliance::m_name, "name");
    }

} // namespace qx
