#pragma once

#include <QString>
#include <QxOrm.h>

class Airline
{
public:
	Airline() : m_id(0), m_alliance(0) {}
	Airline(int id, const QString &name, int alliance)
		: m_id(id), m_name(name), m_alliance(alliance) {}

	virtual ~Airline() {}

	// Getters
	int getId() const { return m_id; }
	QString getName() const { return m_name; }
	int getAlliance() const { return m_alliance; }

	// Setters
	void setId(int id) { m_id = id; }
	void setName(const QString &name) { m_name = name; }
	void setAlliance(int alliance) { m_alliance = alliance; }

protected:
	long m_id;
	QString m_name;
	long m_alliance;

private:
	QX_REGISTER_FRIEND_CLASS(Airline)
};

QX_REGISTER_HPP_EXPORT_DLL_WATCOM_BORLAND(Airline, 0);