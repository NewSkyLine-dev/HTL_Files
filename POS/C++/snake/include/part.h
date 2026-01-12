#ifndef PART_H
#define PART_H

#include <string>

class Part
{
private:
    int x = 0;
    int y = 0;
    Part *next = nullptr;
    std::string data = "▢";
    int value = 0;
    static std::string unicode_letters[26];

public:
    Part(int x, int y, std::string d, int value = 0);
    ~Part();
    void draw();
    void move(int x, int y);
    bool contains(int x, int y);
    bool contains_all_letters(int v); // Erweiterung
    Part *remove_and_move(int v);
    Part *add(Part *p);
    void eat();
    Part *get(int x, int y);
    Part *remove(int x, int y);
};

#endif