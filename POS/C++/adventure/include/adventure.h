#ifndef ADVENTURE_H
#define ADVENTURE_H

#include <vector>
#include <map>
#include <list>

#include "text.h"
#include "location.h"
#include "object.h"

class Adventure
{
private:
    Adventure();
    ~Adventure();
    static Adventure *instance;
    std::vector<Text *> messages;
    std::map<int, Text *> actions;
    std::vector<Location *> locations;
    std::vector<Object *> objects;
    std::list<Object *> inventory;
    int visit(int location);

public:
    static Adventure *get_instance();
    void start_adventuring();
    bool is_carring(int object);
    std::string get_action(int number) { return actions[number]->get(); }
};

#endif