#ifndef LOCATION_H
#define LOCATION_H

#include <string>
#include <list>
#include <vector>

#include "travel.h"
#include "object.h"

class Adventure;
class Location
{
    friend Adventure;

private:
    int number;
    std::string header;
    std::string description;
    std::vector<Travel *> destinations;
    std::list<Object *> objects;

public:
    Location(int number, std::string header);
    ~Location();
    void add_description(std::string description);
    void add_destination(int target, int action);
    void add_object(Object *obj);
    Object *remove_movable_object(int selection);
    Travel *get_destination(int selection);
    Travel *get_alternative_destination(Travel *old);
    void print();
    int print_destinations(int start);
    int print_object_actions(int start);
    bool has_object(int object);
};

#endif