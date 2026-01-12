#pragma once

#include <memory>
#include <QString>

// Forward-Deklaration
class alliance;

class Airline
{
public:
	long id;
	QString name;
	std::shared_ptr<alliance> m_alliance;

	Airline() : id(0), name(""), m_alliance(nullptr) {}
	virtual ~Airline();
};

QX_REGISTER_PRIMARY_KEY(Airline, long)
QX_REGISTER_HPP_QX_FLUGPLANER(Airline, qx::trait::no_base_class_defined, 1)
