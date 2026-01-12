#include <stdexcept>
#include <iostream>

#include "location.h"
#include "adventure.h"

using namespace std;

Location::Location(int number, std::string header)
{
    this->number = number;
    this->header = header;
}

Location::~Location()
{
}

void Location::add_description(std::string description)
{

    if (description.size() > 0)
    {
        this->description += "\n" + description;
    }
    else
    {
        this->description = description;
    }
}

void Location::add_destination(int target, int action)
{
    if (target < 999000)
    {
        destinations.push_back(new Travel(target, action));
    }
}

Travel *Location::get_destination(int selection)
{
    vector<int> targets;
    for (int i = 0; i < destinations.size(); i++)
    {
        bool found = false;
        for (auto it = targets.begin(); it != targets.end(); it++)
        {
            if (*it == destinations[i]->get_action())
            {
                found = true;
                break;
            }
        }
        if (!found)
        {
            if (selection == 0)
            {
                return destinations[i];
            }
            selection--;
        }
    }
    return destinations.back();
}

Travel *Location::get_alternative_destination(Travel *old)
{
    bool found = false;
    for (auto it = destinations.begin(); it != destinations.end(); it++)
    {
        if (!found)
        {
            if (*it == old)
            {
                found = true;
            }
        }
        else
        {
            if ((*it)->get_target() != old->get_target())
            {
                return *it;
            }
        }
    }
    return old;
}

void Location::add_object(Object *obj)
{
    objects.push_back(obj);
}

Object *Location::remove_movable_object(int selection)
{
    for (std::list<Object *>::iterator it = objects.begin(); it != objects.end(); ++it)
    {
        if ((*it)->is_movable())
        {
            if (selection > 0)
            {
                selection--;
            }
            else
            {
                Object *obj = *it;
                objects.erase(it);
                return obj;
            }
        }
    }
    throw invalid_argument("Could not find movable object to remove!");
}

void Location::print()
{
    cout << header << endl;
    cout << description << endl
         << endl;
    for (std::list<Object *>::iterator it = objects.begin(); it != objects.end(); ++it)
    {
        cout << (*it)->get() << endl;
    }
    cout << endl;
}

int Location::print_destinations(int start)
{
    int count = 0;
    vector<int> targets;
    for (int i = 0; i < destinations.size(); i++)
    {
        bool found = false;
        for (auto it = targets.begin(); it != targets.end(); it++)
        {
            if (*it == destinations[i]->get_action())
            {
                found = true;
                break;
            }
        }
        if (!found)
        {
            destinations[i]->print(count + start);
            count++;
            targets.push_back(destinations[i]->get_action());
        }
    }

    return start + count;
}

int Location::print_object_actions(int start)
{
    for (std::list<Object *>::iterator it = objects.begin(); it != objects.end(); ++it)
    {
        if ((*it)->is_movable())
        {
            cout << start << ") Take " << (*it)->get_name() << endl;
            start++;
        }
    }
    return start;
}

bool Location::has_object(int object)
{
    for (auto it = objects.begin(); it != objects.end(); it++)
    {
        if ((*it)->get_number() == object)
        {
            return true;
        }
    }
    return false;
}