#pragma once

#include <QString>
#include <QxOrm.h>

class Airport
{
public:
	Airport() : m_id(0), m_latitude(0.0f), m_longitude(0.0f) {}
	Airport(int id, float latitude, float longitude, const QString &name, const QString &iata)
		: m_id(id), m_latitude(latitude), m_longitude(longitude), m_name(name), m_iata(iata) {}

	virtual ~Airport() {}

	// Getters
	int getId() const { return m_id; }
	float getLatitude() const { return m_latitude; }
	float getLongitude() const { return m_longitude; }
	QString getName() const { return m_name; }
	QString getIata() const { return m_iata; }

	// Setters
	void setId(int id) { m_id = id; }
	void setLatitude(float lat) { m_latitude = lat; }
	void setLongitude(float lon) { m_longitude = lon; }
	void setName(const QString &name) { m_name = name; }
	void setIata(const QString &iata) { m_iata = iata; }

protected:
	long m_id;
	float m_latitude;
	float m_longitude;
	QString m_name;
	QString m_iata;

private:
	QX_REGISTER_FRIEND_CLASS(Airport)
};

QX_REGISTER_HPP_EXPORT_DLL_WATCOM_BORLAND(Airport, 0)
