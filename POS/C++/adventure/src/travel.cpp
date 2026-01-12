#include <iostream>

#include "travel.h"
#include "adventure.h"

using namespace std;

Travel::Travel(int target, int action)
{
    this->target = target % 1000;
    this->condition = target / 1000;
    this->action = action;
}

Travel::~Travel()
{
}

void Travel::print(int menu)
{
    cout << menu << ") " << Adventure::get_instance()->get_action(action) << endl;
}