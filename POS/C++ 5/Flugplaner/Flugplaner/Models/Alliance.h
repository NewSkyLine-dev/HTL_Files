#pragma once

#include <QString>
#include <QSqlQuery>

class Alliance
{
public:
	Alliance(int id, const QString& name) : id(id), name(name) {}

	Alliance() : id(0), name("") {}

	// Constructor to create Alliance from QSqlQuery result
	Alliance(QSqlQuery& query)
		: id(query.value("id").toInt()),
		name(query.value("name").toString()) {
	}

	~Alliance() {}

	int id;
	QString name;
};