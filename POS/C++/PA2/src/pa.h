#ifndef PA_H
#define PA_H

#include <string>
#include "angabedaten.h"

struct Entry
{
    std::string name;
    int count;
};

class Names
{
private:
    int entryCount = ANGABEANZAHL;
    Entry entrys[ANGABEANZAHL] = ANGABEDATEN;
    bool useBegin = false;

public:
    Names(bool useBegin);
    bool stringStartsWith(std::string text, std::string begin);
    void printNames(std::string search);
    void printNamesSorted(std::string search);
};

#endif // PA_H