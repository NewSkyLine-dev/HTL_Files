#pragma once

#include <memory>
#include <vector>
#include <QString>

// Forward-Deklaration
class Airline;

class alliance
{ 
public:
    typedef std::shared_ptr<Airline> airline_ptr;
    typedef std::vector<airline_ptr> list_airline;
    
    long id;
    QString name;
    list_airline airlines;
    
    alliance() : id(0), name("") {}
    virtual ~alliance();
};

QX_REGISTER_PRIMARY_KEY(alliance, long)
QX_REGISTER_HPP_QX_FLUGPLANER(alliance, qx::trait::no_base_class_defined, 1)

typedef std::vector<std::shared_ptr<alliance>> alliance_list;