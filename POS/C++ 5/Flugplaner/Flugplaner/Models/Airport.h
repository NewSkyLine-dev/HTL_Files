#pragma once

#include <QString>
#include <QSqlQuery>

class Airport
{
public:
	Airport(int id, float latitude, float longitude, QString name, QString iata)
		: id(id), latitude(latitude), longitude(longitude), name(name), iata(iata) {}

	Airport() : id(0), latitude(0.0f), longitude(0.0f), name(""), iata("") {}

	// Constructor to create Airport from QSqlQuery result
	Airport(QSqlQuery& query)
		: id(query.value("id").toInt()),
		latitude(query.value("latitude").toFloat()),
		longitude(query.value("longitude").toFloat()),
		name(query.value("name").toString()),
		iata(query.value("iata").toString()) {
	}

	~Airport() {}

	int id;
	float latitude;
	float longitude;
	QString name;
	QString iata;
};

