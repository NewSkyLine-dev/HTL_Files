#include <iostream>

#include "adventure.h"

using namespace std;

int main(int argc, char **argv)
{
    Adventure::get_instance()->start_adventuring();
    return 0;
}