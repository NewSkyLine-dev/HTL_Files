#include <fstream>
#include <iostream>
#include <math.h>
#include <stdlib.h>
#include <time.h>

#include "adventure.h"

using namespace std;

Adventure *Adventure::instance = nullptr;

Adventure *Adventure::get_instance()
{
    if (!instance)
    {
        instance = new Adventure();
    }
    return instance;
}

Adventure::Adventure()
{
    srand(time(NULL));
    // Load Messages
    ifstream messages_file("../data/messages.txt"); // Open the file
    std::string line;                               // For saving each line
    int number = 0;                                 // For saving number of message
    bool first = true;                              // For checking if first line

    if (!messages_file.is_open())
    {
        cout << "Unable to open file" << endl;
        exit(1);
    }
    while (getline(messages_file, line))
    {
        if (!line.empty())
        {
            if (line[0] == '#')
            {
                number = stoi(line.substr(1));
                first = true;
            }
            else if (first)
            {
                Text *text = new Text(number, line);
                messages.push_back(text);
                first = false;
            }
            else
            {
                Text *text = messages[number - 1];
                text->add(line);
            }
        }
    }
    messages_file.close();

    // Load Actions
    ifstream actions_file("../data/actions.txt");
    if (!actions_file.is_open())
    {
        cout << "Unable to open file" << endl;
        exit(1);
    }
    while (getline(actions_file, line))
    {
        if (!line.empty())
        {
            size_t seperator = line.find_first_of(';');
            if (seperator != std::string::npos)
            {
                std::string actionNumberStr = line.substr(0, seperator);
                int actionNumber = stoi(actionNumberStr);
                std::string actionText = line.substr(seperator + 1);
                Text *text = new Text(actionNumber, actionText);
                actions[actionNumber] = text;
            }
        }
    }
    actions_file.close();

    // Load Loaction Headers
    ifstream location_file("../data/headers.txt");
    if (!location_file.is_open())
    {
        cout << "Unable to open file" << endl;
        exit(1);
    }
    while (getline(location_file, line))
    {
        if (!line.empty())
        {
            if (line[0] == '#')
            {
                number = stoi(line.substr(1));
            }
            else
            {
                Location *location = new Location(number, line);
                locations.push_back(location);
            }
        }
    }

    location_file.close();

    // Load Loaction Descriptions
    ifstream description_file("../data/descriptions.txt");
    if (!description_file.is_open())
    {
        cout << "Unable to open file" << endl;
        exit(1);
    }
    while (getline(description_file, line))
    {
        if (!line.empty())
        {
            if (line[0] == '#')
            {
                number = stoi(line.substr(1));
            }
            else
            {
                Location *location = locations[number - 1];
                location->add_description(line);
            }
        }
    }
    description_file.close();

    // Load Travel
    ifstream travel_file("../data/travel.txt");
    if (!travel_file.is_open())
    {
        cout << "Unable to open file" << endl;
        exit(1);
    }
    int target, action = 0;
    while (getline(travel_file, line))
    {
        if (line.size())
        {
            size_t separator = line.find_first_of(';');
            number = std::stoi(line.substr(0, separator));

            line = line.substr(separator + 1);

            separator = line.find_first_of(';');
            target = std::stoi(line.substr(0, separator));

            line = line.substr(separator + 1);

            separator = line.find_first_of(';');
            while (separator != std::string::npos)
            {
                action = std::stoi(line.substr(0, separator));

                line = line.substr(separator + 1);

                separator = line.find_first_of(';');
                locations[number - 1]->add_destination(target, action);
            }

            action = std::stoi(line);
            locations[number - 1]->add_destination(target, action);
        }
    }
    travel_file.close();

    // Load Objects
    ifstream objects_file("../data/objects.txt");
    if (!objects_file.is_open())
    {
        cout << "Unable to open file" << endl;
        exit(1);
    }
    while (getline(objects_file, line))
    {
        if (!line.empty())
        {
            if (line[0] == '#')
            {
                number = stoi(line.substr(1));
            }
            else
            {
                size_t seperator = line.find_first_of(';');
                if (seperator == std::string::npos)
                {
                    Object *object = new Object(number, line);
                    objects.push_back(object);
                }
                else
                {
                    int msg_nr = stoi(line.substr(0, seperator));
                    line = line.substr(seperator + 1);
                    Object *object = objects[number - 1];

                    object->add_message(msg_nr, line);
                }
            }
        }
    }
    objects_file.close();

    // Load Object Locations
    ifstream object_locations_file("../data/locations.txt");
    if (!object_locations_file.is_open())
    {
        cout << "Unable to open file" << endl;
        exit(1);
    }
    while (getline(object_locations_file, line))
    {
        if (!line.empty())
        {
            size_t seperator = line.find_first_of(';');
            number = stoi(line.substr(0, seperator));
            line = line.substr(seperator + 1);
            seperator = line.find_first_of(';');

            if (seperator == std::string::npos)
            {
                int position = stoi(line);
                if (position > 0)
                {
                    Location *loc = locations.at(position);
                    Object *obj = objects[number - 1];
                    loc->add_object(obj);
                }
            }
            else
            {
                int position1 = stoi(line.substr(0, seperator));
                line = line.substr(seperator + 1);
                int position2 = stoi(line.substr(0));

                if (position1 > 0)
                {
                    Location *loc1 = locations[position1 - 1];
                    Object *obj1 = objects[number - 1];
                    loc1->add_object(obj1);
                }
                if (position2 > 0)
                {
                    Location *loc2 = locations[position2 - 1];
                    Object *obj2 = objects[number - 1];
                    loc2->add_object(obj2);
                }

                Object *obj = objects[number - 1];
                obj->set_movable(false);
            }
        }
    }
    object_locations_file.close();
}

Adventure::~Adventure()
{
    for (vector<Text *>::iterator it = messages.begin(); it != messages.end();
         ++it)
    {
        delete *it;
    }
    for (map<int, Text *>::iterator it = actions.begin(); it != actions.end();
         ++it)
    {
        delete it->second;
    }
    for (vector<Location *>::iterator it = locations.begin();
         it != locations.end(); ++it)
    {
        delete *it;
    }
    for (vector<Object *>::iterator it = objects.begin(); it != objects.end();
         ++it)
    {
        delete *it;
    }
}

void Adventure::start_adventuring()
{
    cout << "\033[2J";
    cout << messages[0]->get() << endl;
    cout << endl
         << "Press a key to start your adventure." << endl;
    cin.get();
    // cin.get();
    int current = 1;
    while (current != -1)
    {
        current = visit(current);
    }
}

int Adventure::visit(int location)
{
    Location *l = locations[location - 1];
    cout << "\033[2J";
    l->print();
    int dest_menu_offset = 1;
    int obj_menu_offset = l->print_destinations(dest_menu_offset);
    int menu_end = l->print_object_actions(obj_menu_offset);

    int target = -1;
    cout << "Select your action: ";
    while (!(std::cin >> target) || std::cin.get() != '\n')
    {
        std::cout << "Error: Flasche Eingabe! Bitte eine Zahl eingeben!" << endl;
        std::cin.clear();
        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
    }
    if (target >= dest_menu_offset && target < obj_menu_offset)
    {
        target = target - dest_menu_offset;
    }
    else if (target >= obj_menu_offset && target < menu_end)
    {
        Object *obj = l->remove_movable_object(target - obj_menu_offset);
        inventory.push_back(obj);
        return location;
    }
    else
    {
        cout << "Please select a action from the list!" << endl;
        cout << endl
             << "Press a key to continue your adventure." << endl;
        cin.get();
        return location;
    }

    Travel *dest = l->get_destination(target);
    // dest->print(-1);
    // cin.get();
    // cin.get();
    int m = dest->get_condition();
    int n = dest->get_target();
    bool condition_fulfilled = true;
    if (m != 0)
    {
        if (m > 0 && m < 100)
        {
            // IT IS DONE WITH M% PROBABILITY
            if (rand() % 100 > m)
            {
                condition_fulfilled = false;
            }
        }
        else if (m == 100)
        {
            // UNCONDITIONAL, BUT FORBIDDEN TO DWARVES.
        }
        else if (m > 100 && m <= 200)
        {
            // HE MUST BE CARRYING OBJECT M-100.
            if (!is_carring(m - 100))
            {
                condition_fulfilled = false;
            }
        }
        else if (m > 200 && m <= 300)
        {
            // MUST BE CARRYING OR IN SAME ROOM AS M-200
            if (!is_carring(m - 200) && !l->has_object(m - 200))
            {
                condition_fulfilled = false;
            }
        }
        else
        {
            // PROP(M MOD 100) MUST *NOT* BE X
            // X = ceil(M / 100.0) - 4
            if (objects[m % 100]->get_state() == ceil(m / 100.0) - 4)
            {
                condition_fulfilled = false;
            }
        }
    }

    if (!condition_fulfilled)
    {
        dest = l->get_alternative_destination(dest);
        n = dest->get_target();
    }

    if (n > 500)
    {
        cout << "\033[2J";
        cin.clear();
        cout << messages[n - 500 - 1]->get() << endl;
        cout << endl
             << "Press a key to continue your adventure." << endl;
        cin.get();
        cin.get();
        return location;
    }
    return n;
}

bool Adventure::is_carring(int object)
{
    std::list<Object *>::iterator begin = inventory.begin();
    std::list<Object *>::iterator end = inventory.end();

    for (; begin != end; ++begin)
    {
        Object *obj = *begin;
        if (object == obj->get_number())
        {
            return true;
        }
    }
    return false;
}