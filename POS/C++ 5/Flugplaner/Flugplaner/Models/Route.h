#pragma once

#include <QxOrm.h>

class Route
{
public:
	Route() : m_airline(0), m_airport1(0), m_airport2(0) {}
	Route(int airline, int airport1, int airport2)
		: m_airline(airline), m_airport1(airport1), m_airport2(airport2) {}

	virtual ~Route() {}

	// Getters
	int getAirline() const { return m_airline; }
	int getAirport1() const { return m_airport1; }
	int getAirport2() const { return m_airport2; }

	// Setters
	void setAirline(int airline) { m_airline = airline; }
	void setAirport1(int airport1) { m_airport1 = airport1; }
	void setAirport2(int airport2) { m_airport2 = airport2; }

protected:
	long m_airline;
	long m_airport1;
	long m_airport2;

private:
	QX_REGISTER_FRIEND_CLASS(Route)
};

QX_REGISTER_HPP_EXPORT_DLL_WATCOM_BORLAND(Route, 0)