#pragma once

#include <QSqlQuery>

class Route
{
public:
	Route(int airline, int airport1, int airport2) : airline(airline), airport1(airport1), airport2(airport2) {}

	Route() : airline(0), airport1(0), airport2(0) {}

	// Constructor to create Route from QSqlQuery result
	Route(QSqlQuery& query)
		: airline(query.value("airline").toInt()),
		airport1(query.value("airport1").toInt()),
		airport2(query.value("airport2").toInt()) {
	}

	~Route() {}

	int airline;
	int airport1;
	int airport2;
};