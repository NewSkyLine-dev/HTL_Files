#include "pa.h"
#include <iostream>
#include <algorithm>
#include <vector>

Names::Names(bool useBegin)
{
    this->useBegin = useBegin;
}

bool Names::stringStartsWith(std::string text, std::string begin)
{
    if (text.length() < begin.length())
        return false;
    for (size_t i = 0; i < begin.length(); i++)
    {
        if (text[i] != begin[i])
            return false;
    }
    return true;
}

void Names::printNames(std::string search)
{
    bool found = false;
    std::string praefix = search.substr(0, 1);
    for (const auto &pair : entrys)
    {
        if (search == pair.name)
        {
            std::cout << pair.name << " " << pair.count << std::endl;
            found = true;
        }
        else if (useBegin && stringStartsWith(pair.name, search))
        {
            std::cout << pair.name << " " << pair.count << std::endl;
            found = true;
        }
    }
    if (!found)
        throw std::string("Es wurde kein Name gefunden!");
}

void Names::printNamesSorted(std::string search)
{
    bool found = false;
    std::string praefix = search.substr(0, 1);
    std::vector<Entry> pairs;
    for (const auto &pair : entrys)
    {
        if (search == pair.name)
        {
            std::cout << pair.name << " " << pair.count << std::endl;
            pairs.push_back(pair);
            found = true;
        }
        else if (useBegin && stringStartsWith(pair.name, praefix))
        {
            std::cout << pair.name << " " << pair.count << std::endl;
            pairs.push_back(pair);
            found = true;
        }
    }

    if (!found)
        throw std::string("Es wurde kein Name gefunden!");
    else
    {
        // sort entries by count value
        std::sort(pairs.begin(), pairs.end(),
                  [](const auto &e1, const auto &e2)
                  { return e1.count < e2.count; });

        // print sorted entries
        for (const auto &entry : pairs)
        {
            std::cout << entry.name << " " << entry.count << std::endl;
        }
    }
}