#pragma once
#include <QString>
#include <QSqlQuery>

class Airline
{
public:
	Airline(int id, QString name, int alliance)
		: id(id), name(name), alliance(alliance) {
	}

	Airline() : id(0), name(""), alliance(0) {}

	// Constructor to create Airline from QSqlQuery result
	Airline(QSqlQuery& query)
		: id(query.value("id").toInt()),
		name(query.value("name").toString()),
		alliance(query.value("alliance").toInt()) {
	}

	~Airline() {}

	int id;
	QString name;
	int alliance;
};