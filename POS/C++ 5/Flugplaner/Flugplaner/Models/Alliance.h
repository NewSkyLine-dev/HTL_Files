#pragma once

#include <QString>
#include <QxOrm.h>

class Alliance
{
public:
	Alliance() : m_id(0) {}
	Alliance(int id, const QString &name) : m_id(id), m_name(name) {}

	virtual ~Alliance() {}

	// Getters
	int getId() const { return m_id; }
	QString getName() const { return m_name; }

	// Setters
	void setId(int id) { m_id = id; }
	void setName(const QString &name) { m_name = name; }

protected:
	long m_id;
	QString m_name;

private:
	QX_REGISTER_FRIEND_CLASS(Alliance)
};

QX_REGISTER_HPP_EXPORT_DLL_WATCOM_BORLAND(Alliance, 0)